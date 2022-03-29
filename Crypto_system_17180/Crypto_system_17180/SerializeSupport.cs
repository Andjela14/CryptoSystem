using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Crypto_system_17180
{
	class SerializeSupport
    {
		public SerializeSupport()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void Serialize(object obj, string fileName)
		{
			Type objType = obj.GetType();
			XmlSerializer sr = new XmlSerializer(objType);
			TextWriter wr = new StreamWriter(fileName);
			sr.Serialize(wr, obj);
			wr.Close();
		}

		public static object Deserialize(string fileName, Type t)
		{
			XmlSerializer sr = new XmlSerializer(t);
			FileStream fs = new FileStream(fileName, FileMode.Open);
			TextReader reader = new StreamReader(fs);
			return sr.Deserialize(reader);

		}

    }

}
