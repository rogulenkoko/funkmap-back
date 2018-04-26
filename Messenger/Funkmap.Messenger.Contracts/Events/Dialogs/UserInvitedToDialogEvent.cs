using System.Collections.Generic;
using Funkmap.Messenger.Entities;

namespace Funkmap.Messenger.Contracts.Events.Dialogs
{
    public class UserInvitedToDialogEvent
    {
        public UserInvitedToDialogEvent(Dialog dialog, string userLogin, ICollection<string> invitedParticipants)
        {
            Dialog = dialog;
            UserLogin = userLogin;
            InvitedParticipants = invitedParticipants;
        }

        public Dialog Dialog { get; }

        public string UserLogin { get; }

        public ICollection<string> InvitedParticipants { get; }

    }
}
