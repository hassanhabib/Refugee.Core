// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.Refugees.Exceptions
{
    public class FailedRefugeeServiceException : Xeption
    {
        public FailedRefugeeServiceException(Exception innerException)
            : base(message: "Refugee service error occurred, contact support", innerException)
        { }
    }
}
