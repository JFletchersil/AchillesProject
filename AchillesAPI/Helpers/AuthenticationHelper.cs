using AchillesAPI.Contexts;
using System;
using System.Linq;

namespace AchillesAPI.Helpers
{
    public class AuthenticationHelper
    {
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
