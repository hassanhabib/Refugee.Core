﻿// -------------------------------------------------------
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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            RefugeeGroup randomRefugeeGroup = CreateRandomRefugeeGroup(randomDateTime);
            SqlException sqlException = GetSqlException();

            var failedRefugeeGroupStorageException = 
                new FailedRefugeeGroupStorageException(sqlException);

            var expectedRefugeeGroupDependencyException = 
                new RefugeeGroupDependencyException(failedRefugeeGroupStorageException);

            this.dateTimeBrokerMock.Setup(broker => 
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

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

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), 
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedRefugeeGroupDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker => 
                broker.InsertRefugeeGroupAsync(randomRefugeeGroup),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfRefugeeGroupAlreadyExistsAndLogItAsync()
        {
            //given
            RefugeeGroup randomRefugeeGroup = CreateRandomRefugeeGroup();
            RefugeeGroup alreadyExistsRefugeeGroup = randomRefugeeGroup;
            string randomMessage = GetRandomString();

            var duplicateRefugeeGroupKeyException = 
                new DuplicateKeyException(randomMessage);

            var alreadyExistsRefugeeGroupException =
                new AlreadyExistsRefugeeGroupException(duplicateRefugeeGroupKeyException);

            var expectedRefugeeGroupDependencyValidationException = 
                new RefugeeGroupDependencyValidationException(alreadyExistsRefugeeGroupException);

            this.dateTimeBrokerMock.Setup(broker => 
                broker.GetCurrentDateTimeOffset())
                    .Returns(DateTimeOffset.UtcNow);
            
            this.storageBrokerMock.Setup(broker =>
                broker.InsertRefugeeGroupAsync(randomRefugeeGroup))
                    .Throws(duplicateRefugeeGroupKeyException);
            
            //when
            ValueTask<RefugeeGroup> addRefugeeGroupTask =
                this.refugeeGroupService.AddRefugeeGroupAsync(alreadyExistsRefugeeGroup);

            RefugeeGroupDependencyValidationException actualRefugeeGroupDependencyValidationException =
                await Assert.ThrowsAsync<RefugeeGroupDependencyValidationException>(
                    addRefugeeGroupTask.AsTask);
            
            //then
            actualRefugeeGroupDependencyValidationException.Should().BeEquivalentTo(
                expectedRefugeeGroupDependencyValidationException);
            
            
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRefugeeGroupDependencyValidationException))),
                        Times.Once);
            
            this.storageBrokerMock.Verify(broker =>
                broker.InsertRefugeeGroupAsync(It.IsAny<RefugeeGroup>()),
                    Times.Once);
            
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
        
       [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            RefugeeGroup randomRefugeeGroup = CreateRandomRefugeeGroup();

            var databaseUpdateException =
                new DbUpdateException();

            var failedRefugeeGroupStorageException =
                new FailedRefugeeGroupStorageException(databaseUpdateException);

            var expectedRefugeeGroupDependencyException =
                new RefugeeGroupDependencyException(failedRefugeeGroupStorageException);
            
            this.dateTimeBrokerMock.Setup(broker => 
                broker.GetCurrentDateTimeOffset())
                    .Returns(DateTimeOffset.UtcNow);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertRefugeeGroupAsync(randomRefugeeGroup))
                    .Throws(databaseUpdateException);

            // when
            ValueTask<RefugeeGroup> addRefugeeGroupTask =
                this.refugeeGroupService.AddRefugeeGroupAsync(randomRefugeeGroup);

            RefugeeGroupDependencyException actualRefugeeGroupDependencyException =
                await Assert.ThrowsAsync<RefugeeGroupDependencyException>(
                    addRefugeeGroupTask.AsTask);

            // then
            actualRefugeeGroupDependencyException.Should().BeEquivalentTo(
                expectedRefugeeGroupDependencyException);
            
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRefugeeGroupDependencyException))),
                        Times.Once);
            
            this.storageBrokerMock.Verify(broker =>
                broker.InsertRefugeeGroupAsync(It.IsAny<RefugeeGroup>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
        
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            RefugeeGroup randomRefugeeGroup = CreateRandomRefugeeGroup();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidRefugeeGroupReferenceException =
                new InvalidRefugeeGroupReferenceException(foreignKeyConstraintConflictException);

            var expectedRefugeeGroupValidationException =
                new RefugeeGroupDependencyValidationException(invalidRefugeeGroupReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(DateTimeOffset.UtcNow);
            
            this.storageBrokerMock.Setup(broker =>
                broker.InsertRefugeeGroupAsync(randomRefugeeGroup))
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<RefugeeGroup> addRefugeeGroupTask =
                this.refugeeGroupService.AddRefugeeGroupAsync(randomRefugeeGroup);
            
             RefugeeGroupDependencyValidationException actualRefugeeGroupDependencyValidationException = 
                await Assert.ThrowsAsync<RefugeeGroupDependencyValidationException>(
                    addRefugeeGroupTask.AsTask);

            // then
            actualRefugeeGroupDependencyValidationException.Should() .BeEquivalentTo(
                expectedRefugeeGroupValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRefugeeGroupValidationException))),
                        Times.Once);
            
            this.storageBrokerMock.Verify(broker =>
                broker.InsertRefugeeGroupAsync(randomRefugeeGroup),
                    Times.Once());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
        
        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
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
            ValueTask<RefugeeGroup> addRefugeeGroupTask =
                this.refugeeGroupService.AddRefugeeGroupAsync(randomRefugeeGroup);
            
            RefugeeGroupServiceException actualRefugeeGroupServiceException = 
                await Assert.ThrowsAsync<RefugeeGroupServiceException>(
                    addRefugeeGroupTask.AsTask);
                
            // then
            actualRefugeeGroupServiceException.Should().BeEquivalentTo(
                expectedRefugeeGroupServiceException);
            
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRefugeeGroupServiceException))),
                        Times.Once);
            
            this.storageBrokerMock.Verify(broker =>
                broker.InsertRefugeeGroupAsync(It.IsAny<RefugeeGroup>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}