using System.Collections.Generic;
using Funkmap.Messenger.Entities;

namespace Funkmap.Messenger.Events.Dialogs
{
    public class UserInvitedToDialogEvent
    {
        public UserInvitedToDialogEvent(DialogEntity dialog, string userLogin, ICollection<string> invitedParticipants)
        {
            Dialog = dialog;
            UserLogin = userLogin;
            InvitedParticipants = invitedParticipants;
        }

        public DialogEntity Dialog { get; }

        public string UserLogin { get; }

        public ICollection<string> InvitedParticipants { get; }

    }
}
