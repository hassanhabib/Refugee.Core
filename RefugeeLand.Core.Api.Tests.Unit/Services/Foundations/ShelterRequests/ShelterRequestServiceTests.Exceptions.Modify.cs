using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            ShelterRequest randomShelterRequest = CreateRandomShelterRequest();
            SqlException sqlException = GetSqlException();

            var failedShelterRequestStorageException =
                new FailedShelterRequestStorageException(sqlException);

            var expectedShelterRequestDependencyException =
                new ShelterRequestDependencyException(failedShelterRequestStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<ShelterRequest> modifyShelterRequestTask =
                this.shelterRequestService.ModifyShelterRequestAsync(randomShelterRequest);

            ShelterRequestDependencyException actualShelterRequestDependencyException =
                await Assert.ThrowsAsync<ShelterRequestDependencyException>(
                    modifyShelterRequestTask.AsTask);

            // then
            actualShelterRequestDependencyException.Should()
                .BeEquivalentTo(expectedShelterRequestDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterRequestByIdAsync(randomShelterRequest.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedShelterRequestDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShelterRequestAsync(randomShelterRequest),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            ShelterRequest someShelterRequest = CreateRandomShelterRequest();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidShelterRequestReferenceException =
                new InvalidShelterRequestReferenceException(foreignKeyConstraintConflictException);

            ShelterRequestDependencyValidationException expectedShelterRequestDependencyValidationException =
                new ShelterRequestDependencyValidationException(invalidShelterRequestReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<ShelterRequest> modifyShelterRequestTask =
                this.shelterRequestService.ModifyShelterRequestAsync(someShelterRequest);

            ShelterRequestDependencyValidationException actualShelterRequestDependencyValidationException =
                await Assert.ThrowsAsync<ShelterRequestDependencyValidationException>(
                    modifyShelterRequestTask.AsTask);

            // then
            actualShelterRequestDependencyValidationException.Should()
                .BeEquivalentTo(expectedShelterRequestDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterRequestByIdAsync(someShelterRequest.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedShelterRequestDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShelterRequestAsync(someShelterRequest),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            ShelterRequest randomShelterRequest = CreateRandomShelterRequest();
            var databaseUpdateException = new DbUpdateException();

            var failedShelterRequestStorageException =
                new FailedShelterRequestStorageException(databaseUpdateException);

            var expectedShelterRequestDependencyException =
                new ShelterRequestDependencyException(failedShelterRequestStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<ShelterRequest> modifyShelterRequestTask =
                this.shelterRequestService.ModifyShelterRequestAsync(randomShelterRequest);

            ShelterRequestDependencyException actualShelterRequestDependencyException =
                await Assert.ThrowsAsync<ShelterRequestDependencyException>(
                    modifyShelterRequestTask.AsTask);

            // then
            actualShelterRequestDependencyException.Should()
                .BeEquivalentTo(expectedShelterRequestDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterRequestByIdAsync(randomShelterRequest.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterRequestDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShelterRequestAsync(randomShelterRequest),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            ShelterRequest randomShelterRequest = CreateRandomShelterRequest();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedShelterRequestException =
                new LockedShelterRequestException(databaseUpdateConcurrencyException);

            var expectedShelterRequestDependencyValidationException =
                new ShelterRequestDependencyValidationException(lockedShelterRequestException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<ShelterRequest> modifyShelterRequestTask =
                this.shelterRequestService.ModifyShelterRequestAsync(randomShelterRequest);

            ShelterRequestDependencyValidationException actualShelterRequestDependencyValidationException =
                await Assert.ThrowsAsync<ShelterRequestDependencyValidationException>(
                    modifyShelterRequestTask.AsTask);

            // then
            actualShelterRequestDependencyValidationException.Should()
                .BeEquivalentTo(expectedShelterRequestDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterRequestByIdAsync(randomShelterRequest.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterRequestDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShelterRequestAsync(randomShelterRequest),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            ShelterRequest randomShelterRequest = CreateRandomShelterRequest();
            var serviceException = new Exception();

            var failedShelterRequestServiceException =
                new FailedShelterRequestServiceException(serviceException);

            var expectedShelterRequestServiceException =
                new ShelterRequestServiceException(failedShelterRequestServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<ShelterRequest> modifyShelterRequestTask =
                this.shelterRequestService.ModifyShelterRequestAsync(randomShelterRequest);

            ShelterRequestServiceException actualShelterRequestServiceException =
                await Assert.ThrowsAsync<ShelterRequestServiceException>(
                    modifyShelterRequestTask.AsTask);

            // then
            actualShelterRequestServiceException.Should()
                .BeEquivalentTo(expectedShelterRequestServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterRequestByIdAsync(randomShelterRequest.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterRequestServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShelterRequestAsync(randomShelterRequest),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}