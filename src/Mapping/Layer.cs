using System;
using System.Collections.Generic;
using System.Text;
using Jpp.Common;

namespace Jpp.Common.Backend.Mapping
{
    public class Layer : BaseNotify
    {
        public string Name { get; set; }

        public bool Enabled
        {
            get { return _enabled; }
            set { SetField(ref _enabled, value, nameof(Enabled)); }
        }

        private bool _enabled;
        public string URL { get; set; }

        public double Opacity { get; set; } = 1;
    }
}
