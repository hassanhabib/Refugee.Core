using System;
using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.Nationalities;

namespace RefugeeLand.Core.Api.Services.Foundations.Nationalities
{
    public interface INationalityService
    {
        ValueTask<Nationality> AddNationalityAsync(Nationality nationality);
        IQueryable<Nationality> RetrieveAllNationalities();
        ValueTask<Nationality> RetrieveNationalityByIdAsync(Guid nationalityId);
    }
}