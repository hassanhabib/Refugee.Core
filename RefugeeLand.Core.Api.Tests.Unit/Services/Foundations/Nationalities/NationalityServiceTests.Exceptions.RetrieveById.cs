using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using RefugeeLand.Core.Api.Models.Nationalities;
using RefugeeLand.Core.Api.Models.Nationalities.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.Nationalities
{
    public partial class NationalityServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedNationalityStorageException =
                new FailedNationalityStorageException(sqlException);

            var expectedNationalityDependencyException =
                new NationalityDependencyException(failedNationalityStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectNationalityByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Nationality> retrieveNationalityByIdTask =
                this.nationalityService.RetrieveNationalityByIdAsync(someId);

            NationalityDependencyException actualNationalityDependencyException =
                await Assert.ThrowsAsync<NationalityDependencyException>(
                    retrieveNationalityByIdTask.AsTask);

            // then
            actualNationalityDependencyException.Should()
                .BeEquivalentTo(expectedNationalityDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectNationalityByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedNationalityDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedNationalityServiceException =
                new FailedNationalityServiceException(serviceException);

            var expectedNationalityServiceException =
                new NationalityServiceException(failedNationalityServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectNationalityByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Nationality> retrieveNationalityByIdTask =
                this.nationalityService.RetrieveNationalityByIdAsync(someId);

            NationalityServiceException actualNationalityServiceException =
                await Assert.ThrowsAsync<NationalityServiceException>(
                    retrieveNationalityByIdTask.AsTask);

            // then
            actualNationalityServiceException.Should()
                .BeEquivalentTo(expectedNationalityServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectNationalityByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedNationalityServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}