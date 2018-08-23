using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Udp.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const int port = 8080;
            //var address = await Dns.GetHostAddressesAsync("localhost");

            string message = "This is a test message";
            var data = Encoding.ASCII.GetBytes(message);

            using (var socket = new UdpClient(AddressFamily.InterNetworkV6))
            {
                // Enable Dual Mode, which means the Socket can listen on both IPv4 and IPv6 at the same time.
                socket.Client.DualMode = true;

                socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                
                var ipv4Address = "127.0.0.1"; // IPAddress.Loopback
                socket.Connect(IPAddress.Parse(ipv4Address), port);
                await socket.SendAsync(data, data.Length);
                
                var ipv6Address = "::1"; // IPAddress.IPv6Loopback
                socket.Connect(IPAddress.Parse(ipv6Address), port);
                await socket.SendAsync(data, data.Length);
            }
            Console.WriteLine("Done sending");
        }
    }
}