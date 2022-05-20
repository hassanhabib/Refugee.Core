// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Xeptions;

namespace RefugeeLand.Core.Api.Models.Refugees.Exceptions
{
    public class InvalidRefugeeException : Xeption
    {
        public InvalidRefugeeException() 
            : base(message: "Refugee is invalid")
            { }
    }
}
