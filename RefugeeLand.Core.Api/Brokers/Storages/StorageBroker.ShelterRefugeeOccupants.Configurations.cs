using Microsoft.EntityFrameworkCore;
using RefugeeLand.Core.Api.Models.ShelterRefugeeOccupants;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void AddShelterRefugeeOccupantConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShelterRefugeeOccupant>()
                .HasOne(shelterRefugeeOccupant => shelterRefugeeOccupant.Shelter)
                .WithMany(shelter => shelter.ShelterRefugeeOccupants)
                .HasForeignKey(shelterRefugeeOccupant => shelterRefugeeOccupant.ShelterId)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<ShelterRefugeeOccupant>()
                .HasOne(shelterRefugeeOccupant => shelterRefugeeOccupant.Refugee)
                .WithMany(refugee => refugee.ShelterRefugeeOccupants)
                .HasForeignKey(shelterRefugeeOccupant => shelterRefugeeOccupant.RefugeeId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
