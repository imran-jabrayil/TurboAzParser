using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Logging;
using Moq;
using TurboAzParser.Client.Abstractions;
using TurboAzParser.Services;

namespace TurboAzParser.UnitTests.Services;

public class TurboAzServiceTests
{
    [Theory, AutoMoqData]
    public async Task GetPageLinksAsync_ClientMethodGetPageCarUrlsAsync_CalledOnce(
        Mock<ITurboAzClient> client,
        ILogger<TurboAzService> logger,
        uint pageId,
        uint carId)
    {
        // Arrange
        var sut = new TurboAzService(client.Object, logger);
        
        // Act
        await sut.GetPageLinksAsync(pageId, carId);
        
        // Assert
        client.Verify(c => c.GetPageCarUrlsAsync(pageId, carId), Times.Once);
    }
}

public class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute() : base(() =>
        new Fixture().Customize(new AutoMoqCustomization()))
    {
    }
}