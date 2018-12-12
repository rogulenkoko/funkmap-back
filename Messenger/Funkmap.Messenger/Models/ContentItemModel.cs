using System.ComponentModel.DataAnnotations;
using Funkmap.Messenger.Entities;

namespace Funkmap.Messenger.Models
{
    public class ContentItemModel
    {
        [Required]
        public ContentType ContentType { get; set; }

        [Required]
        public string Name { get; set; }

        public double Size { get; set; }

        [Required]
        public byte[] Data { get; set; }

        public string DataUrl { get; set; }
    }
}
