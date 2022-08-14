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
        public async Task ShouldRemoveNationalityByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputNationalityId = randomId;
            Nationality randomNationality = CreateRandomNationality();
            Nationality storageNationality = randomNationality;
            Nationality expectedInputNationality = storageNationality;
            Nationality deletedNationality = expectedInputNationality;
            Nationality expectedNationality = deletedNationality.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectNationalityByIdAsync(inputNationalityId))
                    .ReturnsAsync(storageNationality);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteNationalityAsync(expectedInputNationality))
                    .ReturnsAsync(deletedNationality);

            // when
            Nationality actualNationality = await this.nationalityService
                .RemoveNationalityByIdAsync(inputNationalityId);

            // then
            actualNationality.Should().BeEquivalentTo(expectedNationality);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectNationalityByIdAsync(inputNationalityId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteNationalityAsync(expectedInputNationality),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}