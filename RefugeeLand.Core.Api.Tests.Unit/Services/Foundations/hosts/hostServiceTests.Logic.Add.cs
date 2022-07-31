using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using RefugeeLand.Core.Api.Models.hosts;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.hosts
{
    public partial class hostServiceTests
    {
        [Fact]
        public async Task ShouldAddhostAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            host randomhost = CreateRandomhost(randomDateTimeOffset);
            host inputhost = randomhost;
            host storagehost = inputhost;
            host expectedhost = storagehost.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.InserthostAsync(inputhost))
                    .ReturnsAsync(storagehost);

            // when
            host actualhost = await this.hostService
                .AddhostAsync(inputhost);

            // then
            actualhost.Should().BeEquivalentTo(expectedhost);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InserthostAsync(inputhost),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}