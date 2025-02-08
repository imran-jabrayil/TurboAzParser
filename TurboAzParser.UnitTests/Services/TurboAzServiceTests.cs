using Microsoft.Extensions.Logging;
using Moq;
using TurboAzParser.Clients.Abstractions;
using TurboAzParser.Services;
using TurboAzParser.UnitTests.Attributes;

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

