using System;
using Funkmap.Domain.Enums;

namespace Funkmap.Domain.Models
{
    public class VideoInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public VideoType Type { get; set; }
        public DateTime SaveDateUtc { get; set; }


        public override bool Equals(object obj)
        {
            var video = obj as VideoInfo;
            return video?.Id == Id && video?.Type == Type;
        }
    }
}
