using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Ajax.Utilities;
using System.Web;

namespace dnc_300_movie_finder_data.Helpers
{
    public class DataSerializer
    {
        public static void BinarySerialize(object data, string filePath)
        {
            FileStream fileStream = default(FileStream);
            BinaryFormatter bf = new BinaryFormatter();

            try
            {

                if (File.Exists(filePath)) File.Delete(filePath);
                fileStream = File.Create(filePath);
                bf.Serialize(fileStream, data);
            }
            finally
            {
                fileStream.Close();
            }
        }

        public static object BinaryDeserialize(string filePath)
        {
            object obj = null;
            FileStream fileStream = default(FileStream);
            BinaryFormatter bf = new BinaryFormatter();
            if (File.Exists(filePath))
            {
                try
                {

                    fileStream = File.OpenRead(filePath);
                    obj = bf.Deserialize(fileStream);
                }
                finally
                {
                    fileStream.Close();
                }
            }
            return obj;
        }
    }
}
