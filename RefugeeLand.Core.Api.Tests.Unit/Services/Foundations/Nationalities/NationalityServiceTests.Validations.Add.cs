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
        public async Task ShouldThrowValidationExceptionOnAddIfNationalityIsNullAndLogItAsync()
        {
            // given
            Nationality nullNationality = null;

            var nullNationalityException =
                new NullNationalityException();

            var expectedNationalityValidationException =
                new NationalityValidationException(nullNationalityException);

            // when
            ValueTask<Nationality> addNationalityTask =
                this.nationalityService.AddNationalityAsync(nullNationality);

            NationalityValidationException actualNationalityValidationException =
                await Assert.ThrowsAsync<NationalityValidationException>(
                    addNationalityTask.AsTask);

            // then
            actualNationalityValidationException.Should().BeEquivalentTo(expectedNationalityValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedNationalityValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}