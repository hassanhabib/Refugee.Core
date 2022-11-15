using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedShelterRequestStorageException =
                new FailedShelterRequestStorageException(sqlException);

            var expectedShelterRequestDependencyException =
                new ShelterRequestDependencyException(failedShelterRequestStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShelterRequestByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ShelterRequest> retrieveShelterRequestByIdTask =
                this.shelterRequestService.RetrieveShelterRequestByIdAsync(someId);

            ShelterRequestDependencyException actualShelterRequestDependencyException =
                await Assert.ThrowsAsync<ShelterRequestDependencyException>(
                    retrieveShelterRequestByIdTask.AsTask);

            // then
            actualShelterRequestDependencyException.Should()
                .BeEquivalentTo(expectedShelterRequestDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterRequestByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedShelterRequestDependencyException))),
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

            var failedShelterRequestServiceException =
                new FailedShelterRequestServiceException(serviceException);

            var expectedShelterRequestServiceException =
                new ShelterRequestServiceException(failedShelterRequestServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShelterRequestByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ShelterRequest> retrieveShelterRequestByIdTask =
                this.shelterRequestService.RetrieveShelterRequestByIdAsync(someId);

            ShelterRequestServiceException actualShelterRequestServiceException =
                await Assert.ThrowsAsync<ShelterRequestServiceException>(
                    retrieveShelterRequestByIdTask.AsTask);

            // then
            actualShelterRequestServiceException.Should()
                .BeEquivalentTo(expectedShelterRequestServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterRequestByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedShelterRequestServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}