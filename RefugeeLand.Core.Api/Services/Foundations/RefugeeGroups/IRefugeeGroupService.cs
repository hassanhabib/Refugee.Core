// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.RefugeeGroups;

namespace RefugeeLand.Core.Api.Services.Foundations.RefugeeGroups
{
    public interface IRefugeeGroupService
    {
        ValueTask<RefugeeGroup> AddRefugeeGroupAsync(RefugeeGroup refugeeGroup);
        IQueryable<RefugeeGroup> RetrieveAllRefugeeGroups();
        ValueTask<RefugeeGroup> RetrieveRefugeeGroupByIdAsync(Guid refugeeGroupId);
    }
}