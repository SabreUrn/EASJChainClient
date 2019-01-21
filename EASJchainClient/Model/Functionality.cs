using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EASJchainClient.Common;
using EASJchainClient.Configuration;

namespace EASJchainClient.Model {
	public static class Functionality {
		static CancellationToken mineCancelToken = new CancellationToken();


		//public Functionality() {
		//	Task.Factory.StartNew(() => RunFunctionality());
		//}


		public static void RunFunctionality() {
			//Task.Factory.StartNew(() => Receive());
			while(true) {
				string message = Console.ReadLine();
				string[] messageWords = message.Split(' ');
				string messageFirstWord = messageWords[0];
				string messageArguments = String.Join(" ", messageWords, 1, messageWords.Count() - 1);

				switch(messageFirstWord) {
					case "debugsend":
						Client.Send(message);
						break;
					case "mine":
						//do mining
						Task.Factory.StartNew(() => Mine());
						break;
					case "printmine":
						//print current mining results
						break;
					case "stopmine":
						//stop mining thread
						break;
					case "dotransaction":
						//transaction protocol:
						//"transactionprotocol" +
						//scriptSig +
						//pubkey +
						//arbitrary data
						string parametersString = ObjectStringConversion.ObjectToString(Client.ParametersPublic);
						Transaction transaction = new Transaction(Client.ScriptSig, Client.PublicKey, parametersString, messageArguments);
						Client.Send("transactionprotocol " + transaction);
						break;
				}
			}
		}

		private static void Mine() {
			Client.IsMining = true;
			while(Client.IsMining) {
				//Console.WriteLine("simulated mining");
			}
			//Blockchain.GetLastBlock().Mine();
		}

		public static void ProcessTransaction(Transaction transaction) {
			Console.WriteLine("DEBUG: transaction processing started");
			UTF8Encoding encoder = new UTF8Encoding();

			//decrypt scriptSig with public key and verify it matches transaction.PublicKey
			bool success = false;

			RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(3072) {
				PersistKeyInCsp = false
			};
			byte[] verifyBytes = encoder.GetBytes(transaction.PublicKey);
			byte[] signedBytes = Convert.FromBase64String(transaction.ScriptSig);

			try {
				RSAParameters parameters = (RSAParameters)ObjectStringConversion.StringToObject(transaction.Parameters);
				rsa.ImportParameters(parameters);
				SHA512Managed hash = new SHA512Managed();
				byte[] hashedBytes = hash.ComputeHash(signedBytes);
				success = rsa.VerifyData(verifyBytes, CryptoConfig.MapNameToOID("SHA512"), signedBytes);
				Console.WriteLine("DEBUG: success = " + success);
				Console.WriteLine("DEBUG: signedBytes = " + BitConverter.ToString(signedBytes));
			} catch(CryptographicException e) {
				Console.WriteLine("ProcessTransaction():");
				Console.WriteLine(e.Message);
			}

			if (success) { //we can be reasonably certain that the sender is who they claim to be
				Blockchain.GetLastBlock().AddTransaction(transaction);
				Console.WriteLine("DEBUG: transaction successful");
				//announce
			}
			Console.WriteLine("DEBUG: transaction happened");
		}
	}
}
