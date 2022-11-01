// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            ShelterOffer someShelterOffer = CreateRandomShelterOffer();
            SqlException sqlException = GetSqlException();

            var failedShelterOfferStorageException =
                new FailedShelterOfferStorageException(sqlException);

            var expectedShelterOfferDependencyException =
                new ShelterOfferDependencyException(failedShelterOfferStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<ShelterOffer> addShelterOfferTask =
                this.shelterOfferService.AddShelterOfferAsync(someShelterOffer);

            ShelterOfferDependencyException actualShelterOfferDependencyException =
                await Assert.ThrowsAsync<ShelterOfferDependencyException>(
                    addShelterOfferTask.AsTask);

            // then
            actualShelterOfferDependencyException.Should()
                .BeEquivalentTo(expectedShelterOfferDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShelterOfferAsync(It.IsAny<ShelterOffer>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedShelterOfferDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfShelterOfferAlreadyExsitsAndLogItAsync()
        {
            // given
            ShelterOffer randomShelterOffer = CreateRandomShelterOffer();
            ShelterOffer alreadyExistsShelterOffer = randomShelterOffer;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsShelterOfferException =
                new AlreadyExistsShelterOfferException(duplicateKeyException);

            var expectedShelterOfferDependencyValidationException =
                new ShelterOfferDependencyValidationException(alreadyExistsShelterOfferException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<ShelterOffer> addShelterOfferTask =
                this.shelterOfferService.AddShelterOfferAsync(alreadyExistsShelterOffer);

            // then
            ShelterOfferDependencyValidationException actualShelterOfferDependencyValidationException =
                await Assert.ThrowsAsync<ShelterOfferDependencyValidationException>(
                    addShelterOfferTask.AsTask);

            actualShelterOfferDependencyValidationException.Should()
                .BeEquivalentTo(expectedShelterOfferDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShelterOfferAsync(It.IsAny<ShelterOffer>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            ShelterOffer someShelterOffer = CreateRandomShelterOffer();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidShelterOfferReferenceException =
                new InvalidShelterOfferReferenceException(foreignKeyConstraintConflictException);

            var expectedShelterOfferValidationException =
                new ShelterOfferDependencyValidationException(invalidShelterOfferReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<ShelterOffer> addShelterOfferTask =
                this.shelterOfferService.AddShelterOfferAsync(someShelterOffer);

            // then
            ShelterOfferDependencyValidationException actualShelterOfferDependencyValidationException =
                await Assert.ThrowsAsync<ShelterOfferDependencyValidationException>(
                    addShelterOfferTask.AsTask);

            actualShelterOfferDependencyValidationException.Should()
                .BeEquivalentTo(expectedShelterOfferValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShelterOfferAsync(someShelterOffer),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            ShelterOffer someShelterOffer = CreateRandomShelterOffer();

            var databaseUpdateException =
                new DbUpdateException();

            var failedShelterOfferStorageException =
                new FailedShelterOfferStorageException(databaseUpdateException);

            var expectedShelterOfferDependencyException =
                new ShelterOfferDependencyException(failedShelterOfferStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<ShelterOffer> addShelterOfferTask =
                this.shelterOfferService.AddShelterOfferAsync(someShelterOffer);

            ShelterOfferDependencyException actualShelterOfferDependencyException =
                await Assert.ThrowsAsync<ShelterOfferDependencyException>(
                    addShelterOfferTask.AsTask);

            // then
            actualShelterOfferDependencyException.Should()
                .BeEquivalentTo(expectedShelterOfferDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShelterOfferAsync(It.IsAny<ShelterOffer>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            ShelterOffer someShelterOffer = CreateRandomShelterOffer();
            var serviceException = new Exception();

            var failedShelterOfferServiceException =
                new FailedShelterOfferServiceException(serviceException);

            var expectedShelterOfferServiceException =
                new ShelterOfferServiceException(failedShelterOfferServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<ShelterOffer> addShelterOfferTask =
                this.shelterOfferService.AddShelterOfferAsync(someShelterOffer);

            ShelterOfferServiceException actualShelterOfferServiceException =
                await Assert.ThrowsAsync<ShelterOfferServiceException>(
                    addShelterOfferTask.AsTask);

            // then
            actualShelterOfferServiceException.Should()
                .BeEquivalentTo(expectedShelterOfferServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShelterOfferAsync(It.IsAny<ShelterOffer>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}