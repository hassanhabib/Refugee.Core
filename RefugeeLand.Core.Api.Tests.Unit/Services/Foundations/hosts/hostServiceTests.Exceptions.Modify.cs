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
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            host randomhost = CreateRandomhost();
            SqlException sqlException = GetSqlException();

            var failedhostStorageException =
                new FailedhostStorageException(sqlException);

            var expectedhostDependencyException =
                new hostDependencyException(failedhostStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<host> modifyhostTask =
                this.hostService.ModifyhostAsync(randomhost);

            hostDependencyException actualhostDependencyException =
                await Assert.ThrowsAsync<hostDependencyException>(
                    modifyhostTask.AsTask);

            // then
            actualhostDependencyException.Should()
                .BeEquivalentTo(expectedhostDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelecthostByIdAsync(randomhost.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedhostDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatehostAsync(randomhost),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            host somehost = CreateRandomhost();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidhostReferenceException =
                new InvalidhostReferenceException(foreignKeyConstraintConflictException);

            hostDependencyValidationException expectedhostDependencyValidationException =
                new hostDependencyValidationException(invalidhostReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<host> modifyhostTask =
                this.hostService.ModifyhostAsync(somehost);

            hostDependencyValidationException actualhostDependencyValidationException =
                await Assert.ThrowsAsync<hostDependencyValidationException>(
                    modifyhostTask.AsTask);

            // then
            actualhostDependencyValidationException.Should()
                .BeEquivalentTo(expectedhostDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelecthostByIdAsync(somehost.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedhostDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatehostAsync(somehost),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            host randomhost = CreateRandomhost();
            var databaseUpdateException = new DbUpdateException();

            var failedhostStorageException =
                new FailedhostStorageException(databaseUpdateException);

            var expectedhostDependencyException =
                new hostDependencyException(failedhostStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<host> modifyhostTask =
                this.hostService.ModifyhostAsync(randomhost);

            hostDependencyException actualhostDependencyException =
                await Assert.ThrowsAsync<hostDependencyException>(
                    modifyhostTask.AsTask);

            // then
            actualhostDependencyException.Should()
                .BeEquivalentTo(expectedhostDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelecthostByIdAsync(randomhost.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedhostDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatehostAsync(randomhost),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            host randomhost = CreateRandomhost();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedhostException =
                new LockedhostException(databaseUpdateConcurrencyException);

            var expectedhostDependencyValidationException =
                new hostDependencyValidationException(lockedhostException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<host> modifyhostTask =
                this.hostService.ModifyhostAsync(randomhost);

            hostDependencyValidationException actualhostDependencyValidationException =
                await Assert.ThrowsAsync<hostDependencyValidationException>(
                    modifyhostTask.AsTask);

            // then
            actualhostDependencyValidationException.Should()
                .BeEquivalentTo(expectedhostDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelecthostByIdAsync(randomhost.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedhostDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatehostAsync(randomhost),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}