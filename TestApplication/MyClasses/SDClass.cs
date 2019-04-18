using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;
using TestApplication.Model;

namespace TestApplication.MyClasses
{
	public class SDClass<T> where T : class
	{
		private BinaryFormatter formatter = new BinaryFormatter();
		private XmlSerializer serializer = new XmlSerializer(typeof(T));
		private XmlSerializer serializerList = new XmlSerializer(typeof(List<T>));
		public void Serialize(T obj, string path)
		{
			using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.None))
			{
				formatter.Serialize(stream, obj);
			}
		}

		public void Serialize(List<T> obj, string path)
		{
			using (Stream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
			{
				formatter.Serialize(stream, obj);
			}
		}
		public List<T> DeSerialize(string path)
		{
			List<T> _obj = null;
			using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				_obj = (List<T>)formatter.Deserialize(stream);
			}
			return _obj;
		}

		public T DeSerialize(T obj, string path)
		{
			T _obj = null;
			if (obj == null)
				throw new NullReferenceException();
			using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				_obj = (T)formatter.Deserialize(stream);
			}
			return _obj;
		}

		public void SerializeXml(List<T> obj, string path)
		{
			using (Stream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
			{
				serializerList.Serialize(stream, obj);
			}
		}

		public List<T> DeserializeXml(string path)
		{
			List<T> _obj = null;		
			using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				_obj = (List<T>)serializerList.Deserialize(stream);
			}
			return _obj;
		}

	}
}
