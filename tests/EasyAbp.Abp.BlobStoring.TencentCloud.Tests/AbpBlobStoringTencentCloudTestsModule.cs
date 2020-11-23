using System;
using COSXML.Model.Bucket;
using COSXML.Model.Object;
using EasyAbp.Abp.BlobStoring.TencentCloud.Infrastructure;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;

namespace EasyAbp.Abp.BlobStoring.TencentCloud.Tests
{
    [DependsOn(typeof(AbpBlobStoringTencentCloudModule),
        typeof(AbpAutofacModule),
        typeof(AbpTestBaseModule))]
    public class AbpBlobStoringTencentCloudTestsModule : AbpModule
    {
        private TencentCloudBlobProviderConfiguration _configuration;
        private string _randomBucketName = Guid.NewGuid().ToString();

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureAll((containerName, containerConfiguration) =>
                {
                    containerConfiguration.UseTencentCloud(tencentCloud =>
                    {
                        tencentCloud.AppId = "YourAppId";
                        tencentCloud.SecretId = "YourSecretId";
                        tencentCloud.SecretKey = "YourSecretKey";
                        tencentCloud.Region = "YourRegion";

                        tencentCloud.KeyDurationSecond = 600;
                        tencentCloud.ReadWriteTimeout = 40;
                        tencentCloud.ConnectionTimeout = 60;

                        tencentCloud.ContainerName = _randomBucketName;
                        tencentCloud.CreateContainerIfNotExists = true;

                        _configuration = tencentCloud;
                    });
                });
            });
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            // Clean data.
            var client = new CosServerWrapObject(_configuration);
            var result = client.CosXmlServer.GetBucket(new GetBucketRequest(_configuration.ContainerName));
            foreach (var content in result.listBucket.contentsList)
            {
                client.CosXmlServer.DeleteObject(new DeleteObjectRequest(_configuration.ContainerName, content.key));
            }

            client.CosXmlServer.DeleteBucket(new DeleteBucketRequest(_configuration.ContainerName));
        }
    }
}