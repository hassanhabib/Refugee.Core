// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using RefugeeLand.Core.Api.Models.RefugeeGroups;
using RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.RefugeeGroups
{
    public partial class RefugeeGroupServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfRefugeeGroupIsNullAndLogItAsync()
        {
            // given
            RefugeeGroup nullRefugeeGroup = null;
            var nullRefugeeGroupException = new NullRefugeeGroupException();

            var expectedRefugeeGroupValidationException =
                new RefugeeGroupValidationException(nullRefugeeGroupException);

            // when
            ValueTask<RefugeeGroup> modifyRefugeeGroupTask =
                this.refugeeGroupService.ModifyRefugeeGroupAsync(nullRefugeeGroup);

            RefugeeGroupValidationException actualRefugeeGroupValidationException =
                await Assert.ThrowsAsync<RefugeeGroupValidationException>(
                    modifyRefugeeGroupTask.AsTask);

            // then
            actualRefugeeGroupValidationException.Should()
                .BeEquivalentTo(expectedRefugeeGroupValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRefugeeGroupValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRefugeeGroupAsync(It.IsAny<RefugeeGroup>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfRefugeeGroupIsInvalidAndLogItAsync(string invalidText)
        {
            //given
            var invalidRefugeeGroup = new RefugeeGroup
            {
                Name = invalidText,
            };

            var invalidRefugeeGroupException = new InvalidRefugeeGroupException();

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
                key: nameof(RefugeeGroup.CreatedDate),
                values: "Date is required");

            invalidRefugeeGroupException.AddData(
                key: nameof(RefugeeGroup.CreatedByUserId),
                values: "Id is required");

            invalidRefugeeGroupException.AddData(
                key: nameof(RefugeeGroup.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(RefugeeGroup.CreatedDate)}"
                });

            invalidRefugeeGroupException.AddData(
                key: nameof(RefugeeGroup.UpdatedByUserId),
                values: "Id is required");

            var expectedRefugeeGroupValidationException =
                new RefugeeGroupValidationException(invalidRefugeeGroupException);

            // when
            ValueTask<RefugeeGroup> modifyRefugeeGroupTask =
                this.refugeeGroupService.ModifyRefugeeGroupAsync(invalidRefugeeGroup);

            RefugeeGroupValidationException actualRefugeeGroupValidationException =
                await Assert.ThrowsAsync<RefugeeGroupValidationException>(
                    modifyRefugeeGroupTask.AsTask);

            //then
            actualRefugeeGroupValidationException.Should()
                .BeEquivalentTo(expectedRefugeeGroupValidationException);
            
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRefugeeGroupValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRefugeeGroupAsync(It.IsAny<RefugeeGroup>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
        
        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            RefugeeGroup randomRefugeeGroup = CreateRandomRefugeeGroup(randomDateTimeOffset);
            randomRefugeeGroup.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidRefugeeGroupException =
                new InvalidRefugeeGroupException();

            invalidRefugeeGroupException.AddData(
                key: nameof(RefugeeGroup.UpdatedDate),
                values: "Date is not recent");

            var expectedRefugeeGroupValidatonException =
                new RefugeeGroupValidationException(invalidRefugeeGroupException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<RefugeeGroup> modifyRefugeeGroupTask =
                this.refugeeGroupService.ModifyRefugeeGroupAsync(randomRefugeeGroup);

            RefugeeGroupValidationException actualRefugeeGroupValidationException =
                await Assert.ThrowsAsync<RefugeeGroupValidationException>(
                    modifyRefugeeGroupTask.AsTask);

            // then
            actualRefugeeGroupValidationException.Should().BeEquivalentTo(expectedRefugeeGroupValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRefugeeGroupValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRefugeeGroupByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
        
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfRefugeeGroupDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            RefugeeGroup randomRefugeeGroup = CreateRandomModifyRefugeeGroup(randomDateTimeOffset);
            RefugeeGroup nonExistRefugeeGroup = randomRefugeeGroup;
            RefugeeGroup nullRefugeeGroup = null;

            var notFoundRefugeeGroupException =
                new NotFoundRefugeeGroupException(nonExistRefugeeGroup.Id);

            var expectedRefugeeGroupValidationException =
                new RefugeeGroupValidationException(notFoundRefugeeGroupException);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateRefugeeGroupAsync(nonExistRefugeeGroup))
                .ReturnsAsync(nullRefugeeGroup);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when 
            ValueTask<RefugeeGroup> modifyRefugeeGroupTask =
                this.refugeeGroupService.ModifyRefugeeGroupAsync(nonExistRefugeeGroup);

            RefugeeGroupValidationException actualRefugeeGroupValidationException =
                await Assert.ThrowsAsync<RefugeeGroupValidationException>(
                    modifyRefugeeGroupTask.AsTask);

            // then
            actualRefugeeGroupValidationException.Should()
                .BeEquivalentTo(expectedRefugeeGroupValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRefugeeGroupAsync(nonExistRefugeeGroup),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRefugeeGroupValidationException))),
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
            RefugeeGroup randomRefugeeGroup = CreateRandomModifyRefugeeGroup(randomDateTimeOffset);
            RefugeeGroup invalidRefugeeGroup = randomRefugeeGroup.DeepClone();
            RefugeeGroup storageRefugeeGroup = invalidRefugeeGroup.DeepClone();
            storageRefugeeGroup.CreatedDate = storageRefugeeGroup.CreatedDate.AddMinutes(randomMinutes);
            storageRefugeeGroup.UpdatedDate = storageRefugeeGroup.UpdatedDate.AddMinutes(randomMinutes);
            var invalidRefugeeGroupException = new InvalidRefugeeGroupException();

            invalidRefugeeGroupException.AddData(
                key: nameof(RefugeeGroup.CreatedDate),
                values: $"Date is not the same as {nameof(RefugeeGroup.CreatedDate)}");

            var expectedRefugeeGroupValidationException =
                new RefugeeGroupValidationException(invalidRefugeeGroupException);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateRefugeeGroupAsync(invalidRefugeeGroup))
                .ReturnsAsync(storageRefugeeGroup);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<RefugeeGroup> modifyRefugeeGroupTask =
                this.refugeeGroupService.ModifyRefugeeGroupAsync(invalidRefugeeGroup);

            RefugeeGroupValidationException actualRefugeeGroupValidationException =
                await Assert.ThrowsAsync<RefugeeGroupValidationException>(
                    modifyRefugeeGroupTask.AsTask);

            // then
            actualRefugeeGroupValidationException.Should()
                .BeEquivalentTo(expectedRefugeeGroupValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRefugeeGroupAsync(invalidRefugeeGroup),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedRefugeeGroupValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
        
         [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserIdDontMatchStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            RefugeeGroup randomRefugeeGroup = CreateRandomModifyRefugeeGroup(randomDateTimeOffset);
            RefugeeGroup invalidRefugeeGroup = randomRefugeeGroup.DeepClone();
            RefugeeGroup storageRefugeeGroup = invalidRefugeeGroup.DeepClone();
            invalidRefugeeGroup.CreatedByUserId = Guid.NewGuid();
            storageRefugeeGroup.UpdatedDate = storageRefugeeGroup.CreatedDate;

            var invalidRefugeeGroupException = new InvalidRefugeeGroupException();

            invalidRefugeeGroupException.AddData(
                key: nameof(RefugeeGroup.CreatedByUserId),
                values: $"Id is not the same as {nameof(RefugeeGroup.CreatedByUserId)}");

            var expectedRefugeeGroupValidationException =
                new RefugeeGroupValidationException(invalidRefugeeGroupException);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateRefugeeGroupAsync(invalidRefugeeGroup))
                .ReturnsAsync(storageRefugeeGroup);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<RefugeeGroup> modifyRefugeeGroupTask =
                this.refugeeGroupService.ModifyRefugeeGroupAsync(invalidRefugeeGroup);

            RefugeeGroupValidationException actualRefugeeGroupValidationException =
                await Assert.ThrowsAsync<RefugeeGroupValidationException>(
                    modifyRefugeeGroupTask.AsTask);

            // then
            actualRefugeeGroupValidationException.Should().BeEquivalentTo(expectedRefugeeGroupValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRefugeeGroupAsync(invalidRefugeeGroup),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedRefugeeGroupValidationException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}