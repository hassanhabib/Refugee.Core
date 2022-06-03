// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Xeptions;

namespace RefugeeLand.Core.Api.Models.Refugees.Exceptions
{
    public class NullRefugeeException : Xeption
    {
        public NullRefugeeException()
            : base(message: "Refugee is null")
        { }
    }
}
