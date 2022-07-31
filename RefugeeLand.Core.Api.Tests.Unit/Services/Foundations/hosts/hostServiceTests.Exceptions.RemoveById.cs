using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            host randomhost = CreateRandomhost();
            SqlException sqlException = GetSqlException();

            var failedhostStorageException =
                new FailedhostStorageException(sqlException);

            var expectedhostDependencyException =
                new hostDependencyException(failedhostStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelecthostByIdAsync(randomhost.Id))
                    .Throws(sqlException);

            // when
            ValueTask<host> addhostTask =
                this.hostService.RemovehostByIdAsync(randomhost.Id);

            hostDependencyException actualhostDependencyException =
                await Assert.ThrowsAsync<hostDependencyException>(
                    addhostTask.AsTask);

            // then
            actualhostDependencyException.Should()
                .BeEquivalentTo(expectedhostDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelecthostByIdAsync(randomhost.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedhostDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletehostAsync(It.IsAny<host>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid somehostId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedhostException =
                new LockedhostException(databaseUpdateConcurrencyException);

            var expectedhostDependencyValidationException =
                new hostDependencyValidationException(lockedhostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelecthostByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<host> removehostByIdTask =
                this.hostService.RemovehostByIdAsync(somehostId);

            hostDependencyValidationException actualhostDependencyValidationException =
                await Assert.ThrowsAsync<hostDependencyValidationException>(
                    removehostByIdTask.AsTask);

            // then
            actualhostDependencyValidationException.Should()
                .BeEquivalentTo(expectedhostDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelecthostByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedhostDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletehostAsync(It.IsAny<host>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid somehostId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedhostStorageException =
                new FailedhostStorageException(sqlException);

            var expectedhostDependencyException =
                new hostDependencyException(failedhostStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelecthostByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<host> deletehostTask =
                this.hostService.RemovehostByIdAsync(somehostId);

            hostDependencyException actualhostDependencyException =
                await Assert.ThrowsAsync<hostDependencyException>(
                    deletehostTask.AsTask);

            // then
            actualhostDependencyException.Should()
                .BeEquivalentTo(expectedhostDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelecthostByIdAsync(It.IsAny<Guid>()),
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
        public async Task ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid somehostId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedhostServiceException =
                new FailedhostServiceException(serviceException);

            var expectedhostServiceException =
                new hostServiceException(failedhostServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelecthostByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<host> removehostByIdTask =
                this.hostService.RemovehostByIdAsync(somehostId);

            hostServiceException actualhostServiceException =
                await Assert.ThrowsAsync<hostServiceException>(
                    removehostByIdTask.AsTask);

            // then
            actualhostServiceException.Should()
                .BeEquivalentTo(expectedhostServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelecthostByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedhostServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}