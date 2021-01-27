using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Listener
{
    class SocketListener
    {
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static void StartServer()
        {
            // Get Host IP Address that is used to establish a connection  
            // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
            // If a host has multiple addresses, you will get a list of addresses

            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 5789);

            Console.WriteLine("Listening for connection...");
            listener.Start();
            Socket handler = listener.AcceptTcpClient().Client;
            listener.Stop();
            // Incoming data from the client.    
            string data = null;
            byte[] bytes = null;

            while (true)
            {
                bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                if (data.IndexOf("<EOF>") > -1)
                {
                    break;
                }
            }

            Console.WriteLine("Text received from {0} : {1}", handler.RemoteEndPoint.ToString(), data);

            byte[] msg = Encoding.ASCII.GetBytes(data);
            handler.Send(msg);
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
            Console.WriteLine("\n Press any key to continue...");
            Console.ReadKey();
        }        
    }
}
