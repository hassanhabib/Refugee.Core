using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using RefugeeLand.Core.Api.Models.hosts;
using RefugeeLand.Core.Api.Models.hosts.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.hosts
{
    public partial class hostServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            host somehost = CreateRandomhost();
            SqlException sqlException = GetSqlException();

            var failedhostStorageException =
                new FailedhostStorageException(sqlException);

            var expectedhostDependencyException =
                new hostDependencyException(failedhostStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<host> addhostTask =
                this.hostService.AddhostAsync(somehost);

            hostDependencyException actualhostDependencyException =
                await Assert.ThrowsAsync<hostDependencyException>(
                    addhostTask.AsTask);

            // then
            actualhostDependencyException.Should()
                .BeEquivalentTo(expectedhostDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InserthostAsync(It.IsAny<host>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedhostDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfhostAlreadyExsitsAndLogItAsync()
        {
            // given
            host randomhost = CreateRandomhost();
            host alreadyExistshost = randomhost;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistshostException =
                new AlreadyExistshostException(duplicateKeyException);

            var expectedhostDependencyValidationException =
                new hostDependencyValidationException(alreadyExistshostException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<host> addhostTask =
                this.hostService.AddhostAsync(alreadyExistshost);

            // then
            hostDependencyValidationException actualhostDependencyValidationException =
                await Assert.ThrowsAsync<hostDependencyValidationException>(
                    addhostTask.AsTask);

            actualhostDependencyValidationException.Should()
                .BeEquivalentTo(expectedhostDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InserthostAsync(It.IsAny<host>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedhostDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            host somehost = CreateRandomhost();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidhostReferenceException =
                new InvalidhostReferenceException(foreignKeyConstraintConflictException);

            var expectedhostValidationException =
                new hostDependencyValidationException(invalidhostReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<host> addhostTask =
                this.hostService.AddhostAsync(somehost);

            // then
            hostDependencyValidationException actualhostDependencyValidationException =
                await Assert.ThrowsAsync<hostDependencyValidationException>(
                    addhostTask.AsTask);

            actualhostDependencyValidationException.Should()
                .BeEquivalentTo(expectedhostValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedhostValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InserthostAsync(somehost),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            host somehost = CreateRandomhost();

            var databaseUpdateException =
                new DbUpdateException();

            var failedhostStorageException =
                new FailedhostStorageException(databaseUpdateException);

            var expectedhostDependencyException =
                new hostDependencyException(failedhostStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<host> addhostTask =
                this.hostService.AddhostAsync(somehost);

            hostDependencyException actualhostDependencyException =
                await Assert.ThrowsAsync<hostDependencyException>(
                    addhostTask.AsTask);

            // then
            actualhostDependencyException.Should()
                .BeEquivalentTo(expectedhostDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InserthostAsync(It.IsAny<host>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedhostDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            host somehost = CreateRandomhost();
            var serviceException = new Exception();

            var failedhostServiceException =
                new FailedhostServiceException(serviceException);

            var expectedhostServiceException =
                new hostServiceException(failedhostServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<host> addhostTask =
                this.hostService.AddhostAsync(somehost);

            hostServiceException actualhostServiceException =
                await Assert.ThrowsAsync<hostServiceException>(
                    addhostTask.AsTask);

            // then
            actualhostServiceException.Should()
                .BeEquivalentTo(expectedhostServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InserthostAsync(It.IsAny<host>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedhostServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}