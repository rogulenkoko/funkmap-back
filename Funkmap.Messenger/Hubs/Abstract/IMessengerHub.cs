using System.Threading.Tasks;
using Funkmap.Messenger.Models;

namespace Funkmap.Messenger.Hubs.Abstract
{
    public interface IMessengerHub
    {
        /// <summary>
        /// Triggers when any user connects to Hub.
        /// </summary>
        /// <param name="userLogin">User's login.</param>
        /// <returns></returns>
        Task OnUserConnected(string userLogin);

        /// <summary>
        /// Triggers when any user disconnects from Hub.
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        Task OnUserDisconnected(string userLogin);

        /// <summary>
        /// Triggers when message was recieved (user who has sent message also will get it).
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task OnMessageSent(MessageModel message);

        /// <summary>
        /// Triggers when dialog was created (available for a dialog creator because dialog is empty during the creation).
        /// </summary>
        /// <param name="dialog"></param>
        /// <returns></returns>
        Task OnDialogCreated(DialogModel dialog);

        /// <summary>
        /// Triggers when dialog was updated (created for reciever, updated avatar, name etc.).
        /// </summary>
        /// <param name="dialog"></param>
        /// <returns></returns>
        Task OnDialogUpdated(DialogModel dialog);

        /// <summary>
        /// Trigger when user opens dialog and it means user has read new messages.
        /// </summary>
        /// <param name="dialogRead"></param>
        /// <returns></returns>
        Task OnDialogRead(DialogReadModel dialogRead);

        /// <summary>
        /// Triggers when one item of content has been loaded to file storage.
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        Task OnContentLoaded(ContentItemModel itemModel);

        /// <summary>
        /// Treiggers when error appers in command processing.
        /// </summary>
        /// <param name="errorModel"></param>
        /// <returns></returns>
        Task OnError(CommandErrorModel errorModel);
    }
}
