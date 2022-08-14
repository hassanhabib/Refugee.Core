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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidNationalityId = Guid.Empty;

            var invalidNationalityException =
                new InvalidNationalityException();

            invalidNationalityException.AddData(
                key: nameof(Nationality.Id),
                values: "Id is required");

            var expectedNationalityValidationException =
                new NationalityValidationException(invalidNationalityException);

            // when
            ValueTask<Nationality> retrieveNationalityByIdTask =
                this.nationalityService.RetrieveNationalityByIdAsync(invalidNationalityId);

            NationalityValidationException actualNationalityValidationException =
                await Assert.ThrowsAsync<NationalityValidationException>(
                    retrieveNationalityByIdTask.AsTask);

            // then
            actualNationalityValidationException.Should()
                .BeEquivalentTo(expectedNationalityValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedNationalityValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectNationalityByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfNationalityIsNotFoundAndLogItAsync()
        {
            //given
            Guid someNationalityId = Guid.NewGuid();
            Nationality noNationality = null;

            var notFoundNationalityException =
                new NotFoundNationalityException(someNationalityId);

            var expectedNationalityValidationException =
                new NationalityValidationException(notFoundNationalityException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectNationalityByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noNationality);

            //when
            ValueTask<Nationality> retrieveNationalityByIdTask =
                this.nationalityService.RetrieveNationalityByIdAsync(someNationalityId);

            NationalityValidationException actualNationalityValidationException =
                await Assert.ThrowsAsync<NationalityValidationException>(
                    retrieveNationalityByIdTask.AsTask);

            //then
            actualNationalityValidationException.Should().BeEquivalentTo(expectedNationalityValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectNationalityByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedNationalityValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}