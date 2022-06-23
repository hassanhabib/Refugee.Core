// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using FluentAssertions;
using Moq;
using RefugeeLand.Core.Api.Models.Refugees;
using RefugeeLand.Core.Api.Models.Refugees.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Foundations
{
    public partial class RefugeeServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfRefugeeIsNullAndLogItAsync()
        {
            // given
            Refugee nullRefugee = null;
            var nullRefugeeException = new NullRefugeeException();

            var expectedRefugeeValidationException =
                new RefugeeValidationException(nullRefugeeException);

            // when
            ValueTask<Refugee> addRefugeeTask =
                this.refugeeService.AddRefugeeAsync(nullRefugee);

            RefugeeValidationException actualRefugeeValidationException =
                await Assert.ThrowsAsync<RefugeeValidationException>(
                    addRefugeeTask.AsTask);

            // then
            actualRefugeeValidationException.Should().BeEquivalentTo(
                expectedRefugeeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogError(It.Is(SameExceptionAs(
                        expectedRefugeeValidationException))),
                            Times.Once);

            this.storageBrokerMock.Verify(broker =>
                    broker.InsertRefugeeAsync(nullRefugee),
                        Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfRefugeeIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidRefugee = new Refugee
            {
                FirstName = invalidText
            };

            var invalidRefugeeException = new InvalidRefugeeException();

            invalidRefugeeException.AddData(
                key: nameof(Refugee.Id),
                values: "Id is required");

            invalidRefugeeException.AddData(
                key: nameof(Refugee.FirstName),
                values: "Text is required");

            invalidRefugeeException.AddData(
                key: nameof(Refugee.LastName),
                values: "Text is required");

            invalidRefugeeException.AddData(
                key: nameof(Refugee.BirthDate),
                values: "Date is required");

            invalidRefugeeException.AddData(
                key: nameof(Refugee.CreatedDate),
                values: "Date is required");

            invalidRefugeeException.AddData(
                key: nameof(Refugee.UpdatedDate),
                values: "Date is required");

            var expectedRefugeeValidationException =
                new RefugeeValidationException(invalidRefugeeException);

            // when
            ValueTask<Refugee> addRefugeeTask =
                this.refugeeService.AddRefugeeAsync(invalidRefugee);

            RefugeeValidationException actualRefugeeValidationException =
                await Assert.ThrowsAsync<RefugeeValidationException>(
                    addRefugeeTask.AsTask);

            // then
            actualRefugeeValidationException.Should().BeEquivalentTo(
                expectedRefugeeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTimeOffset(),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogError(It.Is(SameExceptionAs(
                        expectedRefugeeValidationException))),
                            Times.Once);

            this.storageBrokerMock.Verify(broker =>
                    broker.InsertRefugeeAsync(It.IsAny<Refugee>()),
                        Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedAndUpdatedDatesAreNotSameAndLogItAsync()
        {
            // given
            
            DateTimeOffset dateTime = GetRandomDateTime();
            Refugee randomRefugee = CreateRandomRefugee(dateTime);
            Refugee invalidRefugee = randomRefugee;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            invalidRefugee.UpdatedDate = randomDateTime;
            var invalidRefugeeException = new InvalidRefugeeException();

            invalidRefugeeException.AddData(
                key: nameof(Refugee.UpdatedDate),
                values: $"Date is not the same as {nameof(Refugee.CreatedDate)}");

            var expectedRefugeeValidationException =
                new RefugeeValidationException(invalidRefugeeException);

            this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffset())
                        .Returns(dateTime);

            // when
            ValueTask<Refugee> addRefugeeTask =
                this.refugeeService.AddRefugeeAsync(invalidRefugee);

            RefugeeValidationException actualRefugeeValidationException =
                await Assert.ThrowsAsync<RefugeeValidationException>(
                    addRefugeeTask.AsTask);

            // then
            actualRefugeeValidationException.Should().BeEquivalentTo(
                expectedRefugeeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTimeOffset(),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogError(It.Is(SameExceptionAs(
                        expectedRefugeeValidationException))),
                            Times.Once);

            this.storageBrokerMock.Verify(broker =>
                    broker.InsertRefugeeAsync(It.IsAny<Refugee>()),
                        Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset invalidDateTime = randomDateTime.AddMinutes(minutesBeforeOrAfter);
            Refugee randomRefugee = CreateRandomRefugee(invalidDateTime);
            Refugee invalidRefugee = randomRefugee;
            var invalidRefugeeException = new InvalidRefugeeException();

            invalidRefugeeException.AddData(
                key: nameof(Refugee.CreatedDate),
                values: "Date is not recent");

            var expectedRefugeeValidationException =
                new RefugeeValidationException(invalidRefugeeException);

            this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffset())
                        .Returns(randomDateTime);

            // when
            ValueTask<Refugee> addRefugeeTask =
                this.refugeeService.AddRefugeeAsync(invalidRefugee);

            RefugeeValidationException actualRefugeeValidationException =
                await Assert.ThrowsAsync<RefugeeValidationException>(
                    addRefugeeTask.AsTask);

            // then
            actualRefugeeValidationException.Should().BeEquivalentTo(
                expectedRefugeeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTimeOffset(),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogError(It.Is(SameExceptionAs(
                        expectedRefugeeValidationException))),
                            Times.Once);

            this.storageBrokerMock.Verify(broker =>
                    broker.InsertRefugeeAsync(It.IsAny<Refugee>()),
                        Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfRefugeeGenderIsInvalidAndLogItAsync()
        {
            // given
            
            DateTimeOffset dateTime = GetRandomDateTime();
            Refugee randomRefugee = CreateRandomRefugee(dateTime);
            Refugee invalidRefugee = randomRefugee;
            invalidRefugee.Gender = GetInvalidEnum<RefugeeGender>();
            var invalidRefugeeException = new InvalidRefugeeException();

            invalidRefugeeException.AddData(
                key: nameof(Refugee.Gender),
                values: "Value is invalid");

            var expectedRefugeeValidationException =
                new RefugeeValidationException(invalidRefugeeException);
            
            this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffset())
                        .Returns(dateTime);

            // when
            ValueTask<Refugee> addRefugeeTask =
                this.refugeeService.AddRefugeeAsync(invalidRefugee);

            RefugeeValidationException actualRefugeeValidationException =
                await Assert.ThrowsAsync<RefugeeValidationException>(
                    addRefugeeTask.AsTask);

            // then
            actualRefugeeValidationException.Should().BeEquivalentTo(
                expectedRefugeeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTimeOffset(),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogError(It.Is(SameExceptionAs(
                        expectedRefugeeValidationException))),
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