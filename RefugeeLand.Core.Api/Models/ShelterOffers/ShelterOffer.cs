// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using RefugeeLand.Core.Api.Models.Shelters;
using RefugeeLand.Core.Api.Models.Hosts;
using RefugeeLand.Core.Api.Models.ShelterRequests;

namespace RefugeeLand.Core.Api.Models.ShelterOffers
{
    public class ShelterOffer
    {
        public Guid Id { get; set; }
        
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public ShelterOfferStatus Status { get; set; }

        public Guid ShelterId { get; set; }
        public Shelter Shelter { get; set; }

        public Guid HostId { get; set; }
        public Host Host { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid UpdatedByUserId { get; set; }

        [JsonIgnore]
        public IEnumerable<ShelterRequest> ShelterRequests { get; set; }
    }
}