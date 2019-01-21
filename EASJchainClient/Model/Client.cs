using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EASJchainClient.Model {
	public static class Client {
		private static UdpClient _client;
		public static RSACryptoServiceProvider KeyPair { get; set; }
		public static string ScriptSig { get; set; }
		public static string PublicKey { get; set; }
		public static string PrivateKey { get; set; }
		public static RSAParameters ParametersPublic { get; set; }
		public static RSAParameters ParametersPrivate { get; set; }
		public static bool IsMining { get; set; }

		public static UdpClient GetClient() {
			return _client;
		}

		public static void InitClient(int selfPort) {
			_client = new UdpClient(selfPort);
			KeyPair = new RSACryptoServiceProvider(3072) {
				PersistKeyInCsp = false
			};
			PublicKey = KeyPair.ToXmlString(false);
			PrivateKey = KeyPair.ToXmlString(true);
			ParametersPublic = KeyPair.ExportParameters(false);
			ParametersPrivate = KeyPair.ExportParameters(true);
			//ScriptSig = Encoding.ASCII.GetString(KeyPair.Encrypt(Encoding.ASCII.GetBytes(PublicKey), true)); //pubkey to byte array, back to string
			SignSig(PublicKey, PrivateKey);
			IsMining = false;
		}


		public static void Send(string message) {
			foreach(Node n in NodeList.GetNodes()) {
				n.Send(message);
			}
		}


		private static void SignSig(string pubKey, string privKey) {
			RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(3072);

			byte[] signedBytes;
			UTF8Encoding encoder = new UTF8Encoding();
			byte[] originalBytes = encoder.GetBytes(pubKey);

			try {
				rsa.ImportParameters(ParametersPrivate);
				signedBytes = rsa.SignData(originalBytes, CryptoConfig.MapNameToOID("SHA512"));
			} catch(CryptographicException e) {
				Console.WriteLine("SignSig():");
				Console.WriteLine(e.Message);
				Console.ReadKey();
				return;
			} finally {
				rsa.PersistKeyInCsp = false;
			}

			Console.WriteLine("DEBUG: signedBytes = " + BitConverter.ToString(signedBytes));
			ScriptSig = Convert.ToBase64String(signedBytes);
			Console.WriteLine("DEBUG: SignSig() end");
		}
	}
}
