using System;
using Microsoft.Owin.Security.Infrastructure;

namespace Funkmap.Module.Auth
{
    public class FunkmapRefreshTokenProvider : AuthenticationTokenProvider
    {
        public override void Create(AuthenticationTokenCreateContext context)
        {
            context.Ticket.Properties.ExpiresUtc = new DateTimeOffset(DateTime.Now.AddDays(7));
            context.SetToken(context.SerializeTicket());
        }

        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);
        }
    }
}
