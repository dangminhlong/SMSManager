using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSManager
{
    public abstract class BaseMessage
    {
        public string Date { get; set; }

        public string Id { get; set; }

        public string Message { get; set; }

        public string Number { get; set; }


    }
}
