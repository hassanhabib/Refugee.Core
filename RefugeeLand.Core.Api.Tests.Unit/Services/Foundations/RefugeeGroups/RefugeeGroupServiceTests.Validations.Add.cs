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

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}