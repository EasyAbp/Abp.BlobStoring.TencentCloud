using System.IO;
using System.Threading.Tasks;

namespace EasyAbp.BlobStoring.TencentCloud.Infrastructure
{
    public static class BinaryExtensions
    {
        public static async Task<byte[]> ToBytesAsync(this Stream stream)
        {
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}