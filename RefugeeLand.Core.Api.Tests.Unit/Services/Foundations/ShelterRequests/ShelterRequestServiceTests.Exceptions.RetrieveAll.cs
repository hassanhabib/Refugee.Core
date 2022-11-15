using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.ShelterRequests
{
    public partial class ShelterRequestServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedShelterRequestStorageException(sqlException);

            var expectedShelterRequestDependencyException =
                new ShelterRequestDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllShelterRequests())
                    .Throws(sqlException);

            // when
            Action retrieveAllShelterRequestsAction = () =>
                this.shelterRequestService.RetrieveAllShelterRequests();

            ShelterRequestDependencyException actualShelterRequestDependencyException =
                Assert.Throws<ShelterRequestDependencyException>(retrieveAllShelterRequestsAction);

            // then
            actualShelterRequestDependencyException.Should()
                .BeEquivalentTo(expectedShelterRequestDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllShelterRequests(),
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
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomMessage();
            var serviceException = new Exception(exceptionMessage);

            var failedShelterRequestServiceException =
                new FailedShelterRequestServiceException(serviceException);

            var expectedShelterRequestServiceException =
                new ShelterRequestServiceException(failedShelterRequestServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllShelterRequests())
                    .Throws(serviceException);

            // when
            Action retrieveAllShelterRequestsAction = () =>
                this.shelterRequestService.RetrieveAllShelterRequests();

            ShelterRequestServiceException actualShelterRequestServiceException =
                Assert.Throws<ShelterRequestServiceException>(retrieveAllShelterRequestsAction);

            // then
            actualShelterRequestServiceException.Should()
                .BeEquivalentTo(expectedShelterRequestServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllShelterRequests(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterRequestServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}