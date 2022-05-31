// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Xeptions;

namespace RefugeeLand.Core.Api.Models.Refugees.Exceptions;

public class RefugeeDependencyValidationException : Xeption
{
    public RefugeeDependencyValidationException(Xeption innerException)
    : base(message: "Refugee dependency validation error occurred, try again", innerException)
    { }
}