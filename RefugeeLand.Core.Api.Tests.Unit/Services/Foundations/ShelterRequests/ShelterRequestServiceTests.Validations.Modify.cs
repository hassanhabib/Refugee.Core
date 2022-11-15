using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using RefugeeLand.Core.Api.Models.ShelterRequests;
using RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.ShelterRequests
{
    public partial class ShelterRequestServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfShelterRequestIsNullAndLogItAsync()
        {
            // given
            ShelterRequest nullShelterRequest = null;
            var nullShelterRequestException = new NullShelterRequestException();

            var expectedShelterRequestValidationException =
                new ShelterRequestValidationException(nullShelterRequestException);

            // when
            ValueTask<ShelterRequest> modifyShelterRequestTask =
                this.shelterRequestService.ModifyShelterRequestAsync(nullShelterRequest);

            ShelterRequestValidationException actualShelterRequestValidationException =
                await Assert.ThrowsAsync<ShelterRequestValidationException>(
                    modifyShelterRequestTask.AsTask);

            // then
            actualShelterRequestValidationException.Should()
                .BeEquivalentTo(expectedShelterRequestValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterRequestValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShelterRequestAsync(It.IsAny<ShelterRequest>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}