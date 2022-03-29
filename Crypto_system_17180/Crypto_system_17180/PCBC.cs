using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_system_17180
{
    class PCBC
    {
        private static string VInit = String.Empty; 
        public static string BlockPropagation(string prevBlock, string nextBlock, string ciphBlock, bool EnCipher, ref string VI)
        {
           
            string output = "";
            if (prevBlock == "")
            {
                if (EnCipher)
                {
                    VInit = RandomString(nextBlock.Length);
                    VI = VInit;
                }
                else
                {
                    VInit = VI;
                }
                for (int i = 0; i < nextBlock.Length; i++)
                {
                    output += (char)( (byte) VInit[i] ^ (byte) nextBlock[i] );
                }
            }
            else
            {
                for (int i = 0; i < nextBlock.Length; i++)
                {
                    output += (char)((byte) prevBlock[i] ^ (byte) nextBlock[i] ^ (byte) ciphBlock[i] );
                }
            }

            return output;
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnoprstuvwxyz";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}


