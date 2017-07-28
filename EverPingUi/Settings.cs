using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
