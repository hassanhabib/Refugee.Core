// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;

namespace RefugeeLand.Core.Api.Models.ShelterRequests
{
    public class ShelterRequest
    {
        public Guid Id { get; set; }

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public ShelterRequestStatus Status { get; set; }

        public Guid ShelterOfferId { get; set; }
        //    public ShelterOffer ShelterOffer { get; set; }

        public Guid RefugeeGroupId { get; set; }
        //    public RefugeeGroup RefugeeGroup { get; set; }

        public Guid RefugeeApplicantId { get; set; }
        //   public Refugee RefugeeApplicant { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid UpdatedByUserId { get; set; }
    }
}