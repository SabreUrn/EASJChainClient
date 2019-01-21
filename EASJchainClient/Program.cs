using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EASJchainClient.Model;
using EASJchainClient.Network;

namespace EASJchainClient {
	class Program {
		static void Main(string[] args) {
			Console.Write("Enter port number to use: ");
			int selfPort = Int32.Parse(Console.ReadLine());
			//UdpClient selfSocket = new UdpClient(selfPort);

			Console.Write("Enter port number to connect to: ");
			int remotePort = Int32.Parse(Console.ReadLine());
			IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), remotePort);

			//debug code
			Blockchain.Blocks = new List<Block> { new Block() };
			//debug code end
		

			Client.InitClient(selfPort);
			ConnectToNode.Connect(remotePort);
			ConnectToNode.GetNodes(remotePort, selfPort);
			Task.Factory.StartNew(() => Functionality.RunFunctionality());
			//Functionality func = new Functionality(remoteIpEndPoint, selfPort);

			while(true) {

			}
		}
	}
}
