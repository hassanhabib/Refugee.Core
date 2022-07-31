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
    }
}