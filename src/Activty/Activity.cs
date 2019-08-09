using System;
using System.Collections.Generic;
using System.Text;

namespace Jpp.Common.Backend.Activty
{
    public class Activity
    {
        public string UserId { get; set; }
        public string Entity { get; set; }
        public Guid? EntityId { get; set; }
        public string Body { get; set; }
        public DateTime DateTime { get; set; }
    }
}
