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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertShelterRequestAsync(inputShelterRequest))
                    .ReturnsAsync(storageShelterRequest);

            // when
            ShelterRequest actualShelterRequest = await this.shelterRequestService
                .AddShelterRequestAsync(inputShelterRequest);

            // then
            actualShelterRequest.Should().BeEquivalentTo(expectedShelterRequest);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShelterRequestAsync(inputShelterRequest),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}