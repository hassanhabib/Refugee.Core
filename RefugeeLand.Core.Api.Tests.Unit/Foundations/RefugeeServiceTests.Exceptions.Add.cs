// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using RefugeeLand.Core.Api.Models.Refugees;
using RefugeeLand.Core.Api.Models.Refugees.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Foundations
{
    public partial class RefugeeServiceTests
    {
        [Fact]
        public async Task ShouldThrowCritcalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Refugee someRefugee = CreateRandomRefugee();
            SqlException sqlException = GetSqlException();

            FailedRefugeeStorageException failedRefugeeStorageException =
                new FailedRefugeeStorageException(sqlException);

            var expectedRefugeeDependencyException =
                new RefugeeDependencyException(failedRefugeeStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffset())
                        .Throws(sqlException);

            // when
            ValueTask<Refugee> addRefugeeTask =
                this.refugeeService.AddRefugeeAsync(someRefugee);

            // then
            await Assert.ThrowsAsync<RefugeeDependencyException>(() =>
                addRefugeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTimeOffset(),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogCritical(It.Is(SameExceptionAs(
                        expectedRefugeeDependencyException))),
                            Times.Once);

            this.storageBrokerMock.Verify(broker =>
                    broker.InsertRefugeeAsync(It.IsAny<Refugee>()),
                        Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfRefugeeAlreadyExistAndLogItAsync()
        {
            // given
            Refugee randomRefugee = CreateRandomRefugee();
            Refugee alreadyExistRefugee = randomRefugee;
            string message = GetRandomString();
            var duplicateKeyException = new DuplicateKeyException(message);

            var alreadyExistException =
                new AlreadyExistRefugeeException(duplicateKeyException);

            var expectedRefugeeDependencyValidationException =
                new RefugeeDependencyValidationException(alreadyExistException);

            this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffset())
                        .Throws(duplicateKeyException);

            // when
            ValueTask<Refugee> addRefugeeTask =
                this.refugeeService.AddRefugeeAsync(alreadyExistRefugee);

            // then
            await Assert.ThrowsAsync<RefugeeDependencyValidationException>(() =>
                addRefugeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTimeOffset(),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogError(It.Is(SameExceptionAs(
                        expectedRefugeeDependencyValidationException))),
                            Times.Once);

            this.storageBrokerMock.Verify(broker =>
                    broker.InsertRefugeeAsync(It.IsAny<Refugee>()),
                        Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDbUpdateErrorOccurs()
        {
            // given
            Refugee randomRefugee = CreateRandomRefugee();
            Refugee someRefugee = randomRefugee;
            var dbUpdateException = new DbUpdateException();

            var failedRefugeeStorageException = 
                new FailedRefugeeStorageException(dbUpdateException);

            var expectedRefugeeDependencyValidationException =
                new RefugeeDependencyValidationException(failedRefugeeStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffset())
                        .Throws(dbUpdateException);

            // when
            ValueTask<Refugee> addRefugeeTask =
                this.refugeeService.AddRefugeeAsync(someRefugee);

            // then
            await Assert.ThrowsAsync<RefugeeDependencyValidationException>(() =>
                addRefugeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTimeOffset(),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogError(It.Is(SameExceptionAs(
                        expectedRefugeeDependencyValidationException))),
                            Times.Once);

            this.storageBrokerMock.Verify(broker =>
                    broker.InsertRefugeeAsync(It.IsAny<Refugee>()),
                        Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Refugee randomRefugee = CreateRandomRefugee();
            Refugee someRefugee = randomRefugee;
            var serviceException = new Exception();
            var failedRefugeeServiceException = new FailedRefugeeServiceException(serviceException);

            var expectedRefugeeServiceException =
                new RefugeeServiceException(failedRefugeeServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffset())
                        .Throws(serviceException);

            // when
            ValueTask<Refugee> addRefugeeTask =
                this.refugeeService.AddRefugeeAsync(someRefugee);

            // then
            await Assert.ThrowsAsync<RefugeeServiceException>(() =>
                addRefugeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTimeOffset(),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogError(It.Is(SameExceptionAs(
                        expectedRefugeeServiceException))),
                            Times.Once);

            this.storageBrokerMock.Verify(broker =>
                    broker.InsertRefugeeAsync(It.IsAny<Refugee>()),
                        Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}