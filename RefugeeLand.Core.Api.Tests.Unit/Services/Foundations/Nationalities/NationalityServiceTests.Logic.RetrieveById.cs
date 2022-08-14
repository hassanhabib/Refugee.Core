// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

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
        public async Task ShouldRetrieveNationalityByIdAsync()
        {
            // given
            Nationality randomNationality = CreateRandomNationality();
            Nationality inputNationality = randomNationality;
            Nationality storageNationality = randomNationality;
            Nationality expectedNationality = storageNationality.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectNationalityByIdAsync(inputNationality.Id))
                    .ReturnsAsync(storageNationality);

            // when
            Nationality actualNationality =
                await this.nationalityService.RetrieveNationalityByIdAsync(inputNationality.Id);

            // then
            actualNationality.Should().BeEquivalentTo(expectedNationality);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectNationalityByIdAsync(inputNationality.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}