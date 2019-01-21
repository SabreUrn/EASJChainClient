using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EASJchainClient.Model {
	public class Transaction {
		public string ScriptSig { get; set; }
		public string PublicKey { get; set; }
		public string Parameters { get; set; }
		public string ArbitraryData { get; set; }


		public Transaction(string scriptSig, string pubKey, string parameters, string data) {
			ScriptSig = scriptSig;
			PublicKey = pubKey;
			Parameters = parameters;
			ArbitraryData = data;
		}

		public override string ToString() {
			return ScriptSig + " " + PublicKey + " " + Parameters + " " + ArbitraryData;
		}
	}
}
