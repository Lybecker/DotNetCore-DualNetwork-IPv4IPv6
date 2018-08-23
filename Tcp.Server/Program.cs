using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tcp.Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var endpoint = new IPEndPoint(IPAddress.IPv6Any, 8080);

            var listener = new TcpListener(endpoint);
            // Enable Dual Mode, which means the Socket can listen on both IPv4 and IPv6 at the same time.
            listener.Server.DualMode = true;
            // Setting the DualMode to true, is the same as doing the below:
            //listener.Server.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            listener.Start();

            while (true)
            {
                // The listender Client can only show one endpoint, but Socket is listening on both IPv4 and IPv6
                Console.Write($"Waiting for connections {listener.LocalEndpoint} ...");
                TcpClient client = await listener.AcceptTcpClientAsync();

                Console.WriteLine($"Connection accepted.");

                using (NetworkStream ns = client.GetStream())
                {
                    byte[] bytes = new byte[1024];
                    int bytesRead = await ns.ReadAsync(bytes, 0, bytes.Length);
                    var message = Encoding.ASCII.GetString(bytes, 0, bytesRead);

                    var remoteEndpoint = ((IPEndPoint)client.Client.RemoteEndPoint);
                    var address = remoteEndpoint.Address.IsIPv4MappedToIPv6 ? remoteEndpoint.Address.MapToIPv4() : remoteEndpoint.Address;
                    Console.WriteLine($"Received data '{message}' from {address}");

                    byte[] byteTime = Encoding.ASCII.GetBytes($"Thanks for the message at {DateTime.Now.ToString()}");
                    ns.Write(byteTime, 0, byteTime.Length);
                }
                client.Close();
            }

            //listener.Stop();
        }
    }
}