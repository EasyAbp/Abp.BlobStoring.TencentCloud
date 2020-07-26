using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.Abp.BlobStoring.TencentCloud
{
    public class TencentCloudBlobProvider : BlobProviderBase, ITransientDependency
    {
        protected ITencentCloudBlobNameCalculator TencentCloudBlobNameCalculator { get; }
        
        public TencentCloudBlobProvider(ITencentCloudBlobNameCalculator tencentCloudBlobNameCalculator)
        {
            TencentCloudBlobNameCalculator = tencentCloudBlobNameCalculator;
        }

        public override Task SaveAsync(BlobProviderSaveArgs args)
        {
            var blobName = TencentCloudBlobNameCalculator.Calculate(args);
            
            throw new NotImplementedException();
        }

        public override Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            throw new System.NotImplementedException();
        }

        public override Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            throw new System.NotImplementedException();
        }

        public override Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            throw new System.NotImplementedException();
        }
    }
}