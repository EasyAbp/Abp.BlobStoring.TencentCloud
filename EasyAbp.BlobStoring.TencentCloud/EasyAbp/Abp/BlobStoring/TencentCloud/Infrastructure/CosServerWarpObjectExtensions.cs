using System.IO;
using System.Threading.Tasks;
using COSXML.CosException;
using COSXML.Model.Bucket;
using COSXML.Model.Object;

namespace EasyAbp.Abp.BlobStoring.TencentCloud.Infrastructure
{
    public static class CosServerWarpObjectExtensions
    {
        public static bool CheckBucketIsExist(this CosServerWrapObject client, string bucketName)
        {
            try
            {
                client.CosXmlServer.HeadBucket(new HeadBucketRequest(bucketName));
                return true;
            }
            catch (CosServerException exception)
            {
                if (exception.statusCode == 404)
                {
                    return false;
                }

                throw;
            }
        }

        public static bool CheckObjectIsExist(this CosServerWrapObject client, string bucketName, string objectKey)
        {
            try
            {
                client.CosXmlServer.HeadObject(new HeadObjectRequest(bucketName, objectKey));
                return true;
            }
            catch (CosServerException exception)
            {
                if (exception.statusCode == 404)
                {
                    return false;
                }

                throw;
            }
        }

        public static void CreateBucket(this CosServerWrapObject client, string bucketName)
            => client.CosXmlServer.PutBucket(new PutBucketRequest(bucketName));

        public static async Task UploadObjectAsync(this CosServerWrapObject client,
            string bucketName,
            string objectKey,
            Stream data)
        {
            client.CosXmlServer.PutObject(new PutObjectRequest(bucketName, objectKey, await data.ToBytesAsync()));
        }

        public static async Task<Stream> DownloadObjectAsync(this CosServerWrapObject client,
            string buckName,
            string objectKey)
        {
            var result = client.CosXmlServer.GetObject(new GetObjectBytesRequest(buckName, objectKey));
            await Task.CompletedTask;

            return new MemoryStream(result.content);
        }
    }
}