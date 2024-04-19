using System.IO;
using System.Threading.Tasks;

namespace MediatorEndpoint;
public static class IFileExtensions
{
    public static string ReadAsText(this IFile file)
    {
        using (var stream = new MemoryStream())
        {
            file.CopyTo(stream);
            stream.Seek(0, SeekOrigin.Begin);

            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
    }
    public static async Task<string> ReadAsTextAsync(this IFile file)
    {
        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);

            using (var reader = new StreamReader(stream))
                return await reader.ReadToEndAsync();
        }
    }
    public static byte[] ReadAsBytes(this IFile file)
    {
        using (var stream = new MemoryStream())
        {
            file.CopyTo(stream);
            return stream.ToArray();
        }
    }
    public static async Task<byte[]> ReadAsBytesAsync(this IFile file)
    {
        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            return stream.ToArray();
        }
    }
}