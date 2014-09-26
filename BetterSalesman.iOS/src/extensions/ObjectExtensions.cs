using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace BetterSalesman.iOS
{
    public static class ObjectExtensions
    {
        // Deep clone
        public static T DeepClone<T>(this T a) where T : ISerializable
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T) formatter.Deserialize(stream);
            }
        }
    }
}

