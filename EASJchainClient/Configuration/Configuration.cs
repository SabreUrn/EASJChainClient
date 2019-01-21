using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASJchainClient.Configuration {
	public static class Configuration {
		public static readonly int SelfPort = 40200;
		public static readonly int TransactionsMax = 10;
		public static readonly byte[] TargetLow = new byte[32] { 0, 0, 0, 0, 0, 0, 0, 0,
																 0, 0, 0, 0, 0, 0, 0, 0,
																 0, 0, 0, 0, 0, 0, 0, 0,
																 0, 0, 0, 0, 0, 255, 255, 255 };
		public static readonly byte[] TargetHigh = new byte[32] { 255, 0, 0, 0, 0, 0, 0, 0,
																  0, 0, 0, 0, 0, 0, 0, 0,
																  0, 0, 0, 0, 0, 0, 0, 0,
																  0, 0, 0, 0, 0, 0, 0, 0 };
	}
}
