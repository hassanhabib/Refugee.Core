using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using RefugeeLand.Core.Api.Models.Nationalities;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.Nationalities
{
    public partial class NationalityServiceTests
    {
        [Fact]
        public async Task ShouldAddNationalityAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            Nationality randomNationality = CreateRandomNationality(randomDateTimeOffset);
            Nationality inputNationality = randomNationality;
            Nationality storageNationality = inputNationality;
            Nationality expectedNationality = storageNationality.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertNationalityAsync(inputNationality))
                    .ReturnsAsync(storageNationality);

            // when
            Nationality actualNationality = await this.nationalityService
                .AddNationalityAsync(inputNationality);

            // then
            actualNationality.Should().BeEquivalentTo(expectedNationality);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertNationalityAsync(inputNationality),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}