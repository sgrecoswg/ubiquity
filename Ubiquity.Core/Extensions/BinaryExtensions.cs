using System.IO;

namespace Ubiquity.Core.Extensions
{
    public static class BinaryExtensions
    {
        public static void Save(this byte[] b, string path)
        {
            if (Directory.Exists(path))
            {
                File.WriteAllBytes(path, b);
            }
            else
            {
                throw new DirectoryNotFoundException();
            } 
        }
    }
}
