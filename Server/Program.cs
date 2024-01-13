using System.Net;
using System.Net.Sockets;
using System.Text;
using MainLogic;
using Server;

// int port = 25565;
//
// Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
// server.Bind(new IPEndPoint(IPAddress.Any, port));
// server.Listen(int.MaxValue);
//
// Console.WriteLine("Wait for 1st player...");
// Socket player1 = server.Accept();
// Console.WriteLine("Connected");
//
// byte[] buffer = new byte[1024];
// bool isAllPlayersConnected = false;
// buffer[0] = Convert.ToByte(isAllPlayersConnected);
// player1.Send(buffer);
//     
// Console.WriteLine("Wait for 2nd player...");
// Socket player2 = server.Accept();
// Console.WriteLine("Connected");
//
// isAllPlayersConnected = true;
// buffer[0] = Convert.ToByte(isAllPlayersConnected);
// player1.Send(buffer);
// player2.Send(buffer);
//
//
// Console.WriteLine("Start");
// while (true)
// {
//     
// }
//
// async Task PrintObtainedInfoAsync(Socket player)
// {
//     while (true)
//     {
//         byte[] buffer = new byte[1024];
//         var received = await player.ReceiveAsync(buffer, SocketFlags.None);
//         Console.WriteLine($"\nObtained {player.Handle}");
//
//         var response = Encoding.UTF8.GetString(buffer, 0, received);
//
//
//         Console.WriteLine($"Server received message: \"{response}\" \n\n");
//     }
// }

GameServer server = new GameServer();
server.ConnectPlayer();


Console.WriteLine("Start");


await server.ConnectPlayer();

while (server.IsAllPlayersConnected() == false)
{
    
}

while (true)
{
    
    server.HandleCoordinates();
}




Console.WriteLine("Stop");





