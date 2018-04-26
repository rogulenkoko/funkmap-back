namespace Funkmap.Messenger.Contracts.Events.Dialogs
{
    public class UserLeavedDialogEvent
    {
        public UserLeavedDialogEvent(string dialogId, string leavedUserLogin, string userLogin, string dialogCreatorLogin)
        {
            DialogId = dialogId;
            LeavedUserLogin = leavedUserLogin;
            UserLogin = userLogin;
            DialogCreatorLogin = dialogCreatorLogin;
        }


        public string DialogId { get; }

        /// <summary>
        /// User who should leave
        /// </summary>
        public string LeavedUserLogin { get; }


        /// <summary>
        /// User who executed a command
        /// </summary>
        public string UserLogin { get; }

        public string DialogCreatorLogin { get; }
    }
}
