using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.Nationalities;

namespace RefugeeLand.Core.Api.Services.Foundations.Nationalities
{
    public interface INationalityService
    {
        ValueTask<Nationality> AddNationalityAsync(Nationality nationality);
    }
}