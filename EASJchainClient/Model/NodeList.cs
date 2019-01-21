using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASJchainClient.Model {
	public static class NodeList {
		private static List<Node> _list = new List<Node>();

		public static void Add(Node n) {
			_list.Add(n);
		}

		public static void RemoveByNode(Node n) {
			_list.Remove(n);
		}

		public static int Count() {
			return _list.Count;
		}

		public static List<Node> GetNodes() {
			return _list;
		}

		public static Node GetNodeByIndex(int index) {
			return _list[index];
		}
	}
}
