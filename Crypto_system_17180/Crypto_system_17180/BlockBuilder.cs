using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_system_17180
{
    class BlockBuilder
    {
        public static List<string> StringToList(string dataString, int blockLength)
        {
            List<string> blocks = new List<string>();
            int remined = dataString.Length;
            for (int i = 0; i < dataString.Length; i += blockLength)
            {
                string dataBlock;
                if (remined < blockLength)
                {
                    dataBlock = dataString.Substring(i, remined);
                }
                else
                {
                    dataBlock = dataString.Substring(i, blockLength);
                }
                remined -= blockLength;
                blocks.Add(dataBlock);
            }
            return blocks;
        }

    }
}
