using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using RefugeeLand.Core.Api.Models.Nationalities;
using RefugeeLand.Core.Api.Models.Nationalities.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.Nationalities
{
    public partial class NationalityServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Nationality someNationality = CreateRandomNationality();
            SqlException sqlException = GetSqlException();

            var failedNationalityStorageException =
                new FailedNationalityStorageException(sqlException);

            var expectedNationalityDependencyException =
                new NationalityDependencyException(failedNationalityStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Nationality> addNationalityTask =
                this.nationalityService.AddNationalityAsync(someNationality);

            NationalityDependencyException actualNationalityDependencyException =
                await Assert.ThrowsAsync<NationalityDependencyException>(
                    addNationalityTask.AsTask);

            // then
            actualNationalityDependencyException.Should()
                .BeEquivalentTo(expectedNationalityDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertNationalityAsync(It.IsAny<Nationality>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedNationalityDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfNationalityAlreadyExsitsAndLogItAsync()
        {
            // given
            Nationality randomNationality = CreateRandomNationality();
            Nationality alreadyExistsNationality = randomNationality;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsNationalityException =
                new AlreadyExistsNationalityException(duplicateKeyException);

            var expectedNationalityDependencyValidationException =
                new NationalityDependencyValidationException(alreadyExistsNationalityException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<Nationality> addNationalityTask =
                this.nationalityService.AddNationalityAsync(alreadyExistsNationality);

            // then
            NationalityDependencyValidationException actualNationalityDependencyValidationException =
                await Assert.ThrowsAsync<NationalityDependencyValidationException>(
                    addNationalityTask.AsTask);

            actualNationalityDependencyValidationException.Should()
                .BeEquivalentTo(expectedNationalityDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertNationalityAsync(It.IsAny<Nationality>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedNationalityDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            Nationality someNationality = CreateRandomNationality();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidNationalityReferenceException =
                new InvalidNationalityReferenceException(foreignKeyConstraintConflictException);

            var expectedNationalityValidationException =
                new NationalityDependencyValidationException(invalidNationalityReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<Nationality> addNationalityTask =
                this.nationalityService.AddNationalityAsync(someNationality);

            // then
            NationalityDependencyValidationException actualNationalityDependencyValidationException =
                await Assert.ThrowsAsync<NationalityDependencyValidationException>(
                    addNationalityTask.AsTask);

            actualNationalityDependencyValidationException.Should()
                .BeEquivalentTo(expectedNationalityValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedNationalityValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertNationalityAsync(someNationality),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Nationality someNationality = CreateRandomNationality();

            var databaseUpdateException =
                new DbUpdateException();

            var failedNationalityStorageException =
                new FailedNationalityStorageException(databaseUpdateException);

            var expectedNationalityDependencyException =
                new NationalityDependencyException(failedNationalityStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Nationality> addNationalityTask =
                this.nationalityService.AddNationalityAsync(someNationality);

            NationalityDependencyException actualNationalityDependencyException =
                await Assert.ThrowsAsync<NationalityDependencyException>(
                    addNationalityTask.AsTask);

            // then
            actualNationalityDependencyException.Should()
                .BeEquivalentTo(expectedNationalityDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertNationalityAsync(It.IsAny<Nationality>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedNationalityDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}