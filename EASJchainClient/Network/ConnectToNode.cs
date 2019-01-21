using EASJchainClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EASJchainClient.Network {
	public static class ConnectToNode {
		public static void Connect(int port) {
			IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
			Node node = new Node(remoteIpEndPoint);
			NodeList.Add(node);
		}

		public static void GetNodes(int port, int selfPort) {
			Node gatewayNode = NodeList.GetNodeByIndex(0);
		}

		public static void ConnectToNodeList(string nodesString) {
			List<string> nodeListString = nodesString.Split(',').ToList();
			List<int> nodeListInt = new List<int>();
			foreach(string port in nodeListString) {
				nodeListInt.Add(Int32.Parse(port));
			}

			List<int> existingPorts = new List<int>();
			foreach(Node n in NodeList.GetNodes()) {
				existingPorts.Add(n._endPoint.Port);
			}

			foreach(int port in nodeListInt) {
				int selfPort = ((IPEndPoint)Client.GetClient().Client.RemoteEndPoint).Port; //casting remote endpoint in socket to IPEndPoint to get port

				if(!existingPorts.Contains(port) && port != selfPort) {
					Connect(port);
				}
			}
		}
	}
}
