using System;

namespace EverPingUi
{
    [Serializable]
    public sealed class Settings
    {
        public string Host { get; set; }
        public int Timeout { get; set; }
        public int Bytes { get; set; }
    }
}
