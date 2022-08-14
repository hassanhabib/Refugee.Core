using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Nationality randomNationality = CreateRandomNationality();
            SqlException sqlException = GetSqlException();

            var failedNationalityStorageException =
                new FailedNationalityStorageException(sqlException);

            var expectedNationalityDependencyException =
                new NationalityDependencyException(failedNationalityStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectNationalityByIdAsync(randomNationality.Id))
                    .Throws(sqlException);

            // when
            ValueTask<Nationality> addNationalityTask =
                this.nationalityService.RemoveNationalityByIdAsync(randomNationality.Id);

            NationalityDependencyException actualNationalityDependencyException =
                await Assert.ThrowsAsync<NationalityDependencyException>(
                    addNationalityTask.AsTask);

            // then
            actualNationalityDependencyException.Should()
                .BeEquivalentTo(expectedNationalityDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectNationalityByIdAsync(randomNationality.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedNationalityDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteNationalityAsync(It.IsAny<Nationality>()),
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
            Guid someNationalityId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedNationalityException =
                new LockedNationalityException(databaseUpdateConcurrencyException);

            var expectedNationalityDependencyValidationException =
                new NationalityDependencyValidationException(lockedNationalityException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectNationalityByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Nationality> removeNationalityByIdTask =
                this.nationalityService.RemoveNationalityByIdAsync(someNationalityId);

            NationalityDependencyValidationException actualNationalityDependencyValidationException =
                await Assert.ThrowsAsync<NationalityDependencyValidationException>(
                    removeNationalityByIdTask.AsTask);

            // then
            actualNationalityDependencyValidationException.Should()
                .BeEquivalentTo(expectedNationalityDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectNationalityByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedNationalityDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteNationalityAsync(It.IsAny<Nationality>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}