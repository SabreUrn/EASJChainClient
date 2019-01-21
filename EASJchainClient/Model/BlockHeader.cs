using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EASJchainClient.Model {
	public class BlockHeader {
		public byte[] HashPrevBlock { get; set; }
		public byte[] HashMerkleRoot { get; set; }
		public DateTime Timestamp { get; set; }
		public byte[] Target { get; set; }
		public byte[] Nonce { get; set; }

		public BlockHeader() {
			Timestamp = DateTime.Now;
			Target = Configuration.Configuration.TargetHigh;
		}
	}
}
