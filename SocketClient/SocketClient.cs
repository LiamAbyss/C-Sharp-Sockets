using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class SocketClient
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
        public static void StartClient()
        {
            byte[] bytes = new byte[1024];

            // Connect to a Remote server  
            // Get Host IP Address that is used to establish a connection  
            // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
            // If a host has multiple addresses, you will get a list of addresses  

            // Create a TCP/IP  socket.    
            Socket sender = new TcpClient().Client;

            // Connect the socket to the remote endpoint. Catch any errors. 
            Console.WriteLine("Connecting...");
            // Connect to Remote EndPoint
            sender.Connect("127.0.0.1", 5789);


            Console.WriteLine("Socket connected to {0}",
                sender.RemoteEndPoint.ToString());

            // Encode the data string into a byte array.
            byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");
                    
            // Send the data through the socket.    
            int bytesSent = sender.Send(msg);

            // Receive the response from the remote device.    
            int bytesRec = sender.Receive(bytes);
            Console.WriteLine("Echoed test = {0}",
                Encoding.ASCII.GetString(bytes, 0, bytesRec));

            Console.WriteLine("\n Press any key to continue...");
            Console.ReadKey();
            // Release the socket.    
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
    }
}
