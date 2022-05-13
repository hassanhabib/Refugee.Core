using Microsoft.EntityFrameworkCore;
using RefugeeLand.Core.Api.Models.ContactInformations;

namespace RefugeeLand.Core.Api.Brokers.Storages;

public partial interface IStorageBroker
{
    public DbSet<ContactInformations> Contacts { get; set; }
}