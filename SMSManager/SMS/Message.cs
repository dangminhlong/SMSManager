using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSManager
{
    public class Message
    {
        public string date { get; set; }
        public string id { get; set; }
        public string message { get; set; }
        public string messageType { get; set; }
        public string number { get; set; }
        public string receiver { get; set; }
        public string sender { get; set; }
        public string threadId { get; set; }
        public string read { get; set; }
    }
}
