using Volo.Abp.BlobStoring;

namespace EasyAbp.Abp.BlobStoring.TencentCloud
{
    public interface ITencentCloudBlobNameCalculator
    {
        string Calculate(BlobProviderArgs args);
    }
}