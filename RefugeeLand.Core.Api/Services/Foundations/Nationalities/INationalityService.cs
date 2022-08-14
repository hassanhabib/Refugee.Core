// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

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
        ValueTask<Nationality> ModifyNationalityAsync(Nationality nationality);
        ValueTask<Nationality> RemoveNationalityByIdAsync(Guid nationalityId);
    }
}