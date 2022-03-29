using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Crypto_system_17180
{
    class Solitaire
    {
		private static List<char> CHARACTERS = new List<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
		private static Deck deck = new Deck();
		private static List<char> KEY = new List<char>();
		
		public Solitaire()
		{
			
		}

		private static bool Cipher(string input, string fileName, out string output, bool EnCipher, bool pcbc)
		{
			output = String.Empty;
			string VI = String.Empty;
			List<int> ciphKey = new List<int>();
			if (EnCipher)
			{
				GenerateKeyStream(out ciphKey, 10);
			}
			else
			{
				if (!FileSystemAPI.GetKey(out KEY,fileName,"S"))
				{
					return false;
                }
                else
                {
					if(KEY.Count > 10)
                    {
						for(int i = 10; i< KEY.Count; i++)
                        {
							VI += KEY[i];
                        }
						KEY.RemoveRange(10, KEY.Count - 10);
                    }
					GetLetters(out ciphKey, KEY);
                }
			}
			if (pcbc == true)
			{
				List<string> inputBlocks = BlockBuilder.StringToList(input, 10);
				List<string> outputBlocks = new List<string>();
				if (EnCipher)
				{
					string pcbcResultBlock = PCBC.BlockPropagation("", inputBlocks[0], "",EnCipher,ref VI);

					string cipherResultBlock = BlockCipher(pcbcResultBlock, ciphKey, EnCipher);

					outputBlocks.Add(cipherResultBlock);

					output += cipherResultBlock;

					for (int i = 1; i < inputBlocks.Count; i++)
					{

						pcbcResultBlock = PCBC.BlockPropagation(inputBlocks[i - 1], inputBlocks[i], outputBlocks[i - 1],EnCipher, ref VI);

						cipherResultBlock = BlockCipher(pcbcResultBlock, ciphKey, EnCipher);

						outputBlocks.Add(cipherResultBlock);

						output += cipherResultBlock;

					}
					KEY.AddRange(VI);
					FileSystemAPI.SaveKey(KEY, fileName, "S");
				}
				else
				{
					string cipherResultBlock = BlockCipher(inputBlocks[0], ciphKey, EnCipher);

					string pcbcResultBlock = PCBC.BlockPropagation("", cipherResultBlock, "", EnCipher,ref VI);

					outputBlocks.Add(pcbcResultBlock);

					output += pcbcResultBlock;

					for (int i = 1; i < inputBlocks.Count; i++)
					{

						cipherResultBlock = BlockCipher(inputBlocks[i], ciphKey, EnCipher);

						pcbcResultBlock = PCBC.BlockPropagation(inputBlocks[i - 1], cipherResultBlock, outputBlocks[i - 1],EnCipher, ref VI);

						outputBlocks.Add(pcbcResultBlock);

						output += pcbcResultBlock;

					}
				}
			}
			else
				output = BlockCipher(input, ciphKey, EnCipher);
			return true;
		}

		public static bool Encipher(string input, string fileName, out string output,bool pcbc)
		{
			return Cipher(input, fileName, out output, true, pcbc);
		}

		public static bool Decipher(string input, string fileName, out string output, bool pcbc)
		{
			return Cipher(input, fileName, out output, false, pcbc);
		}

		private static void GetLetters(out List<int> intText, List<char> input)
		{
			intText = new List<int>();
			foreach (char c in input)
			{
				if (char.IsLetter(c))
				{
					intText.Add(CHARACTERS.IndexOf(char.ToUpper(c)) + 1);
				}
                else
                {
					intText.Add((int)c);
                }
                
			}
		}

		private static string BlockCipher(in string inputBlock, List<int> chiphKey, bool encdec)
        {
			string output = "";
			List<int> chiphText = new List<int>();
		
			GetLetters(out chiphText, new List<char>(inputBlock));
			
			for (int i = 0; i < chiphText.Count; i++)
			{
				if (char.IsLetter(inputBlock[i])) // if character in input text isnot a letter it stays as it is
				{
					if (char.IsLower(inputBlock[i]))
					{
						if(encdec == true)
							output += char.ToLower(CHARACTERS[Mod((chiphText[i] + chiphKey[i % 10] - 1), 26)]);
						else
							output += char.ToLower(CHARACTERS[Mod((chiphText[i] - chiphKey[i % 10] - 1), 26)]);

					}
					else
					{
						if (encdec == true)
							output += CHARACTERS[Mod((chiphText[i] + chiphKey[i % 10] - 1), 26)];
						else
							output += CHARACTERS[Mod((chiphText[i] - chiphKey[i % 10] - 1), 26)];
					}
				}
				else
				{
					output += (char)chiphText[i];
				}
			
			}
			return output;
		}

		/**
		 *  Output value of function is list of int values of key stream but this
		 *  function also changes atribute KEY (List<char>)
		 */
		public static void GenerateKeyStream(out List<int> keyStreem, int numOfKeyStreams)
        {
			keyStreem = new List<int>();
			KEY = new List<char>();
			for (int i = 0; i < numOfKeyStreams; i++)
			{
				//1st step
				deck.shuffle(100);
				//2nd step
				deck.MoveJoker("JA", 1); //JA stands for Joker A
				//3rd step
				deck.MoveJoker("JB", 2);// JB stands for Joker B
				//4th step
				deck.TripleCut();
				//5th step
				deck.countCut();
				//6th step
				Card card = deck.ouputCard();
				if (card != null)
				{
					keyStreem.Add(card.toInt26());
					KEY.Add(CHARACTERS[card.toInt26()-1]);
				}
				else
				{
					i--;
				}
			}
		}

		public static int Mod(int a, int n)
		{
			return a - (int)Math.Floor((double)a / n) * n;
		}

	}
}
