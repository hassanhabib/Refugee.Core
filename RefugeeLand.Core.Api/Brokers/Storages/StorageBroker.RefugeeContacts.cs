using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RefugeeLand.Core.Api.Models.ContactInformations;

namespace RefugeeLand.Core.Api.Brokers.Storages;

public partial class StorageBroker
{
    public DbSet<RefugeeContact> Contacts { get; set; }

    public ValueTask<RefugeeContact> InsertContactAsync(RefugeeContact refugeeContact)
    {
        throw new NotImplementedException();
    }

    public IQueryable<RefugeeContact> SelectAllRefugeeContacts()
    {
        throw new NotImplementedException();
    }

    public ValueTask<RefugeeContact> SelectRefugeeContactByIdAsync(Guid contactId)
    {
        throw new NotImplementedException();
    }

    public ValueTask<RefugeeContact> UpdateRefugeeContactAsync(RefugeeContact contact)
    {
        throw new NotImplementedException();
    }

    public ValueTask<RefugeeContact> DeleteRefugeeContactAsync(RefugeeContact contact)
    {
        throw new NotImplementedException();
    }
}