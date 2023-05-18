// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using FluentAssertions;
using Moq;
using RefugeeLand.Core.Api.Models.Refugees;
using RefugeeLand.Core.Api.Models.Refugees.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.Refugees
{
    public partial class RefugeeServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidRefugeeId = Guid.Empty;

            var invalidRefugeeException =
                new InvalidRefugeeException();

            invalidRefugeeException.AddData(
                key: nameof(Refugee.Id),
                values: "Id is required");

            var expectedRefugeeValidationException =
                new RefugeeValidationException(invalidRefugeeException);
            
            // when
            ValueTask<Refugee> retrieveRefugeeByIdTask =
                this.refugeeService.RetrieveRefugeeByIdAsync(invalidRefugeeId);

            RefugeeValidationException actualRefugeeValidationException =
                await Assert.ThrowsAsync<RefugeeValidationException>(
                    retrieveRefugeeByIdTask.AsTask);

            // then
            actualRefugeeValidationException.Should()
                .BeEquivalentTo(expectedRefugeeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRefugeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRefugeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            
        }
    }
}