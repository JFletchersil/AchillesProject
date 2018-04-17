using AchillesAPI.Contexts;
using System;
using System.Linq;

namespace AchillesAPI.Helpers
{
    public class AuthenticationHelper
    {
        public bool VerifySession(Guid sessionId, ApplicationDbContext context)
        {
            var userSession = context.UserSessions.ToList().FirstOrDefault(x => x.SessionId == sessionId);
            var isNotExpired =  DateTime.Now < userSession.ExpiresWhen;
            if (!isNotExpired)
            {
                context.UserSessions.Remove(userSession);
                context.SaveChanges();
            }
            return isNotExpired;
        }
    }
}
