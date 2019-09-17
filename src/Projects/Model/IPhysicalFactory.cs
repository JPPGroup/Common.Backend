using System;
using System.Collections.Generic;
using System.Text;

namespace Jpp.Common.Backend.Projects.Model
{
    public interface IPhysicalFactory
    {
        IPhysicalProject CreateProject(ProjectModel projectModel, IProjectService _projects);
    }
}
