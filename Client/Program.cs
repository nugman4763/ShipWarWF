

using System.Net;
using System.Net.Sockets;
using System.Text;

int port = 25565;
Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));

Thread.Sleep(20000);
// while (true)
// {
//     string input = Console.ReadLine();
//     byte[] buffer = Encoding.UTF8.GetBytes(input);
//     client.Send(buffer);
// }


    
// Console.WriteLine("Wait for data");
// var received = await client.ReceiveAsync(buffer, SocketFlags.None);
// Console.WriteLine("Obtained");

// var response = Encoding.UTF8.GetString(buffer, 0, received);
//
//
// Console.WriteLine($"Server received message: \"{response}\"");

// string input = "Hello"; // Получение координат от пользователя
// byte[] buffer = Encoding.UTF8.GetBytes(input); // Преобразование строки в байтовый массив
// Thread.Sleep(6000);
// client.Send(buffer);


