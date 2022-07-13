// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.Refugees;

namespace RefugeeLand.Core.Api.Services.Foundations.Refugees
{
    public interface IRefugeeService
    {
        ValueTask<Refugee> AddRefugeeAsync(Refugee refugee);
        IQueryable<Refugee> RetrieveAllRefugees();
        ValueTask<Refugee> RetrieveRefugeeByIdAsync();
    }
}
