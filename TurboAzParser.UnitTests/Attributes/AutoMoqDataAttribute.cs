using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace TurboAzParser.UnitTests.Attributes;

internal class AutoMoqDataAttribute() : AutoDataAttribute(() =>
    new Fixture().Customize(new AutoMoqCustomization()));