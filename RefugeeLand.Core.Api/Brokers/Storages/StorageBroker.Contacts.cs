using Microsoft.EntityFrameworkCore;
using RefugeeLand.Core.Api.Models.ContactInformations;

namespace RefugeeLand.Core.Api.Brokers.Storages;

public partial class StorageBroker
{
    public DbSet<ContactInformations> Contacts { get; set; }
}