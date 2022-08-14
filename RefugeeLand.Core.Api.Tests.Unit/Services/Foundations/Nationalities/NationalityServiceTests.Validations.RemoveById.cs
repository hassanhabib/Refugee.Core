// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidNationalityId = Guid.Empty;

            var invalidNationalityException =
                new InvalidNationalityException();

            invalidNationalityException.AddData(
                key: nameof(Nationality.Id),
                values: "Id is required");

            var expectedNationalityValidationException =
                new NationalityValidationException(invalidNationalityException);

            // when
            ValueTask<Nationality> removeNationalityByIdTask =
                this.nationalityService.RemoveNationalityByIdAsync(invalidNationalityId);

            NationalityValidationException actualNationalityValidationException =
                await Assert.ThrowsAsync<NationalityValidationException>(
                    removeNationalityByIdTask.AsTask);

            // then
            actualNationalityValidationException.Should()
                .BeEquivalentTo(expectedNationalityValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedNationalityValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteNationalityAsync(It.IsAny<Nationality>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}