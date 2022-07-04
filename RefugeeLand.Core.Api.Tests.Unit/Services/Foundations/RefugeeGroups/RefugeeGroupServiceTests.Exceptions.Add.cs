// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using RefugeeLand.Core.Api.Models.RefugeeGroups;
using RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.RefugeeGroups
{
    public partial class RefugeeGroupServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            //given
            RefugeeGroup randomRefugeeGroup = CreateRandomRefugeeGroup();
            SqlException sqlException = GetSqlException();

            var failedRefugeeGroupStorageException = 
                new FailedRefugeeGroupStorageException(sqlException);

            var expectedRefugeeGroupDependencyException = 
                new RefugeeGroupDependencyException(failedRefugeeGroupStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertRefugeeGroupAsync(randomRefugeeGroup))
                    .ThrowsAsync(sqlException);
            
            //when
            ValueTask<RefugeeGroup> addRefugeeGroupTask =
                this.refugeeGroupService.AddRefugeeGroupAsync(randomRefugeeGroup);

            RefugeeGroupDependencyException actualRefugeeGroupDependencyException =
                await Assert.ThrowsAsync<RefugeeGroupDependencyException>(addRefugeeGroupTask.AsTask);

            //then
            actualRefugeeGroupDependencyException.Should().BeEquivalentTo(
                expectedRefugeeGroupDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedRefugeeGroupDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker => 
                broker.InsertRefugeeGroupAsync(randomRefugeeGroup),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}