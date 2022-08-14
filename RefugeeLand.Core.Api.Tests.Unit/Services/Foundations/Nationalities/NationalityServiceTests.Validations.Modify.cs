using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using RefugeeLand.Core.Api.Models.Nationalities;
using RefugeeLand.Core.Api.Models.Nationalities.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.Nationalities
{
    public partial class NationalityServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfNationalityIsNullAndLogItAsync()
        {
            // given
            Nationality nullNationality = null;
            var nullNationalityException = new NullNationalityException();

            var expectedNationalityValidationException =
                new NationalityValidationException(nullNationalityException);

            // when
            ValueTask<Nationality> modifyNationalityTask =
                this.nationalityService.ModifyNationalityAsync(nullNationality);

            NationalityValidationException actualNationalityValidationException =
                await Assert.ThrowsAsync<NationalityValidationException>(
                    modifyNationalityTask.AsTask);

            // then
            actualNationalityValidationException.Should()
                .BeEquivalentTo(expectedNationalityValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedNationalityValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateNationalityAsync(It.IsAny<Nationality>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfNationalityIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidNationality = new Nationality
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidNationalityException = new InvalidNationalityException();

            invalidNationalityException.AddData(
                key: nameof(Nationality.Id),
                values: "Id is required");

            //invalidNationalityException.AddData(
            //    key: nameof(Nationality.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the Nationality model

            invalidNationalityException.AddData(
                key: nameof(Nationality.CreatedDate),
                values: "Date is required");

            invalidNationalityException.AddData(
                key: nameof(Nationality.CreatedByUserId),
                values: "Id is required");

            invalidNationalityException.AddData(
                key: nameof(Nationality.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(Nationality.CreatedDate)}"
                });

            invalidNationalityException.AddData(
                key: nameof(Nationality.UpdatedByUserId),
                values: "Id is required");

            var expectedNationalityValidationException =
                new NationalityValidationException(invalidNationalityException);

            // when
            ValueTask<Nationality> modifyNationalityTask =
                this.nationalityService.ModifyNationalityAsync(invalidNationality);

            NationalityValidationException actualNationalityValidationException =
                await Assert.ThrowsAsync<NationalityValidationException>(
                    modifyNationalityTask.AsTask);

            //then
            actualNationalityValidationException.Should()
                .BeEquivalentTo(expectedNationalityValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedNationalityValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateNationalityAsync(It.IsAny<Nationality>()),
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
            Nationality randomNationality = CreateRandomNationality(randomDateTimeOffset);
            Nationality invalidNationality = randomNationality;
            var invalidNationalityException = new InvalidNationalityException();

            invalidNationalityException.AddData(
                key: nameof(Nationality.UpdatedDate),
                values: $"Date is the same as {nameof(Nationality.CreatedDate)}");

            var expectedNationalityValidationException =
                new NationalityValidationException(invalidNationalityException);

            // when
            ValueTask<Nationality> modifyNationalityTask =
                this.nationalityService.ModifyNationalityAsync(invalidNationality);

            NationalityValidationException actualNationalityValidationException =
                await Assert.ThrowsAsync<NationalityValidationException>(
                    modifyNationalityTask.AsTask);

            // then
            actualNationalityValidationException.Should()
                .BeEquivalentTo(expectedNationalityValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedNationalityValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectNationalityByIdAsync(invalidNationality.Id),
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
            Nationality randomNationality = CreateRandomNationality(randomDateTimeOffset);
            randomNationality.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidNationalityException =
                new InvalidNationalityException();

            invalidNationalityException.AddData(
                key: nameof(Nationality.UpdatedDate),
                values: "Date is not recent");

            var expectedNationalityValidatonException =
                new NationalityValidationException(invalidNationalityException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<Nationality> modifyNationalityTask =
                this.nationalityService.ModifyNationalityAsync(randomNationality);

            NationalityValidationException actualNationalityValidationException =
                await Assert.ThrowsAsync<NationalityValidationException>(
                    modifyNationalityTask.AsTask);

            // then
            actualNationalityValidationException.Should().BeEquivalentTo(expectedNationalityValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedNationalityValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectNationalityByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}