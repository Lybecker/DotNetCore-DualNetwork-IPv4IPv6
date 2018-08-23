using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tcp.Client
{
    class Program
    {
        const int PORT = 8080;

        static async Task Main(string[] args)
        {
            TcpClient socket = new TcpClient(AddressFamily.InterNetworkV6);
            socket.Client.DualMode = true;
            var ipv4Address = IPAddress.Parse("127.0.0.1"); // IPAddress.Loopback
            await ConnectSendReceive(socket, ipv4Address);

            socket = new TcpClient(AddressFamily.InterNetworkV6);
            socket.Client.DualMode = true;
            var ipv6Address = IPAddress.Parse("::1"); // IPAddress.IPv6Loopback
            await ConnectSendReceive(socket, ipv6Address);

        }

        static async Task ConnectSendReceive(TcpClient socket, IPAddress ipAddress)
        {
            Console.WriteLine($"Connecting to {ipAddress}...");
            await socket.ConnectAsync(ipAddress, PORT);

            string message = "This is a test message";
            var data = Encoding.ASCII.GetBytes(message);

            using (NetworkStream ns = socket.GetStream())
            {
                Console.WriteLine("Writing to server");
                await ns.WriteAsync(data, 0, data.Length);

                byte[] bytes = new byte[1024];
                int bytesRead = await ns.ReadAsync(bytes, 0, bytes.Length);
                Console.WriteLine($"Response '{Encoding.ASCII.GetString(bytes, 0, bytesRead)}");
            }

            socket.Close();
        }
    }
}