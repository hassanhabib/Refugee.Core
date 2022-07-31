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
        public async Task ShouldRemovehostByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputhostId = randomId;
            host randomhost = CreateRandomhost();
            host storagehost = randomhost;
            host expectedInputhost = storagehost;
            host deletedhost = expectedInputhost;
            host expectedhost = deletedhost.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelecthostByIdAsync(inputhostId))
                    .ReturnsAsync(storagehost);

            this.storageBrokerMock.Setup(broker =>
                broker.DeletehostAsync(expectedInputhost))
                    .ReturnsAsync(deletedhost);

            // when
            host actualhost = await this.hostService
                .RemovehostByIdAsync(inputhostId);

            // then
            actualhost.Should().BeEquivalentTo(expectedhost);

            this.storageBrokerMock.Verify(broker =>
                broker.SelecthostByIdAsync(inputhostId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletehostAsync(expectedInputhost),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}