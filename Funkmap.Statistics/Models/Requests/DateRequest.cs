using System;

namespace Funkmap.Statistics.Models.Requests
{
    public class DateRequest
    {
        /// <summary>
        /// Begin of period
        /// </summary>
        public DateTime Begin { get; set; }

        /// <summary>
        /// End of period
        /// </summary>
        public DateTime End { get; set; }
    }
}
