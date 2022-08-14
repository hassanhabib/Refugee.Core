// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

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
        ValueTask<Nationality> UpdateNationalityAsync(Nationality nationality);
        ValueTask<Nationality> DeleteNationalityAsync(Nationality nationality);
    }
}
