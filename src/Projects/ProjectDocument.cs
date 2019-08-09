using System;
using System.Collections.Generic;
using System.Text;

namespace Jpp.Common.Backend.Projects
{
    public class ProjectDocument : ProjectItem
    {
        public string CurrentRevision { get; set; } = "0";
    }
}
