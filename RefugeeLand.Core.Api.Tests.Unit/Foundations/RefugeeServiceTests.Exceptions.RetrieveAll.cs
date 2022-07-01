// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using RefugeeLand.Core.Api.Models.Refugees.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Foundations
{
    public partial class RefugeeServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedRefugeeStorageException =
                new FailedRefugeeStorageException(sqlException);

            var expectedRefugeeDependencyException =
                new RefugeeDependencyException(failedRefugeeStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllRefugees())
                    .Throws(sqlException);

            // when
            Action retrieveAllRefugeeAction = () =>
                this.refugeeService.RetrieveAllRefugees();
            
            RefugeeDependencyException actualRefugeeDependencyException =
                Assert.Throws<RefugeeDependencyException>(
                    retrieveAllRefugeeAction);

            // then
            actualRefugeeDependencyException.Should()
                .BeEquivalentTo(expectedRefugeeDependencyException);
            
            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllRefugees(),
                    Times.Once);
            
            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedRefugeeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var failedRefugeeServiceException =
                new FailedRefugeeServiceException(serviceException);

            var expectedRefugeeServiceException =
                new RefugeeServiceException(failedRefugeeServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllRefugees())
                    .Throws(serviceException);
            
            // when
            Action retrieveAllRefugeesAction = () =>
                this.refugeeService.RetrieveAllRefugees();
            
            RefugeeServiceException actualRefugeeServiceException =
                Assert.Throws<RefugeeServiceException>(
                    retrieveAllRefugeesAction);

            // then
            actualRefugeeServiceException.Should()
                .BeEquivalentTo(expectedRefugeeServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllRefugees(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRefugeeServiceException))),
                        Times.Once);
            
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
