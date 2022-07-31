using System.Linq;
using FluentAssertions;
using Moq;
using RefugeeLand.Core.Api.Models.hosts;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.hosts
{
    public partial class hostServiceTests
    {
        [Fact]
        public void ShouldReturnhosts()
        {
            // given
            IQueryable<host> randomhosts = CreateRandomhosts();
            IQueryable<host> storagehosts = randomhosts;
            IQueryable<host> expectedhosts = storagehosts;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllhosts())
                    .Returns(storagehosts);

            // when
            IQueryable<host> actualhosts =
                this.hostService.RetrieveAllhosts();

            // then
            actualhosts.Should().BeEquivalentTo(expectedhosts);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllhosts(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}