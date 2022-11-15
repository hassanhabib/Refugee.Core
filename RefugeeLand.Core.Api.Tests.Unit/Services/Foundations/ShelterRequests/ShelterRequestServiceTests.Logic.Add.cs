using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using RefugeeLand.Core.Api.Models.ShelterRequests;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.ShelterRequests
{
    public partial class ShelterRequestServiceTests
    {
        [Fact]
        public async Task ShouldAddShelterRequestAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            ShelterRequest randomShelterRequest = CreateRandomShelterRequest(randomDateTimeOffset);
            ShelterRequest inputShelterRequest = randomShelterRequest;
            ShelterRequest storageShelterRequest = inputShelterRequest;
            ShelterRequest expectedShelterRequest = storageShelterRequest.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertShelterRequestAsync(inputShelterRequest))
                    .ReturnsAsync(storageShelterRequest);

            // when
            ShelterRequest actualShelterRequest = await this.shelterRequestService
                .AddShelterRequestAsync(inputShelterRequest);

            // then
            actualShelterRequest.Should().BeEquivalentTo(expectedShelterRequest);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShelterRequestAsync(inputShelterRequest),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}