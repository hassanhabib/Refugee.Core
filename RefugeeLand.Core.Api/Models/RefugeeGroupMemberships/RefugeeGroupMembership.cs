// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using RefugeeLand.Core.Api.Models.RefugeeGroups;
using RefugeeLand.Core.Api.Models.Refugees;

namespace RefugeeLand.Core.Api.Models.RefugeeGroupMemberships
{
    public class RefugeeGroupMembership
    {
        public Guid Id { get; set; }

        public Guid RefugeeGroupId { get; set; }
        public RefugeeGroup RefugeeGroup { get; set; }

        public Guid RefugeeId { get; set; }
        public Refugee Refugee { get; set; }

        //Leader or head of Refugee group
        public bool IsDecisionMaker { get; set; }

        //This is the main contact to facilitate communications with the Refugee group
        public bool IsRefugeeGroupRepresentative { get; set; }

        public RefugeeGroupMembershipStatus Status { get; set; }
        public string Details { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid UpdatedByUserId { get; set; }
    }
}