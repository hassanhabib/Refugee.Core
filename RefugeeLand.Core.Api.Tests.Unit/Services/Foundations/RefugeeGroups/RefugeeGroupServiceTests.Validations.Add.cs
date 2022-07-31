// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using FluentAssertions;
using Moq;
using RefugeeLand.Core.Api.Models.RefugeeGroups;
using RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.RefugeeGroups
{
    public partial class RefugeeGroupServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfRefugeeGroupIsNullAndLogItAsync()
        {
            //given
            RefugeeGroup nullRefugeeGroup = null;

            var nullRefugeeGroupException = 
                new NullRefugeeGroupException();

            var expectedRefugeeGroupValidationException =
                new RefugeeGroupValidationException(nullRefugeeGroupException);
            
            //when
            ValueTask<RefugeeGroup> addRefugeeGroupTask =
                this.refugeeGroupService.AddRefugeeGroupAsync(nullRefugeeGroup);
            
            RefugeeGroupValidationException actualRefugeeGroupValidationException =
                await Assert.ThrowsAsync<RefugeeGroupValidationException>(
                    addRefugeeGroupTask.AsTask);
            
            // then
            actualRefugeeGroupValidationException.Should().BeEquivalentTo(
                expectedRefugeeGroupValidationException);
            
            this.loggingBrokerMock.Verify( broker =>
                broker.LogError( It.Is(SameExceptionAs(
                expectedRefugeeGroupValidationException))), Times.Once);
            
            this.storageBrokerMock.Verify(broker => 
                broker.InsertRefugeeGroupAsync(nullRefugeeGroup),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
        
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfRefugeeGroupIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidRefugeeGroup = new RefugeeGroup
            {
                Name = invalidText
            };

            var invalidRefugeeGroupException =
                new InvalidRefugeeGroupException();

            invalidRefugeeGroupException.AddData(
                key: nameof(RefugeeGroup.Id),
                values: "Id is required");

            invalidRefugeeGroupException.AddData(
                key: nameof(RefugeeGroup.Name),
                values: "Text is required");

            invalidRefugeeGroupException.AddData(
                key: nameof(RefugeeGroup.RefugeeGroupMainRepresentativeId),
                values: "Id is required");

            invalidRefugeeGroupException.AddData(
                key: nameof(RefugeeGroup.CreatedByUserId),
                values: "Id is required");

            invalidRefugeeGroupException.AddData(
                key: nameof(RefugeeGroup.UpdatedByUserId),
                values: "Id is required");
            
            invalidRefugeeGroupException.AddData(
                key: nameof(RefugeeGroup.CreatedDate),
                values: "Date is required");
            
            invalidRefugeeGroupException.AddData(
                key: nameof(RefugeeGroup.UpdatedDate),
                values: "Date is required");
            
            var expectedRefugeeGroupValidationException =
                new RefugeeGroupValidationException(invalidRefugeeGroupException);
            
            //when
            ValueTask<RefugeeGroup> addRefugeeGroupTask =
                this.refugeeGroupService.AddRefugeeGroupAsync(invalidRefugeeGroup);
            
            RefugeeGroupValidationException actualRefugeeGroupValidationException =
                await Assert.ThrowsAsync<RefugeeGroupValidationException>(
                    addRefugeeGroupTask.AsTask);
            
            // then
            actualRefugeeGroupValidationException.Should().BeEquivalentTo(
                expectedRefugeeGroupValidationException);
            
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRefugeeGroupValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRefugeeGroupAsync(It.IsAny<RefugeeGroup>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            RefugeeGroup randomRefugeeGroup = CreateRandomRefugeeGroup(dateTime);
            RefugeeGroup invalidRefugeeGroup = randomRefugeeGroup;
            int randomNumber = GetRandomNumber();
            
            invalidRefugeeGroup.UpdatedDate =
                invalidRefugeeGroup.CreatedDate.AddDays(randomNumber);

            var invalidRefugeeGroupException =
                new InvalidRefugeeGroupException();

            invalidRefugeeGroupException.AddData(
                key: nameof(RefugeeGroup.UpdatedDate),
                values: $"Date is not the same as {nameof(RefugeeGroup.CreatedDate)}");
            
            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(dateTime);

            var expectedRefugeeGroupValidationException =
                new RefugeeGroupValidationException(invalidRefugeeGroupException);

            // when
            ValueTask<RefugeeGroup> addRefugeeGroupTask =
                this.refugeeGroupService.AddRefugeeGroupAsync(invalidRefugeeGroup);
            
            RefugeeGroupValidationException actualRefugeeGroupValidationException =
                await Assert.ThrowsAsync<RefugeeGroupValidationException>(
                    addRefugeeGroupTask.AsTask);
            
            // then
            actualRefugeeGroupValidationException.Should().BeEquivalentTo(
                expectedRefugeeGroupValidationException);
            
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRefugeeGroupValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRefugeeGroupAsync(It.IsAny<RefugeeGroup>()),
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
            DateTimeOffset randomDateTime =
                GetRandomDateTimeOffset();

            DateTimeOffset invalidDateTime =
                randomDateTime.AddMinutes(minutesBeforeOrAfter);

            RefugeeGroup randomRefugeeGroup = CreateRandomRefugeeGroup(invalidDateTime);
            RefugeeGroup invalidRefugeeGroup = randomRefugeeGroup;

            var invalidRefugeeGroupException =
                new InvalidRefugeeGroupException();

            invalidRefugeeGroupException.AddData(
                key: nameof(RefugeeGroup.CreatedDate),
                values: "Date is not recent");

            var expectedRefugeeGroupValidationException =
                new RefugeeGroupValidationException(invalidRefugeeGroupException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            //when
            ValueTask<RefugeeGroup> addRefugeeGroupTask =
                this.refugeeGroupService.AddRefugeeGroupAsync(invalidRefugeeGroup);
            
            RefugeeGroupValidationException actualRefugeeGroupValidationException =
                await Assert.ThrowsAsync<RefugeeGroupValidationException>(
                    addRefugeeGroupTask.AsTask);
            
            // then
            actualRefugeeGroupValidationException.Should().BeEquivalentTo(
                expectedRefugeeGroupValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRefugeeGroupValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRefugeeGroupAsync(It.IsAny<RefugeeGroup>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}