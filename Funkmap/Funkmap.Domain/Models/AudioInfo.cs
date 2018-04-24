using System;

namespace Funkmap.Domain.Models
{
    public class AudioInfo
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public override bool Equals(object obj)
        {
            var audio = obj as AudioInfo;
            return audio?.Id == Id;
        }
    }
}
