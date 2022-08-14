// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.Hosts;

namespace RefugeeLand.Core.Api.Services.Foundations.Hosts
{
    public interface IHostService
    {
        ValueTask<Host> AddHostAsync(Host host);
        IQueryable<Host> RetrieveAllHosts();
        ValueTask<Host> RetrieveHostByIdAsync(Guid hostId);
        ValueTask<Host> ModifyHostAsync(Host host);
        ValueTask<Host> RemoveHostByIdAsync(Guid hostId);
    }
}