using System.Linq;
using FluentAssertions;
using Moq;
using RefugeeLand.Core.Api.Models.Nationalities;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.Nationalities
{
    public partial class NationalityServiceTests
    {
        [Fact]
        public void ShouldReturnNationalities()
        {
            // given
            IQueryable<Nationality> randomNationalities = CreateRandomNationalities();
            IQueryable<Nationality> storageNationalities = randomNationalities;
            IQueryable<Nationality> expectedNationalities = storageNationalities;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllNationalities())
                    .Returns(storageNationalities);

            // when
            IQueryable<Nationality> actualNationalities =
                this.nationalityService.RetrieveAllNationalities();

            // then
            actualNationalities.Should().BeEquivalentTo(expectedNationalities);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllNationalities(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}