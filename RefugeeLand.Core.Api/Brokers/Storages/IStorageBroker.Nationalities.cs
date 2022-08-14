using System;
using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.Nationalities;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Nationality> InsertNationalityAsync(Nationality nationality);
        IQueryable<Nationality> SelectAllNationalities();
        ValueTask<Nationality> SelectNationalityByIdAsync(Guid nationalityId);
    }
}
