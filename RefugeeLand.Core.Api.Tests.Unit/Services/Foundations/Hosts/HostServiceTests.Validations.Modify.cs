using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using RefugeeLand.Core.Api.Models.Hosts;
using RefugeeLand.Core.Api.Models.Hosts.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.Hosts
{
    public partial class HostServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfHostIsNullAndLogItAsync()
        {
            // given
            Host nullHost = null;
            var nullHostException = new NullHostException();

            var expectedHostValidationException =
                new HostValidationException(nullHostException);

            // when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(nullHost);

            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(
                    modifyHostTask.AsTask);

            // then
            actualHostValidationException.Should()
                .BeEquivalentTo(expectedHostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHostAsync(It.IsAny<Host>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfHostIsInvalidAndLogItAsync(string invalidText)
        {
            HostGender invalidGender = (HostGender)42; // Forcing out of range to build invalid enum

            var invalidHost = new Host
            {
                FirstName = invalidText,
                MiddleName = invalidText,
                LastName = invalidText,
                AdditionalDetails = invalidText,
                Gender = invalidGender
            };

            var invalidHostException = new InvalidHostException();

            invalidHostException.AddData(
                key: nameof(Host.Id),
                values: "Id is required");

            invalidHostException.AddData(
                key: nameof(Host.FirstName),
                values: "Text is required");
            
            invalidHostException.AddData(
                key: nameof(Host.MiddleName),
                values: "Text is required");
            
            invalidHostException.AddData(
                key: nameof(Host.LastName),
                values: "Text is required");
            
            invalidHostException.AddData(
                key: nameof(Host.AdditionalDetails),
                values: "Text is required");
            
            invalidHostException.AddData(
                key: nameof(Host.Gender),
                values: "Value is invalid");
            
            invalidHostException.AddData(
                key: nameof(Host.BirthDate),
                values: "Date is required");

            invalidHostException.AddData(
                key: nameof(Host.CreatedDate),
                values: "Date is required");

            invalidHostException.AddData(
                key: nameof(Host.CreatedByUserId),
                values: "Id is required");

            invalidHostException.AddData(
                key: nameof(Host.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(Host.CreatedDate)}"
                });

            invalidHostException.AddData(
                key: nameof(Host.UpdatedByUserId),
                values: "Id is required");

            var expectedHostValidationException =
                new HostValidationException(invalidHostException);

            // when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(invalidHost);

            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(
                    modifyHostTask.AsTask);

            //then
            actualHostValidationException.Should()
                .BeEquivalentTo(expectedHostValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHostAsync(It.IsAny<Host>()),
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
            Host randomHost = CreateRandomHost(randomDateTimeOffset);
            Host invalidHost = randomHost;
            var invalidHostException = new InvalidHostException();

            invalidHostException.AddData(
                key: nameof(Host.UpdatedDate),
                values: $"Date is the same as {nameof(Host.CreatedDate)}");

            var expectedHostValidationException =
                new HostValidationException(invalidHostException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(invalidHost);

            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(
                    modifyHostTask.AsTask);

            // then
            actualHostValidationException.Should()
                .BeEquivalentTo(expectedHostValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHostByIdAsync(invalidHost.Id),
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
            Host randomHost = CreateRandomHost(randomDateTimeOffset);
            randomHost.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidHostException =
                new InvalidHostException();

            invalidHostException.AddData(
                key: nameof(Host.UpdatedDate),
                values: "Date is not recent");

            var expectedHostValidatonException =
                new HostValidationException(invalidHostException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(randomHost);

            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(
                    modifyHostTask.AsTask);

            // then
            actualHostValidationException.Should()
                .BeEquivalentTo(expectedHostValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHostByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfHostDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Host randomHost = CreateRandomModifyHost(randomDateTimeOffset);
            Host nonExistHost = randomHost;
            Host nullHost = null;

            var notFoundHostException =
                new NotFoundHostException(nonExistHost.Id);

            var expectedHostValidationException =
                new HostValidationException(notFoundHostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHostByIdAsync(nonExistHost.Id))
                .ReturnsAsync(nullHost);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when 
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(nonExistHost);

            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(
                    modifyHostTask.AsTask);

            // then
            actualHostValidationException.Should()
                .BeEquivalentTo(expectedHostValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHostByIdAsync(nonExistHost.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostValidationException))),
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
            Host randomHost = CreateRandomModifyHost(randomDateTimeOffset);
            Host invalidHost = randomHost.DeepClone();
            Host storageHost = invalidHost.DeepClone();
            storageHost.CreatedDate = storageHost.CreatedDate.AddMinutes(randomMinutes);
            storageHost.UpdatedDate = storageHost.UpdatedDate.AddMinutes(randomMinutes);
            var invalidHostException = new InvalidHostException();

            invalidHostException.AddData(
                key: nameof(Host.CreatedDate),
                values: $"Date is not the same as {nameof(Host.CreatedDate)}");

            var expectedHostValidationException =
                new HostValidationException(invalidHostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHostByIdAsync(invalidHost.Id))
                .ReturnsAsync(storageHost);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(invalidHost);

            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(
                    modifyHostTask.AsTask);

            // then
            actualHostValidationException.Should()
                .BeEquivalentTo(expectedHostValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHostByIdAsync(invalidHost.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedHostValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserIdDontMacthStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Host randomHost = CreateRandomModifyHost(randomDateTimeOffset);
            Host invalidHost = randomHost.DeepClone();
            Host storageHost = invalidHost.DeepClone();
            invalidHost.CreatedByUserId = Guid.NewGuid();
            storageHost.UpdatedDate = storageHost.CreatedDate;

            var invalidHostException = new InvalidHostException();

            invalidHostException.AddData(
                key: nameof(Host.CreatedByUserId),
                values: $"Id is not the same as {nameof(Host.CreatedByUserId)}");

            var expectedHostValidationException =
                new HostValidationException(invalidHostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHostByIdAsync(invalidHost.Id))
                .ReturnsAsync(storageHost);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(invalidHost);

            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(
                    modifyHostTask.AsTask);

            // then
            actualHostValidationException.Should().BeEquivalentTo(expectedHostValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHostByIdAsync(invalidHost.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedHostValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Host randomHost = CreateRandomModifyHost(randomDateTimeOffset);
            Host invalidHost = randomHost;
            Host storageHost = randomHost.DeepClone();

            var invalidHostException = new InvalidHostException();

            invalidHostException.AddData(
                key: nameof(Host.UpdatedDate),
                values: $"Date is the same as {nameof(Host.UpdatedDate)}");

            var expectedHostValidationException =
                new HostValidationException(invalidHostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHostByIdAsync(invalidHost.Id))
                .ReturnsAsync(storageHost);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(invalidHost);

            // then
            await Assert.ThrowsAsync<HostValidationException>(
                modifyHostTask.AsTask);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHostByIdAsync(invalidHost.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}