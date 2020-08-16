using System;
using System.IO;
using System.Threading.Tasks;
using EasyAbp.BlobStoring.TencentCloud.Infrastructure;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.BlobStoring.TencentCloud
{
    public class TencentCloudBlobProvider : BlobProviderBase, ITransientDependency
    {
        protected ITencentCloudBlobNameCalculator TencentCloudBlobNameCalculator { get; }

        public TencentCloudBlobProvider(ITencentCloudBlobNameCalculator tencentCloudBlobNameCalculator)
        {
            TencentCloudBlobNameCalculator = tencentCloudBlobNameCalculator;
        }

        public override async Task SaveAsync(BlobProviderSaveArgs args)
        {
            var blobName = TencentCloudBlobNameCalculator.Calculate(args);
            var configuration = args.Configuration.GetTencentCloudConfiguration();
            var client = GetClient(args);

            if (!args.OverrideExisting && await BlobExistsAsync(args, blobName))
            {
                throw new BlobAlreadyExistsException(
                    $"Saving BLOB '{args.BlobName}' does already exists in the container '{GetContainerName(args)}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.");
            }

            if (configuration.CreateContainerIfNotExists)
            {
                await CreateContainerIfNotExistsAsync(args);
            }

            await client.UploadObjectAsync(GetContainerName(args), blobName, args.BlobStream);
        }

        public override async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            var blobName = TencentCloudBlobNameCalculator.Calculate(args);
            var containerName = GetContainerName(args);
            var client = GetClient(args);

            if (!await BlobExistsAsync(args, blobName))
            {
                return false;
            }

            await client.DeleteObjectAsync(containerName, blobName);

            return true;
        }

        public override Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            return BlobExistsAsync(args, TencentCloudBlobNameCalculator.Calculate(args));
        }

        public override async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            var blobName = TencentCloudBlobNameCalculator.Calculate(args);
            var containerName = GetContainerName(args);
            var client = GetClient(args);

            if (!client.CheckObjectIsExist(containerName, blobName))
            {
                return null;
            }

            return await client.DownloadObjectAsync(containerName, blobName);
        }

        protected virtual CosServerWrapObject GetClient(BlobProviderArgs args)
        {
            var configuration = args.Configuration.GetTencentCloudConfiguration();
            return new CosServerWrapObject(configuration);
        }

        protected virtual Task<bool> BlobExistsAsync(BlobProviderArgs args, string blobName)
        {
            var client = GetClient(args);
            var containerName = GetContainerName(args);

            return Task.FromResult(client.CheckBucketIsExist(containerName) &&
                                   client.CheckObjectIsExist(containerName, blobName));
        }

        protected virtual Task CreateContainerIfNotExistsAsync(BlobProviderArgs args)
        {
            var client = GetClient(args);
            var containerName = GetContainerName(args);

            if (!client.CheckBucketIsExist(containerName))
            {
                client.CreateBucket(containerName);
            }

            return Task.CompletedTask;
        }

        protected virtual string GetContainerName(BlobProviderArgs args)
        {
            var configuration = args.Configuration.GetTencentCloudConfiguration();
            return configuration.ContainerName.IsNullOrWhiteSpace()
                ? args.ContainerName
                : $"{configuration.ContainerName}-{configuration.AppId}";
        }
    }
}