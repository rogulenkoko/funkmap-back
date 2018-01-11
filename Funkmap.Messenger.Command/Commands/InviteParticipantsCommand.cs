using System.Collections.Generic;

namespace Funkmap.Messenger.Command.Commands
{
    public class InviteParticipantsCommand
    {
        public InviteParticipantsCommand(string userLogin, ICollection<string> invitedUsers, string dialogId)
        {
            UserLogin = userLogin;
            InvitedUsers = invitedUsers;
            DialogId = dialogId;
        }

        public string UserLogin { get; }
        public ICollection<string> InvitedUsers { get; }

        public string DialogId { get; }
    }
}
