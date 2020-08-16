using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;

namespace EasyAbp.BlobStoring.TencentCloud
{
    [DependsOn(typeof(AbpBlobStoringModule))]
    public class AbpBlobStoringTencentCloudModule : AbpModule
    {
    }
}