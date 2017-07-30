using System;

namespace Pinger
{
    /// <summary>
    /// Represents a report that records data for repeated diagnostic pings over a time period
    /// NOTE: All accessors are public for easy serialization of this object; play nice
    /// </summary>
    public class PingReport
    {
        /// <summary>
        /// Creates a new PingReport instance
        /// </summary>
        public PingReport()
        {
            Reset();
        }

        /// <summary>
        /// Represents the response time in milliseconds that is considered moderately high for gaming
        /// </summary>
        public const int CodeOrange = 500;
        /// <summary>
        /// Represents the response time in milliseconds that is considered excessively high for gaming
        /// </summary>
        public const int CodeRed = 1000;
        
        /// <summary>
        /// Gets or sets the host that was pinged
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the start time of the diagnostic pings
        /// </summary>
        public DateTimeOffset Start { get; set; }
        /// <summary>
        /// Gets or sets the time the diagnostic ping test ended
        /// </summary>
        public DateTimeOffset End { get; set; }
        /// <summary>
        /// Gets or sets the duration of the diagnostic ping test
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the number of diagnostic pings sent
        /// </summary>
        public int Sent { get; set; }
        /// <summary>
        /// Gets or sets the number of diagnostic pings received
        /// </summary>
        public int Received { get; set; }
        /// <summary>
        /// Gets or sets the number of diagnostic pings lost
        /// </summary>
        public int Lost { get; set; }
        /// <summary>
        /// Gets or sets the percentage of pings lost relative to pings sent
        /// </summary>
        public decimal PacketLoss { get; set; }

        /// <summary>
        /// Gets or sets the lowest ping response time during the test in milliseconds
        /// </summary>
        public int Min { get; set; }
        /// <summary>
        /// Gets or sets the highest ping respose time during the test in milliseconds
        /// </summary>
        public int Max { get; set; }
        /// <summary>
        /// Gets or sets the average ping response time in milliseconds taken over the duration of the test
        /// </summary>
        public decimal Average { get; set; }

        /// <summary>
        /// Gets or sets the number of ping responses that are considered moderate or excessively high for gaming
        /// </summary>
        public int HighPingEvents { get; set; }
        /// <summary>
        /// Gets or sets the number of high ping response times per hour during the test
        /// </summary>
        public decimal HighPingEventsPerHour { get; set; }
        /// <summary>
        /// Gets or sets the number of potential disconnects (at least two consecutive dropped packets) during the test
        /// </summary>
        public int PotentialDisconnects { get; set; }
        /// <summary>
        /// Gets or sets the number of potential disconnects per hour during the test
        /// </summary>
        public decimal PotentialDisconnectsPerHour { get; set; }

        /// <summary>
        /// Flag to track whether we are currently dropping packets
        /// </summary>
        private bool Dropping;
        /// <summary>
        /// Flag to track whether we have recorded the potential disconnect event
        /// </summary>
        private bool DisconnectEventRecorded;

        /// <summary>
        /// Lock for thread safety
        /// </summary>
        private object Lock = new object();

        /// <summary>
        /// Updates the current diagnostic ping test report using the specified PingResult
        /// </summary>
        /// <param name="result">The ping result</param>
        public void AddToReport(PingResult result)
        {
            lock(Lock)
            {
                // Set the host and start time on the initial ping
                if (string.IsNullOrEmpty(Host))
                {
                    Host = result.Host;
                }
                if (Start == default(DateTimeOffset))
                {
                    Start = result.Sent;
                }

                Sent++;

                if (string.IsNullOrEmpty(result.ErrorMessage))
                {
                    Min = Math.Min(Min, (int)result.Milliseconds);
                    Max = Math.Max(Max, (int)result.Milliseconds);
                    Average = (Average * Received + (decimal)result.Milliseconds) / (Received + 1);

                    if (result.Milliseconds >= CodeOrange)
                    {
                        HighPingEvents++;
                    }

                    Received++;
                    Dropping = false;
                    DisconnectEventRecorded = false;
                }
                else
                {
                    // Record potential disconnects only once and only on consecutively dropped packets
                    if (Dropping == true && DisconnectEventRecorded == false)
                    {
                        PotentialDisconnects++;
                        DisconnectEventRecorded = true;
                    }
                    Dropping = true;
                }

                // Do calculations
                Lost = Sent - Received;
                PacketLoss = (decimal)Lost / (decimal)Sent;

                End = DateTimeOffset.Now;
                Duration = End - Start;
                decimal hours = (decimal)Duration.TotalHours;
                HighPingEventsPerHour = HighPingEvents / hours;
                PotentialDisconnectsPerHour = PotentialDisconnects / hours;
            }
        }

        /// <summary>
        /// Resets this PingReport instance
        /// </summary>
        public void Reset()
        {
            Host = string.Empty;

            Start = default(DateTimeOffset);
            End = default(DateTimeOffset);
            Duration = default(TimeSpan);

            Sent = 0;
            Received = 0;
            Lost = 0;
            PacketLoss = 0m;

            Min = int.MaxValue;
            Max = 0;
            Average = 0m;

            HighPingEvents = 0;
            HighPingEventsPerHour = 0;
            PotentialDisconnects = 0;
            PotentialDisconnectsPerHour = 0;

            Dropping = false;
            DisconnectEventRecorded = false;
        }

        public PingReport Copy()
        {
            lock(Lock)
            {
                PingReport report = new PingReport();

                report.Host = Host;

                report.Start = Start;
                report.End = End;
                report.Duration = Duration;

                report.Sent = Sent;
                report.Received = Received;
                report.Lost = Lost;
                report.PacketLoss = PacketLoss;

                report.Min = Min;
                report.Max = Max;
                report.Average = Average;

                report.HighPingEvents = HighPingEvents;
                report.HighPingEventsPerHour = HighPingEventsPerHour;
                report.PotentialDisconnects = PotentialDisconnects;
                report.PotentialDisconnectsPerHour = PotentialDisconnectsPerHour;

                report.Dropping = Dropping;
                report.DisconnectEventRecorded = DisconnectEventRecorded;

                return report;
            }
        }
    }
}
