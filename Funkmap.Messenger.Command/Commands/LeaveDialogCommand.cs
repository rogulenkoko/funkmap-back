
namespace Funkmap.Messenger.Command.Commands
{
    public class LeaveDialogCommand
    {
        public LeaveDialogCommand(string dialogId, string leavedUserLogin, string userLogin)
        {
            DialogId = dialogId;
            LeavedUserLogin = leavedUserLogin;
            UserLogin = userLogin;
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
    }
}
