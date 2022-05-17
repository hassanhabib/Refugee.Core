// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using RefugeeLand.Core.Api.Models.Refugees;
using RefugeeLand.Core.Api.Models.Refugees.Exceptions;

namespace RefugeeLand.Core.Api.Services.Foundations.Refugees
{
    public partial class RefugeeService
    {
        private void ValidateRefugeeOnAdd(Refugee refugee)
        {
            ValidateRefugeeIsNotNull(refugee);
        }

        private static void ValidateRefugeeIsNotNull(Refugee refugee)
        {
            if (refugee is null)
            {
                throw new NullRefugeeException();
            }
        }
    }
}
