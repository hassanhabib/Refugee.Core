// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Moq;
using RefugeeLand.Core.Api.Models.Refugees;
using RefugeeLand.Core.Api.Models.Refugees.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Foundations
{
    public partial class RefugeeServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfRefugeeIsNullAndLogItAsync()
        {
            // given
            Refugee nullRefugee = null;
            var nullRefugeeException = new NullRefugeeException();

            var expectedRefugeeValidationException =
                new RefugeeValidationException(nullRefugeeException);

            // when
            ValueTask<Refugee> addRefugeeTask =
                this.refugeeService.AddRefugeeAsync(nullRefugee);

            // then
            await Assert.ThrowsAsync<RefugeeValidationException>(() =>
                addRefugeeTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRefugeeAsync(nullRefugee),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRefugeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
