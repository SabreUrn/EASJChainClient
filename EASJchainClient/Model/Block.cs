using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EASJchainClient.Configuration;

namespace EASJchainClient.Model {
	public class Block {
		private int _magicNumber = 835542410; //0x31CD5D8A
		private int _blockSize;
		private Block _prevBlock;
		private BlockHeader _header;
		private int _transactionsCount;
		private int _transactionsMax;
		private List<Transaction> _transactions;

		public Block() {
			ConstructorCommon();
		}
		
		public Block(Block prevBlock) {
			_prevBlock = prevBlock;
			ConstructorCommon();
			_header.HashPrevBlock = BitConverter.GetBytes(_prevBlock.GetHashCode());
		}

		private void ConstructorCommon() {
			_blockSize = 0;
			_transactions = new List<Transaction>();
			_transactionsCount = _transactions.Count;
			_transactionsMax = Configuration.Configuration.TransactionsMax;
			_header = new BlockHeader();
		}


		public void Mine() {
			bool lastRun = false;

			//hash random value
			SHA256 sha256 = SHA256.Create();
			Random rnd = new Random();
			int valueToHash = rnd.Next(Int32.MinValue, Int32.MaxValue);
			byte[] hash = sha256.ComputeHash(BitConverter.GetBytes(valueToHash));

			//update _header.Nonce to it
			_header.Nonce = hash;

			//check if it's lower than _header.Target
			int comparison = ((IStructuralComparable)_header.Nonce).CompareTo(_header.Target, Comparer<byte>.Default);
			if(comparison < 0) {
				SolveBlock();
			} else { //announcing solved block is handled in that method
				//announce
			}
		}

		private void Update() {
			UpdateTransactionsCount();
			UpdateMerkleRoot();
		}

		private void UpdateTransactionsCount() {
			_transactionsCount = _transactions.Count;
		}

		private void UpdateMerkleRoot() {
			SHA256 sha256 = SHA256.Create();
			if(_transactions.Count == 0) {
				_header.HashMerkleRoot = null;
				//announce
				return;
			}
			if(_transactions.Count == 1) {
				int transactionHash = _transactions[0].GetHashCode();
				byte[] transactionBytes = BitConverter.GetBytes(transactionHash);
				_header.HashMerkleRoot = sha256.ComputeHash(transactionBytes);
				//announce
				return;
			}

			//23 and 31 are primes
			int hash = 23;
			foreach(Transaction t in _transactions) {
				hash = hash * 31 + _transactions.GetHashCode();
			}
			byte[] hashBytes = BitConverter.GetBytes(hash);
			_header.HashMerkleRoot = hashBytes;

			//announce
		}

		public void AddTransaction(Transaction t) {
			_transactions.Add(t);

			Update();
			if(_transactionsCount >= _transactionsMax) {
				SolveBlock();
			}
		}

		private void SolveBlock() {
			//stop mining block
			//add new block to blockchain
			//announce
			//start mining new block
		}
	}
}
