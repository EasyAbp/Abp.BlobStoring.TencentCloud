using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.BlobStoring;
using Xunit;

namespace EasyAbp.BlobStoring.TencentCloud.Tests
{
    public class AbpBlobStoringTencentCloudTests : AbpBlobStoringTencentCloudTestsBase
    {
        private readonly IBlobContainer _blobContainer;

        public AbpBlobStoringTencentCloudTests()
        {
            _blobContainer = GetRequiredService<IBlobContainer>();
        }

        [Fact]
        public async Task Should_Upload_File_Success()
        {
            // Arrange
            var bytes = new byte[] {0x01, 0x02};

            // Act
            await _blobContainer.SaveAsync("TestFile1", bytes);

            // Assert
            var fileStream = await _blobContainer.GetAsync("TestFile1");
            fileStream.Length.ShouldBe(2);
        }
    }
}