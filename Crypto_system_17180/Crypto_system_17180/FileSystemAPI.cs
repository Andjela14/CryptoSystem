using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_system_17180
{
	class FileSystemAPI
	{
		private static string keyPath = @"./keys.txt";
		public static string KeyPath{
            get { return keyPath; }
			set { keyPath = value; }
		}
		public static void SaveKey(in List<char> key, string fileName,  string chiferShortcut ) // K for Simple Substitution and S for Solitaire
        {
			FileStream fileSystem = null;
			try
			{
				fileSystem = new FileStream(KeyPath, FileMode.Append);
				using (StreamWriter sw = new StreamWriter(fileSystem))
				{
					sw.WriteLine(fileName + chiferShortcut + new string(key.ToArray()));
				}

			}
			finally
			{
				if (fileSystem != null)
				{
					fileSystem.Dispose();
				}

			}
		}
		public static bool GetKey(out List<char> key, string fileName, string chiferShortcut)
		{
            key = new List<char>();
			using (StreamReader sr = File.OpenText(KeyPath))
			{
				string s = "";
				while ((s = sr.ReadLine()) != null)
				{
					if (s.StartsWith(fileName + chiferShortcut))
					{
						int index = fileName.Length+chiferShortcut.Length;
						string skey = s.Substring(index);
						key = new List<char>(skey);

					}
				}
			}
			return !(key.Count == 0);
			
		}
	
	}
}
