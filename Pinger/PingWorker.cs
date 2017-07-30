using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Pinger
{
    /// <summary>
    /// Represents a thread-safe object that will continuously perform diagnostic pings on an end-point until a stop command is issued
    /// </summary>
    public class PingWorker
    {
        /// <summary>
        /// Creates a new PingWorker instance with the specified host
        /// </summary>
        /// <param name="host">The host</param>
        public PingWorker(string host) : this(host, 3000, 32) { }
        /// <summary>
        /// Creates a new PingWorker instance with the specified host and packet size in bytes
        /// </summary>
        /// <param name="host">The host</param>
        /// <param name="bytes">The packet size in bytes</param>
        public PingWorker(string host, int bytes) : this(host, 3000, bytes) { }
        /// <summary>
        ///  Creates a new PingWorker instance with the specified host, timeout in milliseconds, and packet size in bytes
        /// </summary>
        /// <param name="host">The host</param>
        /// <param name="timeout">The timeout for each ping</param>
        /// <param name="bytes">The number of bytes to send</param>
        public PingWorker(string host, int timeout, int bytes)
        {
            Host = host;
            Timeout = timeout;
            Bytes = bytes;
            TimeBetween = 1000;
            KeepResults = false;
        }

        private const int Infinity = System.Threading.Timeout.Infinite;

        /// <summary>
        /// Occurs when a ping request is completed or results in an error
        /// </summary>
        public event EventHandler<PingCompleteEventArgs> PingComplete;

        /// <summary>
        /// Gets or sets a flag indicating whether or not to keep ALL results in memory until the worker is stopped
        /// </summary>
        public bool KeepResults
        {
            get
            {
                return _KeepResults;
            }
            set
            {
                lock(Lock)
                {
                    ThrowIfWorking();
                    _KeepResults = value;
                }
            }
        }
        private bool _KeepResults;

        /// <summary>
        /// Gets or sets the host to ping
        /// </summary>
        public string Host
        {
            get
            {
                return _Host;
            }
            set
            {
                lock (Lock)
                {
                    ThrowIfWorking();
                    _Host = value;
                }
            }
        }
        private string _Host;
        /// <summary>
        /// Gets or sets the number of bytes to send
        /// </summary>
        public int Bytes
        {
            get
            {
                return _Bytes;
            }
            set
            {
                lock (Lock)
                {
                    ThrowIfWorking();
                    _Bytes = value;
                }
            }
        }
        private int _Bytes;
        /// <summary>
        /// Gets or sets the timeout in milliseconds
        /// </summary>
        public int Timeout
        {
            get
            {
                return _Timeout;
            }
            set
            {
                lock (Lock)
                {
                    ThrowIfWorking();
                    _Timeout = value;
                }
            }
        }
        private int _Timeout;
        /// <summary>
        /// Gets or sets the time between pings in milliseconds
        /// </summary>
        public int TimeBetween
        {
            get
            {
                return _TimeBetween;
            }
            set
            {
                lock (Lock)
                {
                    ThrowIfWorking();
                    _TimeBetween = value;
                }
            }
        }
        private int _TimeBetween;
        
        /// <summary>
        /// Sync lock for thread safety
        /// </summary>
        private object Lock = new object();
        
        /// <summary>
        /// The timer which invokes the ping callback at set intervals
        /// </summary>
        private Timer PingTimer;

        /// <summary>
        /// Collection to hold all ping results if KeepResults is set to true
        /// </summary>
        private List<PingResult> Results = new List<PingResult>();

        /// <summary>
        /// Field to hold the report for this worker
        /// </summary>
        private PingReport Report = new PingReport();
        
        /// <summary>
        /// Throws an InvalidOperationException if an attempt is made to set properties while a ping test is already in progress
        /// </summary>
        private void ThrowIfWorking()
        {
            if (PingTimer != null)
            {
                throw new InvalidOperationException("Unable to set properties while PingWorker is working");
            }
        }

        /// <summary>
        /// Starts the ping worker by spawning a task to perform diagnostic pings until Stop is called;
        /// in order to access each PingResult in real time subscribe to the PingComplete event
        /// </summary>
        public void Start()
        {
            if (PingTimer != null)
            {
                return;
            }

            lock(Lock)
            {
                if (PingTimer != null)
                {
                    return;
                }

                Results.Clear();
                Report.Reset();
                PingTimer = new Timer(PingCallback, null, 100, Infinity);
            }
        }

        /// <summary>
        /// Stops the PingWorker
        /// </summary>
        public void Stop()
        {
            lock(Lock)
            {
                if (PingTimer == null)
                {
                    return;
                }

                PingTimer.Dispose();
                PingTimer = null;
            }
        }

        /// <summary>
        /// Gets the report on the diagnostic ping test
        /// </summary>
        /// <returns>The ping report</returns>
        public PingReport GetReport()
        {
            lock(Lock)
            {
                return Report.Copy();
            }
        }

        /// <summary>
        /// Gets ALL detailed ping results for each ping conducted during the last test performed;
        /// this list will be EMPTY unless you set KeepResults to true before starting
        /// </summary>
        /// <returns></returns>
        public List<PingResult> GetResults()
        {
            lock(Lock)
            {
                return Results.Select(x => x.Copy()).ToList();
            }
        }

        private void PingCallback(object state) // state not used
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            PingResult result = PingTask.Sync(Host, Timeout, Bytes);
            lock(Lock)
            {
                if (PingTimer == null)
                {
                    return;
                }
                if (KeepResults)
                {
                    Results.Add(result);
                }
                Report.AddToReport(result);
                if (PingComplete != null)
                {
                    PingComplete.Invoke(this, new PingCompleteEventArgs(result));
                }
                watch.Stop();
                int next = Math.Max(200, TimeBetween - (int)watch.ElapsedMilliseconds);
                PingTimer.Change(next, Infinity);
            }            
        }
    }

    /// <summary>
    /// Contains data for the PingComplete event
    /// </summary>
    public class PingCompleteEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new PingCompleteEventArgs using the specified ping result
        /// </summary>
        /// <param name="result">The result of the ping</param>
        public PingCompleteEventArgs(PingResult result)
        {
            Result = result;
        }

        /// <summary>
        /// Gets the result of the completed ping
        /// </summary>
        public PingResult Result { get; private set; }
    }
}
