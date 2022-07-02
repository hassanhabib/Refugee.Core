// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using RefugeeLand.Core.Api.Models.RefugeeGroups;
using RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions;

namespace RefugeeLand.Core.Api.Services.Foundations.RefugeeGroups
{
    public partial class RefugeeGroupService
    {
         private static void ValidateRefugeeGroup(RefugeeGroup refugeeGroup)
         {
             ValidateRefugeeGroupIsNotNull(refugeeGroup);
         }

         private static void ValidateRefugeeGroupIsNotNull(RefugeeGroup refugeeGroup)
         {
             if(refugeeGroup is null)
             {
                 throw new NullRefugeeGroupException();
             }
         }
    }
}