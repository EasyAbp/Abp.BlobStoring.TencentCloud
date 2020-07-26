using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.Abp.BlobStoring.TencentCloud
{
    public class TencentCloudBlobNamingNormalizer : IBlobNamingNormalizer, ITransientDependency
    {
        public string NormalizeContainerName(string containerName)
        {
            throw new System.NotImplementedException();
        }

        public string NormalizeBlobName(string blobName)
        {
            throw new System.NotImplementedException();
        }
    }
}