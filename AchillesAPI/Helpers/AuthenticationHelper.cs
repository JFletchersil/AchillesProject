using AchillesAPI.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Helpers
{
    public class AuthenticationHelper
    {
        public bool VerifySession(Guid sessionId, ApplicationDbContext context)
        {
            var expireDate = context.UserSessions.FirstOrDefault(x => x.SessionId == sessionId).ExpiresWhen;
            return expireDate < DateTime.Now;
        }
    }
}
