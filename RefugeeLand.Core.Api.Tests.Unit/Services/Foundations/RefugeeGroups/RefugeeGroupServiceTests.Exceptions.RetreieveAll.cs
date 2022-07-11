// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.RefugeeGroups
{
    public partial class RefugeeGroupServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAndLogItAsync()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedRefugeeGroupStorageException =
                new FailedRefugeeGroupStorageException(sqlException);

            var expectedRefugeeGroupDependencyException =
                new RefugeeGroupDependencyException(failedRefugeeGroupStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllRefugeeGroups())
                    .Throws(sqlException);

            // when
            Action retrieveAllRefugeeGroupsAction = () =>
                this.refugeeGroupService.RetrieveAllRefugeeGroups();
            
            RefugeeGroupDependencyException actualRefugeeGroupDependencyException =
                Assert.Throws<RefugeeGroupDependencyException>(
                    retrieveAllRefugeeGroupsAction);

            // then
            actualRefugeeGroupDependencyException.Should()
                .BeEquivalentTo(expectedRefugeeGroupDependencyException);
            
            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllRefugeeGroups(),
                    Times.Once);
            
            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedRefugeeGroupDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
        
        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var failedRefugeeGroupServiceException =
                new FailedRefugeeGroupServiceException(serviceException);

            var expectedRefugeeGroupServiceException =
                new RefugeeGroupServiceException(failedRefugeeGroupServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllRefugeeGroups())
                    .Throws(serviceException);
            
            // when
            Action retrieveAllRefugeeGroupsAction = () =>
                this.refugeeGroupService.RetrieveAllRefugeeGroups();
            
            RefugeeGroupServiceException actualRefugeeGroupServiceException =
                Assert.Throws<RefugeeGroupServiceException>(
                    retrieveAllRefugeeGroupsAction);

            // then
            actualRefugeeGroupServiceException.Should()
                .BeEquivalentTo(expectedRefugeeGroupServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllRefugeeGroups(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRefugeeGroupServiceException))),
                        Times.Once);
            
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}