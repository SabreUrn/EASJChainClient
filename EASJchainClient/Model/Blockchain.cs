using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASJchainClient.Model {
	public static class Blockchain {
		public static List<Block> Blocks;

		public static Block GetLastBlock() {
			return Blocks[Blocks.Count - 1];
		}
	}
}
