using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using RefugeeLand.Core.Api.Models.hosts.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.hosts
{
    public partial class hostServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedhostStorageException(sqlException);

            var expectedhostDependencyException =
                new hostDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllhosts())
                    .Throws(sqlException);

            // when
            Action retrieveAllhostsAction = () =>
                this.hostService.RetrieveAllhosts();

            hostDependencyException actualhostDependencyException =
                Assert.Throws<hostDependencyException>(retrieveAllhostsAction);

            // then
            actualhostDependencyException.Should()
                .BeEquivalentTo(expectedhostDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllhosts(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedhostDependencyException))),
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

            var failedhostServiceException =
                new FailedhostServiceException(serviceException);

            var expectedhostServiceException =
                new hostServiceException(failedhostServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllhosts())
                    .Throws(serviceException);

            // when
            Action retrieveAllhostsAction = () =>
                this.hostService.RetrieveAllhosts();

            hostServiceException actualhostServiceException =
                Assert.Throws<hostServiceException>(retrieveAllhostsAction);

            // then
            actualhostServiceException.Should()
                .BeEquivalentTo(expectedhostServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllhosts(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedhostServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}