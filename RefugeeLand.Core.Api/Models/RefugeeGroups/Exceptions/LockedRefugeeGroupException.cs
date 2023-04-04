// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions
{
    public class LockedRefugeeGroupException : Xeption
    {
        public LockedRefugeeGroupException(Exception innerException)
            : base(message: "Locked RefugeeGroup record exception, please try again later", innerException)
        {
        }
    }
}