using AchillesAPI.Contexts;
using System;
using System.Linq;

namespace AchillesAPI.Helpers
{
    /// <summary>
    /// A collection of classes designed to make the operation of controllers easier.
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGenerated]
    internal class NamespaceDoc
    {

    }

    /// <summary>
    /// A simple helper class designed to make handling Authentication easier.
    /// </summary>
    public class AuthenticationHelper
    {
        /// <summary>
        /// Verifies the session.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="bool"/> indicating if a given sessionId is valid or not.</returns>
        public bool VerifySession(Guid sessionId, ApplicationDbContext context)
        {
            try
            {
                var userSession = context.UserSessions.ToList().FirstOrDefault(x => x.SessionId == sessionId);
                var isNotExpired = DateTime.Now < userSession.ExpiresWhen;
                if (!isNotExpired)
                {
                    context.UserSessions.Remove(userSession);
                    context.SaveChanges();
                }
                return isNotExpired;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Derives the user identifier from session identifier.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="context">The context.</param>
        /// <returns>The <see cref="Guid"/> of a given user, based off their sessionId</returns>
        public Guid DerriveUserIdFromSessionId(Guid sessionId, ApplicationDbContext context)
        {
            try
            {
                return context.UserSessions.FirstOrDefault(x => x.SessionId == sessionId).UserId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
