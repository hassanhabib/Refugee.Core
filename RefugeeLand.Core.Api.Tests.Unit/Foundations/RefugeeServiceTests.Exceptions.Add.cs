// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Microsoft.Data.SqlClient;
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
            
            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedRefugeeDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRefugeeAsync(It.IsAny<Refugee>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
         }
    }
}
