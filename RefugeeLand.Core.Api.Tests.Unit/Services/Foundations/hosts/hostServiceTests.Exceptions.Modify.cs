using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
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
    }
}