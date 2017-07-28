using System;

namespace Pinger
{
    /// <summary>
    /// Represents the result of a single ping
    /// NOTE: All accessors are public for easy serialization of this object; play nice
    /// </summary>
    public sealed class PingResult
    {
        /// <summary>
        /// Gets or sets the host of the ping
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// Gets or sets the IP address of the host
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// Gets or sets the timeout in milliseconds
        /// </summary>
        public int Timeout { get; set; }
        /// <summary>
        /// Gets or sets the number of bytes sent
        /// </summary>
        public int Bytes { get; set; }
        /// <summary>
        /// Gets or sets the time at which the ping was sent
        /// </summary>
        public DateTimeOffset Sent { get; set; }
        /// <summary>
        /// Gets the number of milliseconds elapsed during the round trip time or zero if it failed
        /// </summary>
        public long Milliseconds { get; set; }
        /// <summary>
        /// Gets or sets the error message if the ping failed
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
