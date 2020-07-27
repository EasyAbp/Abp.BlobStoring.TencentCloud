using Volo.Abp;
using Volo.Abp.Testing;

namespace EasyAbp.BlobStoring.TencentCloud.Tests
{
    public class AbpBlobStoringTencentCloudTestsBase : AbpIntegratedTest<AbpBlobStoringTencentCloudTestsModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}