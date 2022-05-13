// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.Refugees;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Refugee> InsertRefugeeAsync(Refugee refugee);
        IQueryable<Refugee> SelectAllRefugees();
        ValueTask<Refugee> SelectRefugeeByIdAsync(Guid refugeeId);
        ValueTask<Refugee> UpdateRefugeeAsync(Refugee refugee);
        ValueTask<Refugee> DeleteRefugeeAsync(Refugee refugee);
    }
}
