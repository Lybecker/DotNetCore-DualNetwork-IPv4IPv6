using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var endpoint = new IPEndPoint(IPAddress.IPv6Any, 8080);

            // I'm using the UdpClient as a server in this case like https://docs.microsoft.com/en-us/dotnet/framework/network-programming/using-udp-services
            var listener = new UdpClient(AddressFamily.InterNetworkV6);

            // Enable Dual Mode, which means the Socket can listen on both IPv4 and IPv6 at the same time.
            listener.Client.DualMode = true;
            // Setting the DualMode to true, is the same as doing the below:
            //listener.Client.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

            // Start listening on the endpoint
            listener.Client.Bind(endpoint);
            
            // The listender Client can only show one endpoint, but Socket is listening on both IPv4 and IPv6
            Console.WriteLine($"Listining for data on {listener.Client.LocalEndPoint} ...");
            while (true)
            {
                var result = await listener.ReceiveAsync();
                
                string data = Encoding.ASCII.GetString(result.Buffer);

                var address = result.RemoteEndPoint.Address.IsIPv4MappedToIPv6 ? result.RemoteEndPoint.Address.MapToIPv4() : result.RemoteEndPoint.Address;

                Console.WriteLine($"Received data '{data}' from {address}");
            }
        }
    }
}