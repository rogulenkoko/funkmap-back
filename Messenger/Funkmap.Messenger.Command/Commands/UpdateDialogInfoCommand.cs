
namespace Funkmap.Messenger.Command.Commands
{
    public class UpdateDialogInfoCommand
    {
        public string DialogId { get; set; }
        public string Name { get; set; }
        public byte[] Avatar { get; set; }

        public string UserLogin { get; set; }
    }
}
