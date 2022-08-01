using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using RefugeeLand.Core.Api.Models.Hosts.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.Hosts
{
    public partial class HostServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedHostStorageException(sqlException);

            var expectedHostDependencyException =
                new HostDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllHosts())
                    .Throws(sqlException);

            // when
            Action retrieveAllHostsAction = () =>
                this.hostService.RetrieveAllHosts();

            HostDependencyException actualHostDependencyException =
                Assert.Throws<HostDependencyException>(retrieveAllHostsAction);

            // then
            actualHostDependencyException.Should()
                .BeEquivalentTo(expectedHostDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllHosts(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedHostDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}