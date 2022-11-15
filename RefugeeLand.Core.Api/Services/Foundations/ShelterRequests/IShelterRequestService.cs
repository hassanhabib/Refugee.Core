using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.ShelterRequests;

namespace RefugeeLand.Core.Api.Services.Foundations.ShelterRequests
{
    public interface IShelterRequestService
    {
        ValueTask<ShelterRequest> AddShelterRequestAsync(ShelterRequest shelterRequest);
        IQueryable<ShelterRequest> RetrieveAllShelterRequests();
    }
}