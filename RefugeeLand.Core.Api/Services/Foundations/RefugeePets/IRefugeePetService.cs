using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.RefugeePets;

namespace RefugeeLand.Core.Api.Services.Foundations.RefugeePets
{
    public interface IRefugeePetService
    {
        ValueTask<RefugeePet> AddRefugeePetAsync(RefugeePet refugeePet);
    }
}