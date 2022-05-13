using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RefugeeLand.Core.Api.Models.ContactInformations;

namespace RefugeeLand.Core.Api.Brokers.Storages;

public partial interface IStorageBroker
{
    public DbSet<RefugeeContact> Contacts { get; set; }

    ValueTask<RefugeeContact> InsertContactAsync(RefugeeContact refugeeContact);

    IQueryable<RefugeeContact> SelectAllRefugeeContacts();
    ValueTask<RefugeeContact> SelectRefugeeContactByIdAsync(Guid contactId);
    ValueTask<RefugeeContact> UpdateRefugeeContactAsync(RefugeeContact contact);
    ValueTask<RefugeeContact> DeleteRefugeeContactAsync(RefugeeContact contact);
}