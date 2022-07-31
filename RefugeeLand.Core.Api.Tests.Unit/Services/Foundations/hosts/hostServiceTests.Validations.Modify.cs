using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using RefugeeLand.Core.Api.Models.hosts;
using RefugeeLand.Core.Api.Models.hosts.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.hosts
{
    public partial class hostServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfhostIsNullAndLogItAsync()
        {
            // given
            host nullhost = null;
            var nullhostException = new NullhostException();

            var expectedhostValidationException =
                new hostValidationException(nullhostException);

            // when
            ValueTask<host> modifyhostTask =
                this.hostService.ModifyhostAsync(nullhost);

            hostValidationException actualhostValidationException =
                await Assert.ThrowsAsync<hostValidationException>(
                    modifyhostTask.AsTask);

            // then
            actualhostValidationException.Should()
                .BeEquivalentTo(expectedhostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedhostValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatehostAsync(It.IsAny<host>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}