using System.Net;
using System.Net.Sockets;
using System.Text;
using MainLogic;

namespace Server;

public class GameServer
{
    private bool _isAllPlayersConnected;
    private List<Socket> _players;
    private Dictionary<IntPtr, List<Ship>> _playersData;
    private Socket _server;
    private ShipsValidator _shipsValidator;
    private ShipsParser _shipsParser;
    private BytesConverter _bytesConverter;

    public GameServer()
    {
        int port = 25565;
        string address = "127.0.0.1";

        _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _server.Bind(new IPEndPoint(IPAddress.Parse(address), port));
        _server.Listen(int.MaxValue);
        
        _players = new List<Socket>();
        _playersData = new Dictionary<IntPtr, List<Ship>>();
        
        var settings = new ShipSettings(
            0,
            2,
            0,
            0
        );

        _shipsValidator = new ShipsValidator(settings);
        _shipsParser = new ShipsParser();
        _bytesConverter = new BytesConverter();

    }

    public async Task ConnectPlayer()
    {
        Socket currentPlayer = await _server.AcceptAsync();
        _playersData.Add(currentPlayer.Handle, new List<Ship>());

        bool isAccepted = false;
        while (isAccepted == false)
        {
            isAccepted = GetShipData(currentPlayer);
            SendConfirmation(currentPlayer,isAccepted);
            Console.WriteLine(isAccepted);
        }

        _players.Add(currentPlayer);

        foreach (var player in _players)
        {
            SendConfirmation(player, IsAllPlayersConnected());
        }
    }

    private void SendConfirmation(Socket player, bool confirmation)
    {
        byte[] byteConfirmation =
            BitConverter.GetBytes(confirmation);
        player.Send(byteConfirmation);
    }

    public bool IsAllPlayersConnected()
    {
        return _players.Count == 2;
    }
    
    private bool GetShipData(Socket player)
    {
        
        List<byte[]> receivedData = new List<byte[]>();

        do
        {
            byte[] buffer = new byte[1024];
            player.Receive(buffer);
            receivedData.Add(buffer);
        } while (player.Available > 0);
        
        
        int totalLength = receivedData.Sum(ba => ba.Length);
        
        Console.WriteLine($"Available {totalLength}");
        byte[] received = new byte[totalLength];
        
        int offset = 0;
        foreach (var byteArray in receivedData)
        {
            Buffer.BlockCopy(byteArray, 0, received, offset, byteArray.Length);
            offset += byteArray.Length;
        }
        
        List<ShipDataJson> shipDataJsons = _bytesConverter.ConvertBytesToShip(received);
        List<Ship> ships = _shipsParser.Execute(shipDataJsons);

        bool isMatch = _shipsValidator.Execute(ships);
        if (isMatch)
        {
            _playersData[player.Handle] = ships;
            return true;
        }
        return false;
        
    }

    public void SendAllPlayersConnected()
    {
        foreach (var player in _players)
        {
            byte[] isAllPlayersConnectedByte = BitConverter.GetBytes(_isAllPlayersConnected);
            player.Send(isAllPlayersConnectedByte);  
        }
    }
}