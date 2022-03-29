using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace Crypto_system_17180
{
	class SimpleSubstitution
	{
		
		private static List<char> CHARACTERS = new List<char>("abcdefghijklmnopqrstuvwxyz");
		private static List<char> KEY = new List<char>();
	
		public SimpleSubstitution()
		{
			
		}

        private static bool Cipher(string input,string fileName, out string output, bool EnChipher)
		{
			output = "";
			List<char> simbols = new List<char>();
			List<char> chiphKey = new List<char>();
			if (EnChipher)
            {
				KEY = GenerateKey();
				FileSystemAPI.SaveKey(KEY, fileName,"K");
				simbols = CHARACTERS;
				chiphKey = KEY;
			}
			else
            {
				if (!FileSystemAPI.GetKey(out KEY, fileName,"K")) return false;
				simbols = KEY;
				chiphKey = CHARACTERS;
			}
           
			if (simbols.Count != chiphKey.Count)
            {
				return false;
            }
			for (int i = 0; i < input.Length; ++i)
			{
				int oldCharIndex = simbols.IndexOf(char.ToLower(input[i]));

				if (oldCharIndex >= 0)
                {
					output += char.IsUpper(input[i]) ? char.ToUpper(chiphKey[oldCharIndex]) : chiphKey[oldCharIndex];
                }
				else
				{
					output += input[i];
				}
			}

			return true;
		}
		public static bool Encipher(string input, string fileName, out string output)
		{
			return Cipher(input, fileName, out output, true);
		}

		public static bool Decipher(string input, string fileName, out string output)
		{
			return Cipher(input, fileName, out output, false);
		}

		private static List<char> GenerateKey()
		{
			List<char> keyList = new List<char>(CHARACTERS);
			return Shuffle(keyList);
		}

		public static List<char> Shuffle(List<char> list)
		{
			RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
			int n = list.Count;
			while (n > 1)
			{
				byte[] box = new byte[1];
				do provider.GetBytes(box);
				while (!(box[0] < n * (Byte.MaxValue / n)));
				int k = (box[0] % n);
				n--;
				char value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
			return list;
		}

	}
}
