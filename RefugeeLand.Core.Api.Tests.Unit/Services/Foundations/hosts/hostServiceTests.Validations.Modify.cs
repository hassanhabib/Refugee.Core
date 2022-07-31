using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using RefugeeLand.Core.Api.Models.hosts;
using RefugeeLand.Core.Api.Models.hosts.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.hosts
{
    public partial class hostServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfhostIsNullAndLogItAsync()
        {
            // given
            host nullhost = null;
            var nullhostException = new NullhostException();

            var expectedhostValidationException =
                new hostValidationException(nullhostException);

            // when
            ValueTask<host> modifyhostTask =
                this.hostService.ModifyhostAsync(nullhost);

            hostValidationException actualhostValidationException =
                await Assert.ThrowsAsync<hostValidationException>(
                    modifyhostTask.AsTask);

            // then
            actualhostValidationException.Should()
                .BeEquivalentTo(expectedhostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedhostValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatehostAsync(It.IsAny<host>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfhostIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidhost = new host
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidhostException = new InvalidhostException();

            invalidhostException.AddData(
                key: nameof(host.Id),
                values: "Id is required");

            //invalidhostException.AddData(
            //    key: nameof(host.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the host model

            invalidhostException.AddData(
                key: nameof(host.CreatedDate),
                values: "Date is required");

            invalidhostException.AddData(
                key: nameof(host.CreatedByUserId),
                values: "Id is required");

            invalidhostException.AddData(
                key: nameof(host.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(host.CreatedDate)}"
                });

            invalidhostException.AddData(
                key: nameof(host.UpdatedByUserId),
                values: "Id is required");

            var expectedhostValidationException =
                new hostValidationException(invalidhostException);

            // when
            ValueTask<host> modifyhostTask =
                this.hostService.ModifyhostAsync(invalidhost);

            hostValidationException actualhostValidationException =
                await Assert.ThrowsAsync<hostValidationException>(
                    modifyhostTask.AsTask);

            //then
            actualhostValidationException.Should()
                .BeEquivalentTo(expectedhostValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedhostValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatehostAsync(It.IsAny<host>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            host randomhost = CreateRandomhost(randomDateTimeOffset);
            host invalidhost = randomhost;
            var invalidhostException = new InvalidhostException();

            invalidhostException.AddData(
                key: nameof(host.UpdatedDate),
                values: $"Date is the same as {nameof(host.CreatedDate)}");

            var expectedhostValidationException =
                new hostValidationException(invalidhostException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<host> modifyhostTask =
                this.hostService.ModifyhostAsync(invalidhost);

            hostValidationException actualhostValidationException =
                await Assert.ThrowsAsync<hostValidationException>(
                    modifyhostTask.AsTask);

            // then
            actualhostValidationException.Should()
                .BeEquivalentTo(expectedhostValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedhostValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelecthostByIdAsync(invalidhost.Id),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            host randomhost = CreateRandomhost(randomDateTimeOffset);
            randomhost.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidhostException =
                new InvalidhostException();

            invalidhostException.AddData(
                key: nameof(host.UpdatedDate),
                values: "Date is not recent");

            var expectedhostValidatonException =
                new hostValidationException(invalidhostException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<host> modifyhostTask =
                this.hostService.ModifyhostAsync(randomhost);

            hostValidationException actualhostValidationException =
                await Assert.ThrowsAsync<hostValidationException>(
                    modifyhostTask.AsTask);

            // then
            actualhostValidationException.Should()
                .BeEquivalentTo(expectedhostValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedhostValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelecthostByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfhostDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            host randomhost = CreateRandomModifyhost(randomDateTimeOffset);
            host nonExisthost = randomhost;
            host nullhost = null;

            var notFoundhostException =
                new NotFoundhostException(nonExisthost.Id);

            var expectedhostValidationException =
                new hostValidationException(notFoundhostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelecthostByIdAsync(nonExisthost.Id))
                .ReturnsAsync(nullhost);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when 
            ValueTask<host> modifyhostTask =
                this.hostService.ModifyhostAsync(nonExisthost);

            hostValidationException actualhostValidationException =
                await Assert.ThrowsAsync<hostValidationException>(
                    modifyhostTask.AsTask);

            // then
            actualhostValidationException.Should()
                .BeEquivalentTo(expectedhostValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelecthostByIdAsync(nonExisthost.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedhostValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            host randomhost = CreateRandomModifyhost(randomDateTimeOffset);
            host invalidhost = randomhost.DeepClone();
            host storagehost = invalidhost.DeepClone();
            storagehost.CreatedDate = storagehost.CreatedDate.AddMinutes(randomMinutes);
            storagehost.UpdatedDate = storagehost.UpdatedDate.AddMinutes(randomMinutes);
            var invalidhostException = new InvalidhostException();

            invalidhostException.AddData(
                key: nameof(host.CreatedDate),
                values: $"Date is not the same as {nameof(host.CreatedDate)}");

            var expectedhostValidationException =
                new hostValidationException(invalidhostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelecthostByIdAsync(invalidhost.Id))
                .ReturnsAsync(storagehost);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<host> modifyhostTask =
                this.hostService.ModifyhostAsync(invalidhost);

            hostValidationException actualhostValidationException =
                await Assert.ThrowsAsync<hostValidationException>(
                    modifyhostTask.AsTask);

            // then
            actualhostValidationException.Should()
                .BeEquivalentTo(expectedhostValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelecthostByIdAsync(invalidhost.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedhostValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}