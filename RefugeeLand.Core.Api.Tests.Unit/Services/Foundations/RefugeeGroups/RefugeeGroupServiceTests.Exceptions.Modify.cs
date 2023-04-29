// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using RefugeeLand.Core.Api.Models.RefugeeGroups;
using RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.RefugeeGroups
{
    public partial class RefugeeGroupServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            RefugeeGroup randomRefugeeGroup = CreateRandomRefugeeGroup();
            SqlException sqlException = GetSqlException();
    
            var failedRefugeeGroupStorageException =
                new FailedRefugeeGroupStorageException(sqlException);
    
            var expectedRefugeeGroupDependencyException =
                new RefugeeGroupDependencyException(failedRefugeeGroupStorageException);
    
            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);
    
            // when
            ValueTask<RefugeeGroup> modifyRefugeeGroupTask =
                this.refugeeGroupService.ModifyRefugeeGroupAsync(randomRefugeeGroup);
    
            RefugeeGroupDependencyException actualRefugeeGroupDependencyException =
                await Assert.ThrowsAsync<RefugeeGroupDependencyException>(
                    modifyRefugeeGroupTask.AsTask);
    
            // then
            actualRefugeeGroupDependencyException.Should()
                .BeEquivalentTo(expectedRefugeeGroupDependencyException);
    
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);
    
            this.storageBrokerMock.Verify(broker =>
                broker.SelectRefugeeGroupByIdAsync(randomRefugeeGroup.Id),
                    Times.Never);
    
            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedRefugeeGroupDependencyException))),
                        Times.Once);
    
            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRefugeeGroupAsync(randomRefugeeGroup),
                    Times.Never);
    
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
        
        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            RefugeeGroup someRefugeeGroup = CreateRandomRefugeeGroup();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidRefugeeGroupReferenceException =
                new InvalidRefugeeGroupReferenceException(foreignKeyConstraintConflictException);

            RefugeeGroupDependencyValidationException expectedRefugeeGroupDependencyValidationException =
                new RefugeeGroupDependencyValidationException(invalidRefugeeGroupReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<RefugeeGroup> modifyRefugeeGroupTask =
                this.refugeeGroupService.ModifyRefugeeGroupAsync(someRefugeeGroup);

            RefugeeGroupDependencyValidationException actualRefugeeGroupDependencyValidationException =
                await Assert.ThrowsAsync<RefugeeGroupDependencyValidationException>(
                    modifyRefugeeGroupTask.AsTask);

            // then
            actualRefugeeGroupDependencyValidationException.Should()
                .BeEquivalentTo(expectedRefugeeGroupDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRefugeeGroupByIdAsync(someRefugeeGroup.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRefugeeGroupDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRefugeeGroupAsync(someRefugeeGroup),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
        
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            RefugeeGroup randomRefugeeGroup = CreateRandomRefugeeGroup();
            var databaseUpdateException = new DbUpdateException();

            var failedRefugeeGroupStorageException =
                new FailedRefugeeGroupStorageException(databaseUpdateException);

            var expectedRefugeeGroupDependencyException =
                new RefugeeGroupDependencyException(failedRefugeeGroupStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<RefugeeGroup> modifyRefugeeGroupTask =
                this.refugeeGroupService.ModifyRefugeeGroupAsync(randomRefugeeGroup);

            RefugeeGroupDependencyException actualRefugeeGroupDependencyException =
                await Assert.ThrowsAsync<RefugeeGroupDependencyException>(
                    modifyRefugeeGroupTask.AsTask);

            // then
            actualRefugeeGroupDependencyException.Should()
                .BeEquivalentTo(expectedRefugeeGroupDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRefugeeGroupByIdAsync(randomRefugeeGroup.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRefugeeGroupDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRefugeeGroupAsync(randomRefugeeGroup),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
        
        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            RefugeeGroup randomRefugeeGroup = CreateRandomRefugeeGroup();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedRefugeeGroupException =
                new LockedRefugeeGroupException(databaseUpdateConcurrencyException);

            var expectedRefugeeGroupDependencyValidationException =
                new RefugeeGroupDependencyValidationException(lockedRefugeeGroupException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<RefugeeGroup> modifyRefugeeGroupTask =
                this.refugeeGroupService.ModifyRefugeeGroupAsync(randomRefugeeGroup);

            RefugeeGroupDependencyValidationException actualRefugeeGroupDependencyValidationException =
                await Assert.ThrowsAsync<RefugeeGroupDependencyValidationException>(
                    modifyRefugeeGroupTask.AsTask);

            // then
            actualRefugeeGroupDependencyValidationException.Should()
                .BeEquivalentTo(expectedRefugeeGroupDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRefugeeGroupByIdAsync(randomRefugeeGroup.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRefugeeGroupDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRefugeeGroupAsync(randomRefugeeGroup),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
        
        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            RefugeeGroup randomRefugeeGroup = CreateRandomRefugeeGroup();
            var serviceException = new Exception();

            var failedRefugeeGroupServiceException =
                new FailedRefugeeGroupServiceException(serviceException);

            var expectedRefugeeGroupServiceException =
                new RefugeeGroupServiceException(failedRefugeeGroupServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<RefugeeGroup> modifyRefugeeGroupTask =
                this.refugeeGroupService.ModifyRefugeeGroupAsync(randomRefugeeGroup);

            RefugeeGroupServiceException actualRefugeeGroupServiceException =
                await Assert.ThrowsAsync<RefugeeGroupServiceException>(
                    modifyRefugeeGroupTask.AsTask);

            // then
            actualRefugeeGroupServiceException.Should()
                .BeEquivalentTo(expectedRefugeeGroupServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRefugeeGroupAsync(randomRefugeeGroup),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRefugeeGroupServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRefugeeGroupAsync(randomRefugeeGroup),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}