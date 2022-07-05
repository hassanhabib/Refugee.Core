// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RefugeeLand.Core.Api.Models.RefugeeGroups;
using RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions;
using RefugeeLand.Core.Api.Models.RefugeePets;
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
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsRefugeeGroupException =
                    new AlreadyExistsRefugeeGroupException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsRefugeeGroupException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedRefugeeGroupStorageException =
                    new FailedRefugeeGroupStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedRefugeeGroupStorageException);
            }
            catch(ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidRefugeeGroupReferenceException =
                    new InvalidRefugeeGroupReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidRefugeeGroupReferenceException);
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
        
        private RefugeeGroupDependencyValidationException  CreateAndLogDependencyValidationException(Xeption exception)
        {
            var refugeeGroupDependencyValidationException = 
                new RefugeeGroupDependencyValidationException(exception);

            this.loggingBroker.LogError(refugeeGroupDependencyValidationException);

            return refugeeGroupDependencyValidationException;
        }
        
        private RefugeeGroupDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var refugeeGroupDependencyException = 
                new RefugeeGroupDependencyException(exception);

            this.loggingBroker.LogError(refugeeGroupDependencyException);

            return refugeeGroupDependencyException;
        }

    }
}