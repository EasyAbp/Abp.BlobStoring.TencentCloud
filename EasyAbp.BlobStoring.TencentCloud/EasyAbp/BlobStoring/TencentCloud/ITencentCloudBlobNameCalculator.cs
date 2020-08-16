using Volo.Abp.BlobStoring;

namespace EasyAbp.BlobStoring.TencentCloud
{
    public interface ITencentCloudBlobNameCalculator
    {
        string Calculate(BlobProviderArgs args);
    }
}