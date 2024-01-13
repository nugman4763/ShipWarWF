using System.Collections;
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
    private Dictionary<IntPtr, ShipDetector> _contrPlayersCells;
    private Socket _server;
    private ShipsValidator _shipsValidator;
    private ShipsParser _shipsParser;
    private BytesConverter _bytesConverter;
    private int _currentPlayerId;
    private ShipSettings _settings;

    public GameServer()
    {
        int port = 25565;
        string address = "127.0.0.1";

        _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _server.Bind(new IPEndPoint(IPAddress.Parse(address), port));
        _server.Listen(int.MaxValue);
        
        _players = new List<Socket>();
        _playersData = new Dictionary<IntPtr, List<Ship>>();
        _contrPlayersCells = new Dictionary<IntPtr, ShipDetector>();

        _currentPlayerId = 0;

         _settings = new ShipSettings(
            4,
            0,
            0,
            4
        );

        _shipsValidator = new ShipsValidator(_settings);
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
            
        }


        _players.Add(currentPlayer);

        foreach (var player in _players)
        {
            SendConfirmation(player, IsAllPlayersConnected());
        }

        

        if (IsAllPlayersConnected())
        {
            bool currentTurn = true;

            _contrPlayersCells.Add(_players[0].Handle, new ShipDetector(_playersData[_players[1].Handle],
                _settings));
            _contrPlayersCells.Add(_players[1].Handle, new ShipDetector(_playersData[_players[0].Handle],
                _settings));

            SendConfirmation(_players[0], currentTurn);
            SendConfirmation(_players[1], !currentTurn);
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

    public void HandleCoordinates()
    {
        var player = _players[_currentPlayerId];

        while (player.Available > 0)
        {
            byte[] discardBuffer = new byte[player.Available];
            player.Receive(discardBuffer);
        }

        byte[] bytesCoordinate = new byte[1024];
        player.Receive(bytesCoordinate);


        string stringCoordinate = Encoding.UTF8.GetString(bytesCoordinate);


        string[] parts = stringCoordinate.Split(' ');
        int[] coordinates = new int[parts.Length];

        for (int i = 0; i < parts.Length; i++)
        {

            if (!int.TryParse(parts[i], out coordinates[i]))
            {
                throw new Exception ($"Не удалось преобразовать '{parts[i]}' в число.");
            }
        }
        Console.WriteLine($"{coordinates[0]} - {coordinates[1]}");


            
            

        bool isHitting = _contrPlayersCells[player.Handle]
            .IsDetected(coordinates[0], coordinates[1]);

        var cellInfo = new CellInfoJson();

        cellInfo.X = coordinates[0].ToString();
        cellInfo.Y = coordinates[1].ToString();
        cellInfo.IsHitted = isHitting.ToString();
        cellInfo.IsWin = _contrPlayersCells[player.Handle].CheckWin().ToString();

        Console.WriteLine($"{player.Handle} - ({cellInfo.X},{cellInfo.Y})");

        if (isHitting == false)
        {
            ChangeId();
        }

        byte[] bytesCell = _bytesConverter.ConvertCellToBytes(cellInfo);
        
        foreach(var client in _players)
        {
            client.Send(bytesCell);
        }


        
    }

    private void ChangeId()
    {
        if (_currentPlayerId == 1)
        {
            _currentPlayerId = 0;
        }
        else
        {
            _currentPlayerId = 1;
        }
    }


}