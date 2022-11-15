using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using RefugeeLand.Core.Api.Models.ShelterRequests;
using RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.ShelterRequests
{
    public partial class ShelterRequestServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            ShelterRequest randomShelterRequest = CreateRandomShelterRequest();
            SqlException sqlException = GetSqlException();

            var failedShelterRequestStorageException =
                new FailedShelterRequestStorageException(sqlException);

            var expectedShelterRequestDependencyException =
                new ShelterRequestDependencyException(failedShelterRequestStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShelterRequestByIdAsync(randomShelterRequest.Id))
                    .Throws(sqlException);

            // when
            ValueTask<ShelterRequest> addShelterRequestTask =
                this.shelterRequestService.RemoveShelterRequestByIdAsync(randomShelterRequest.Id);

            ShelterRequestDependencyException actualShelterRequestDependencyException =
                await Assert.ThrowsAsync<ShelterRequestDependencyException>(
                    addShelterRequestTask.AsTask);

            // then
            actualShelterRequestDependencyException.Should()
                .BeEquivalentTo(expectedShelterRequestDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterRequestByIdAsync(randomShelterRequest.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedShelterRequestDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteShelterRequestAsync(It.IsAny<ShelterRequest>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someShelterRequestId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedShelterRequestException =
                new LockedShelterRequestException(databaseUpdateConcurrencyException);

            var expectedShelterRequestDependencyValidationException =
                new ShelterRequestDependencyValidationException(lockedShelterRequestException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShelterRequestByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ShelterRequest> removeShelterRequestByIdTask =
                this.shelterRequestService.RemoveShelterRequestByIdAsync(someShelterRequestId);

            ShelterRequestDependencyValidationException actualShelterRequestDependencyValidationException =
                await Assert.ThrowsAsync<ShelterRequestDependencyValidationException>(
                    removeShelterRequestByIdTask.AsTask);

            // then
            actualShelterRequestDependencyValidationException.Should()
                .BeEquivalentTo(expectedShelterRequestDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterRequestByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterRequestDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteShelterRequestAsync(It.IsAny<ShelterRequest>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}