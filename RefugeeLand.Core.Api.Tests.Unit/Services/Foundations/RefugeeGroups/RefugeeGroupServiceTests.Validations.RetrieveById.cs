// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidRefugeeGroupId = Guid.Empty;

            var invalidRefugeeGroupException =
                new InvalidRefugeeGroupException();

            invalidRefugeeGroupException.AddData(
                key: nameof(RefugeeGroup.Id),
                values: "Id is required");

            var expectedRefugeeGroupValidationException =
                new RefugeeGroupValidationException(invalidRefugeeGroupException);

            // when
            ValueTask<RefugeeGroup> retrieveRefugeeGroupByIdTask =
                this.refugeeGroupService.RetrieveRefugeeGroupByIdAsync(invalidRefugeeGroupId);

            RefugeeGroupValidationException actualRefugeeGroupValidationException =
                await Assert.ThrowsAsync<RefugeeGroupValidationException>(
                    retrieveRefugeeGroupByIdTask.AsTask);

            // then
            actualRefugeeGroupValidationException.Should()
                .BeEquivalentTo(expectedRefugeeGroupValidationException);

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogError(It.Is(SameExceptionAs(
                        expectedRefugeeGroupValidationException))),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                    broker.SelectRefugeeGroupByIdAsync(It.IsAny<Guid>()),
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfRefugeeGroupIsNotFoundAndLogItAsync()
        {
            //given
            Guid someRefugeeGroupId = Guid.NewGuid();
            RefugeeGroup noRefugeeGroup = null;

            var notFoundRefugeeGroupException =
                new NotFoundRefugeeGroupException(someRefugeeGroupId);

            var expectedRefugeeGroupValidationException =
                new RefugeeGroupValidationException(notFoundRefugeeGroupException);

            this.storageBrokerMock.Setup(broker =>
                    broker.SelectRefugeeGroupByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(noRefugeeGroup);

            //when
            ValueTask<RefugeeGroup> retrieveRefugeeGroupByIdTask =
                this.refugeeGroupService.RetrieveRefugeeGroupByIdAsync(someRefugeeGroupId);

            RefugeeGroupValidationException actualRefugeeGroupValidationException =
                await Assert.ThrowsAsync<RefugeeGroupValidationException>(
                    retrieveRefugeeGroupByIdTask.AsTask);

            //then
            actualRefugeeGroupValidationException.Should().BeEquivalentTo(expectedRefugeeGroupValidationException);

            this.storageBrokerMock.Verify(broker =>
                    broker.SelectRefugeeGroupByIdAsync(It.IsAny<Guid>()),
                Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogError(It.Is(SameExceptionAs(
                        expectedRefugeeGroupValidationException))),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}