// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Microsoft.Data.SqlClient;
using Moq;
using RefugeeLand.Core.Api.Models.Refugees.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Foundations
{
    public partial class RefugeeServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAndLogIt()
        {
            //given
            SqlException sqlException = GetSqlException();

            var failedRefugeeStorageException =
                new FailedRefugeeStorageException(sqlException);

            var expectedRefugeeDependencyException =
                new RefugeeDependencyException(failedRefugeeStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllRefugees())
                    .Throws(sqlException);

            //when
            Action retrieveAllRefugeesAction = () =>
                this.refugeeService.RetreiveAllRefugees();

            //then
            Assert.Throws<RefugeeDependencyException>(retrieveAllRefugeesAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllRefugees(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedRefugeeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
