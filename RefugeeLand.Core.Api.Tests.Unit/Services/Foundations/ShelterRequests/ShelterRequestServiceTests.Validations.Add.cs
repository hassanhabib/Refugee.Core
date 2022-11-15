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
        public async Task ShouldThrowValidationExceptionOnAddIfShelterRequestIsNullAndLogItAsync()
        {
            // given
            ShelterRequest nullShelterRequest = null;

            var nullShelterRequestException =
                new NullShelterRequestException();

            var expectedShelterRequestValidationException =
                new ShelterRequestValidationException(nullShelterRequestException);

            // when
            ValueTask<ShelterRequest> addShelterRequestTask =
                this.shelterRequestService.AddShelterRequestAsync(nullShelterRequest);

            ShelterRequestValidationException actualShelterRequestValidationException =
                await Assert.ThrowsAsync<ShelterRequestValidationException>(
                    addShelterRequestTask.AsTask);

            // then
            actualShelterRequestValidationException.Should().BeEquivalentTo(expectedShelterRequestValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterRequestValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}