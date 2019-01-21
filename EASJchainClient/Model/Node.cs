using EASJchainClient.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EASJchainClient.Model {
	public class Node {
		public IPEndPoint _endPoint;

		public Node(IPEndPoint endPoint) {
			_endPoint = endPoint;
			Task.Factory.StartNew(() => Receive());
		}


		//really bad snowflake code
		//should be standardised with the rest of the send-protocols but whatever
		public void NegotiateNodeList() {
			Send("getnodeports");
		}

		public void Send(string message) {
			byte[] sendBytes = Encoding.ASCII.GetBytes(message);
			Client.GetClient().Send(sendBytes, sendBytes.Length, _endPoint);
		}

		public void Receive() {
			byte[] receiveBytes = Client.GetClient().Receive(ref _endPoint);
			string receivedData = Encoding.ASCII.GetString(receiveBytes);
			Console.WriteLine("MESSAGE: " + receivedData);
			string[] messageWords = receivedData.Split(' ');
			string messageFirstWord = messageWords[0];
			string messageArguments = String.Join(" ", messageWords, 1, messageWords.Count() - 2);

			switch (messageFirstWord) {
				case "transactionprotocol":
					//process transaction if mining
					if (Client.IsMining) {
						Transaction t = new Transaction(messageWords[1], messageWords[2], messageWords[3], messageWords[4]);
						Functionality.ProcessTransaction(t);
					}
					break;
				case "getnodeports":
					List<string> nodePorts = new List<string>();
					foreach(Node n in NodeList.GetNodes()) {
						nodePorts.Add(n._endPoint.Port.ToString());
					}
					string nodesString = String.Join(",", nodePorts);
					Send("nodeportlist" + nodesString);
					break;
				case "nodeportlist":
					if(messageWords.Length == 1) {
						break;
					}
					ConnectToNode.ConnectToNodeList(messageWords[1]);
					break;
			}
		}
	}
}
