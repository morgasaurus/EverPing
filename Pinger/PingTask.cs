using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Pinger
{
    /// <summary>
    /// Shortcut class for performing network Ping operation tasks
    /// </summary>
    public static class PingTask
    {
        /// <summary>
        /// Synchronously pings the specified host using the specified timeout in milliseconds and number of bytes
        /// </summary>
        /// <param name="host">The host</param>
        /// <param name="timeout">The timeout in milliseconds</param>
        /// <param name="bytes">The bytes</param>
        /// <returns>A ping result object containing the results</returns>
        public static PingResult Sync(string host, int timeout, int bytes)
        {
            PingResult result = new PingResult() { Host = host, Timeout = timeout, Bytes = bytes };

            // Initialize packet
            byte[] packet = new byte[bytes];
            for (int i = 0; i < packet.Length; i++)
            {
                packet[i] = 1;
            }

            // Send ping and catch network error then dispose the Ping object
            Ping pinger = new Ping();
            try
            {
                result.Sent = DateTimeOffset.Now;
                PingReply reply = pinger.Send(host, timeout, packet);
                result.IpAddress = reply.Address != null ? reply.Address.ToString() : string.Empty;
                if (reply.Status == IPStatus.Success)
                {
                    result.ErrorMessage = string.Empty;
                    result.Milliseconds = reply.RoundtripTime;
                }
                else
                {
                    result.ErrorMessage = reply.Status.ToString();
                    result.Milliseconds = 0;
                }
            }
            catch (Exception ex)
            {
                result.IpAddress = string.Empty;
                result.Milliseconds = 0;
                result.ErrorMessage = ex.GetBaseException().Message;
            }
            finally
            {
                pinger.Dispose();
            }

            return result;
        }

        /// <summary>
        /// Starts and returns the handle to a new task that pings the specified host using the specified timeout in milliseconds and number of bytes
        /// </summary>
        /// <param name="host">The host</param>
        /// <param name="timeout">The timeout in milliseconds</param>
        /// <param name="bytes">The bytes</param>
        /// <returns>The handle to a task which will return the ping result object</returns>
        public static Task<PingResult> New(string host, int timeout, int bytes)
        {
            return Task.Run(() =>
            {
                return Sync(host, timeout, bytes);
            });          
        }        
    }
}
