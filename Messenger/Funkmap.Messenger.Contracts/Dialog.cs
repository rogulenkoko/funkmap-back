using System;
using System.Collections.Generic;
using Funkmap.Messenger.Entities;

namespace Funkmap.Messenger.Contracts
{
    public class Dialog
    {
        public string Id { get; set; }

        public string Name { get; set; }
        
        public ImageInfo Avatar { get; set; }
        
        public string AvatarId { get; set; }
        
        public List<string> Participants { get; set; }
        
        public DateTime LastMessageDate { get; set; }
        
        public Message LastMessage { get; set; }
        
        public string CreatorLogin { get; set; }
        
        public DialogType DialogType { get; set; }
    }
}
