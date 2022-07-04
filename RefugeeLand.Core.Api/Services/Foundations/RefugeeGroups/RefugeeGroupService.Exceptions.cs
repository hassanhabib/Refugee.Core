// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using RefugeeLand.Core.Api.Models.RefugeeGroups;
using RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions;
using Xeptions;

namespace RefugeeLand.Core.Api.Services.Foundations.RefugeeGroups
{
    public partial class RefugeeGroupService
    {
        private delegate ValueTask<RefugeeGroup> ReturningRefugeeGroupFunction();
        
        private async ValueTask<RefugeeGroup> TryCatch(ReturningRefugeeGroupFunction returningRefugeeGroupFunction)
        {
            try
            {
                return await returningRefugeeGroupFunction();
            }
            catch (NullRefugeeGroupException nullRefugeeGroupException)
            {
                throw CreateAndLogValidationException(nullRefugeeGroupException);
            }
            catch (SqlException sqlException)
            {
                var failedRefugeeGroupStorageException =
                    new FailedRefugeeGroupStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedRefugeeGroupStorageException);
            }
            catch (InvalidRefugeeGroupException invalidRefugeeGroupException)
            {
                throw CreateAndLogValidationException(invalidRefugeeGroupException);
            }
        }

        private RefugeeGroupDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var refugeeGroupDependencyException =
                new RefugeeGroupDependencyException(exception);

            this.loggingBroker.LogCritical(refugeeGroupDependencyException);

            return refugeeGroupDependencyException;
        }

        private RefugeeGroupValidationException CreateAndLogValidationException(Xeption exception)
        {
            var refugeeGroupValidationException = new RefugeeGroupValidationException(exception);
            this.loggingBroker.LogError(refugeeGroupValidationException);

            return refugeeGroupValidationException;
        }
    }
}