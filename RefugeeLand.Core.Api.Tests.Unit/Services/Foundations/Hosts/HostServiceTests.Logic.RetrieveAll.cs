using System.Linq;
using FluentAssertions;
using Moq;
using RefugeeLand.Core.Api.Models.Hosts;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.Hosts
{
    public partial class HostServiceTests
    {
        [Fact]
        public void ShouldReturnHosts()
        {
            // given
            IQueryable<Host> randomHosts = CreateRandomHosts();
            IQueryable<Host> storageHosts = randomHosts;
            IQueryable<Host> expectedHosts = storageHosts;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllHosts())
                    .Returns(storageHosts);

            // when
            IQueryable<Host> actualHosts =
                this.hostService.RetrieveAllHosts();

            // then
            actualHosts.Should().BeEquivalentTo(expectedHosts);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllHosts(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}