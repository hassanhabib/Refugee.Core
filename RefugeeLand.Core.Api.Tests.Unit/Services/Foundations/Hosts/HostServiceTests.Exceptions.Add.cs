// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using RefugeeLand.Core.Api.Models.Hosts;
using RefugeeLand.Core.Api.Models.Hosts.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.Hosts
{
    public partial class HostServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Host someHost = CreateRandomHost();
            SqlException sqlException = GetSqlException();

            var failedHostStorageException =
                new FailedHostStorageException(sqlException);

            var expectedHostDependencyException =
                new HostDependencyException(failedHostStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Host> addHostTask =
                this.hostService.AddHostAsync(someHost);

            HostDependencyException actualHostDependencyException =
                await Assert.ThrowsAsync<HostDependencyException>(
                    addHostTask.AsTask);

            // then
            actualHostDependencyException.Should()
                .BeEquivalentTo(expectedHostDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHostAsync(It.IsAny<Host>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedHostDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfHostAlreadyExsitsAndLogItAsync()
        {
            // given
            Host randomHost = CreateRandomHost();
            Host alreadyExistsHost = randomHost;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsHostException =
                new AlreadyExistsHostException(duplicateKeyException);

            var expectedHostDependencyValidationException =
                new HostDependencyValidationException(alreadyExistsHostException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<Host> addHostTask =
                this.hostService.AddHostAsync(alreadyExistsHost);

            // then
            HostDependencyValidationException actualHostDependencyValidationException =
                await Assert.ThrowsAsync<HostDependencyValidationException>(
                    addHostTask.AsTask);

            actualHostDependencyValidationException.Should()
                .BeEquivalentTo(expectedHostDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHostAsync(It.IsAny<Host>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            Host someHost = CreateRandomHost();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidHostReferenceException =
                new InvalidHostReferenceException(foreignKeyConstraintConflictException);

            var expectedHostValidationException =
                new HostDependencyValidationException(invalidHostReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<Host> addHostTask =
                this.hostService.AddHostAsync(someHost);

            // then
            HostDependencyValidationException actualHostDependencyValidationException =
                await Assert.ThrowsAsync<HostDependencyValidationException>(
                    addHostTask.AsTask);

            actualHostDependencyValidationException.Should()
                .BeEquivalentTo(expectedHostValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHostAsync(someHost),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Host someHost = CreateRandomHost();

            var databaseUpdateException =
                new DbUpdateException();

            var failedHostStorageException =
                new FailedHostStorageException(databaseUpdateException);

            var expectedHostDependencyException =
                new HostDependencyException(failedHostStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Host> addHostTask =
                this.hostService.AddHostAsync(someHost);

            HostDependencyException actualHostDependencyException =
                await Assert.ThrowsAsync<HostDependencyException>(
                    addHostTask.AsTask);

            // then
            actualHostDependencyException.Should()
                .BeEquivalentTo(expectedHostDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHostAsync(It.IsAny<Host>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Host someHost = CreateRandomHost();
            var serviceException = new Exception();

            var failedHostServiceException =
                new FailedHostServiceException(serviceException);

            var expectedHostServiceException =
                new HostServiceException(failedHostServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<Host> addHostTask =
                this.hostService.AddHostAsync(someHost);

            HostServiceException actualHostServiceException =
                await Assert.ThrowsAsync<HostServiceException>(
                    addHostTask.AsTask);

            // then
            actualHostServiceException.Should()
                .BeEquivalentTo(expectedHostServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHostAsync(It.IsAny<Host>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}