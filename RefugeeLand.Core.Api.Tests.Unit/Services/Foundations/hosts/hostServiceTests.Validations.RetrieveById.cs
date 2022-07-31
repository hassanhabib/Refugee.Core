using System;
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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidhostId = Guid.Empty;

            var invalidhostException =
                new InvalidhostException();

            invalidhostException.AddData(
                key: nameof(host.Id),
                values: "Id is required");

            var expectedhostValidationException =
                new hostValidationException(invalidhostException);

            // when
            ValueTask<host> retrievehostByIdTask =
                this.hostService.RetrievehostByIdAsync(invalidhostId);

            hostValidationException actualhostValidationException =
                await Assert.ThrowsAsync<hostValidationException>(
                    retrievehostByIdTask.AsTask);

            // then
            actualhostValidationException.Should()
                .BeEquivalentTo(expectedhostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedhostValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelecthostByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}