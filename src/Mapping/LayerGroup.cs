using System;
using System.Collections.Generic;
using System.Text;

namespace Jpp.Common.Backend.Mapping
{
    public class LayerGroup
    {
        public string Name { get; set; }
        public IEnumerable<Layer> Layers { get; set; }
    }
}
