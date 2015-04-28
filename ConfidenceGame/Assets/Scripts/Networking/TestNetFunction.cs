using UnityEngine;
using System.Collections;
using System.IO;

[System.Serializable]
public class TestNetFunction{

	public int intOne;
	public string message;
	public Vector3 vector;

	public byte[] Serialize(){

		using (MemoryStream m = new MemoryStream()) {
			using (BinaryWriter writer = new BinaryWriter(m)) {

				writer.Write(intOne);
				writer.Write(message);
				writer.Write(vector.x);
				writer.Write(vector.y);
				writer.Write(vector.z);

			}

			return m.ToArray();
		}
	}

	public static TestNetFunction Deserialize(byte[] data){

		TestNetFunction result = new TestNetFunction();

		using (MemoryStream m = new MemoryStream(data)) {
			using (BinaryReader reader = new BinaryReader(m)) {

				result.intOne = reader.ReadInt32();
				result.message = reader.ReadString();
				result.vector.x = reader.ReadSingle();
				result.vector.y = reader.ReadSingle();
				result.vector.z = reader.ReadSingle();

			}
		}

		return result;
	}
}
