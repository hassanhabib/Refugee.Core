// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

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

            // then
            await Assert.ThrowsAsync<RefugeeValidationException>(() =>
                addRefugeeTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRefugeeAsync(nullRefugee),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRefugeeValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
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
                key: nameof(Refugee.Email),
                values: "Text is required");

            invalidRefugeeException.AddData(
                key: nameof(Refugee.CreatedDate),
                values: "Date is required");

            invalidRefugeeException.AddData(
                key: nameof(Refugee.UpdatedDate),
                values: "Date is required");

            var expectedRefugeeException = 
                new RefugeeValidationException(invalidRefugeeException);

            // when
            ValueTask<Refugee> addRefugeeTask =
                this.refugeeService.AddRefugeeAsync(invalidRefugee);

            // then
            await Assert.ThrowsAsync<RefugeeValidationException>(() =>
                addRefugeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRefugeeException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRefugeeAsync(It.IsAny<Refugee>()), 
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedAndUpdatedDatesAreNotSameAndLogItAsync() 
        {
            // given
            Refugee randomRefugee = CreateRandomRefugee();
            Refugee invalidRefugee = randomRefugee;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            invalidRefugee.UpdatedDate = randomDateTime;
            var invalidRefugeeException = new InvalidRefugeeException();

            invalidRefugeeException.AddData(
                key: nameof(Refugee.UpdatedDate),
                values: $"Date is not the same as {nameof(Refugee.CreatedDate)}");

            var expectedRefugeeValidationException =
                new RefugeeValidationException(invalidRefugeeException);

            // when
            ValueTask<Refugee> addRefugeeTask =
                this.refugeeService.AddRefugeeAsync(invalidRefugee);

            // then
            await Assert.ThrowsAsync<RefugeeValidationException>(() =>
                addRefugeeTask.AsTask()); 

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRefugeeValidationException))), 
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
