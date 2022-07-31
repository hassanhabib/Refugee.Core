// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using RefugeeLand.Core.Api.Models.RefugeeGroupMemberships;
using RefugeeLand.Core.Api.Models.Refugees;
using RefugeeLand.Core.Api.Models.ShelterRequests;

namespace RefugeeLand.Core.Api.Models.RefugeeGroups
{
    public class RefugeeGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        //One who speaks for the group during negotiations and requests
        public Guid RefugeeGroupMainRepresentativeId { get; set; }
        public Refugee RefugeeGroupMainRepresentative { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid UpdatedByUserId { get; set; }

        [JsonIgnore] 
        public IEnumerable<RefugeeGroupMembership> RefugeeGroupMemberships { get; set; }

        [JsonIgnore]
        public IEnumerable<ShelterRequest> ShelterRequests { get; set; }
    }
}