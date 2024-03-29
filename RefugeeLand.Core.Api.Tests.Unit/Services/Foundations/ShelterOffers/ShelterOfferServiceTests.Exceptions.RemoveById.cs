// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using RefugeeLand.Core.Api.Models.ShelterOffers;
using RefugeeLand.Core.Api.Models.ShelterOffers.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.ShelterOffers
{
    public partial class ShelterOfferServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            ShelterOffer randomShelterOffer = CreateRandomShelterOffer();
            SqlException sqlException = GetSqlException();

            var failedShelterOfferStorageException =
                new FailedShelterOfferStorageException(sqlException);

            var expectedShelterOfferDependencyException =
                new ShelterOfferDependencyException(failedShelterOfferStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShelterOfferByIdAsync(randomShelterOffer.Id))
                    .Throws(sqlException);

            // when
            ValueTask<ShelterOffer> addShelterOfferTask =
                this.shelterOfferService.RemoveShelterOfferByIdAsync(randomShelterOffer.Id);

            ShelterOfferDependencyException actualShelterOfferDependencyException =
                await Assert.ThrowsAsync<ShelterOfferDependencyException>(
                    addShelterOfferTask.AsTask);

            // then
            actualShelterOfferDependencyException.Should()
                .BeEquivalentTo(expectedShelterOfferDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterOfferByIdAsync(randomShelterOffer.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedShelterOfferDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteShelterOfferAsync(It.IsAny<ShelterOffer>()),
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
            Guid someShelterOfferId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedShelterOfferException =
                new LockedShelterOfferException(databaseUpdateConcurrencyException);

            var expectedShelterOfferDependencyValidationException =
                new ShelterOfferDependencyValidationException(lockedShelterOfferException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShelterOfferByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ShelterOffer> removeShelterOfferByIdTask =
                this.shelterOfferService.RemoveShelterOfferByIdAsync(someShelterOfferId);

            ShelterOfferDependencyValidationException actualShelterOfferDependencyValidationException =
                await Assert.ThrowsAsync<ShelterOfferDependencyValidationException>(
                    removeShelterOfferByIdTask.AsTask);

            // then
            actualShelterOfferDependencyValidationException.Should()
                .BeEquivalentTo(expectedShelterOfferDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterOfferByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteShelterOfferAsync(It.IsAny<ShelterOffer>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someShelterOfferId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedShelterOfferStorageException =
                new FailedShelterOfferStorageException(sqlException);

            var expectedShelterOfferDependencyException =
                new ShelterOfferDependencyException(failedShelterOfferStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShelterOfferByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ShelterOffer> deleteShelterOfferTask =
                this.shelterOfferService.RemoveShelterOfferByIdAsync(someShelterOfferId);

            ShelterOfferDependencyException actualShelterOfferDependencyException =
                await Assert.ThrowsAsync<ShelterOfferDependencyException>(
                    deleteShelterOfferTask.AsTask);

            // then
            actualShelterOfferDependencyException.Should()
                .BeEquivalentTo(expectedShelterOfferDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterOfferByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedShelterOfferDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someShelterOfferId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedShelterOfferServiceException =
                new FailedShelterOfferServiceException(serviceException);

            var expectedShelterOfferServiceException =
                new ShelterOfferServiceException(failedShelterOfferServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShelterOfferByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ShelterOffer> removeShelterOfferByIdTask =
                this.shelterOfferService.RemoveShelterOfferByIdAsync(someShelterOfferId);

            ShelterOfferServiceException actualShelterOfferServiceException =
                await Assert.ThrowsAsync<ShelterOfferServiceException>(
                    removeShelterOfferByIdTask.AsTask);

            // then
            actualShelterOfferServiceException.Should()
                .BeEquivalentTo(expectedShelterOfferServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterOfferByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}