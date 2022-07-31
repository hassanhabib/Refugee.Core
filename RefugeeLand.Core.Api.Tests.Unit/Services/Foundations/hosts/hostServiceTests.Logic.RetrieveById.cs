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
        public async Task ShouldRetrievehostByIdAsync()
        {
            // given
            host randomhost = CreateRandomhost();
            host inputhost = randomhost;
            host storagehost = randomhost;
            host expectedhost = storagehost.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelecthostByIdAsync(inputhost.Id))
                    .ReturnsAsync(storagehost);

            // when
            host actualhost =
                await this.hostService.RetrievehostByIdAsync(inputhost.Id);

            // then
            actualhost.Should().BeEquivalentTo(expectedhost);

            this.storageBrokerMock.Verify(broker =>
                broker.SelecthostByIdAsync(inputhost.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}