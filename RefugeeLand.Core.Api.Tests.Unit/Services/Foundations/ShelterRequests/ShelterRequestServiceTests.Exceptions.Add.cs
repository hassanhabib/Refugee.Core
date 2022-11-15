using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using RefugeeLand.Core.Api.Models.ShelterRequests;
using RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.ShelterRequests
{
    public partial class ShelterRequestServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            ShelterRequest someShelterRequest = CreateRandomShelterRequest();
            SqlException sqlException = GetSqlException();

            var failedShelterRequestStorageException =
                new FailedShelterRequestStorageException(sqlException);

            var expectedShelterRequestDependencyException =
                new ShelterRequestDependencyException(failedShelterRequestStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<ShelterRequest> addShelterRequestTask =
                this.shelterRequestService.AddShelterRequestAsync(someShelterRequest);

            ShelterRequestDependencyException actualShelterRequestDependencyException =
                await Assert.ThrowsAsync<ShelterRequestDependencyException>(
                    addShelterRequestTask.AsTask);

            // then
            actualShelterRequestDependencyException.Should()
                .BeEquivalentTo(expectedShelterRequestDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShelterRequestAsync(It.IsAny<ShelterRequest>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedShelterRequestDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfShelterRequestAlreadyExsitsAndLogItAsync()
        {
            // given
            ShelterRequest randomShelterRequest = CreateRandomShelterRequest();
            ShelterRequest alreadyExistsShelterRequest = randomShelterRequest;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsShelterRequestException =
                new AlreadyExistsShelterRequestException(duplicateKeyException);

            var expectedShelterRequestDependencyValidationException =
                new ShelterRequestDependencyValidationException(alreadyExistsShelterRequestException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<ShelterRequest> addShelterRequestTask =
                this.shelterRequestService.AddShelterRequestAsync(alreadyExistsShelterRequest);

            // then
            ShelterRequestDependencyValidationException actualShelterRequestDependencyValidationException =
                await Assert.ThrowsAsync<ShelterRequestDependencyValidationException>(
                    addShelterRequestTask.AsTask);

            actualShelterRequestDependencyValidationException.Should()
                .BeEquivalentTo(expectedShelterRequestDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShelterRequestAsync(It.IsAny<ShelterRequest>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterRequestDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}