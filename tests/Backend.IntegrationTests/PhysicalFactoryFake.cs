using Jpp.Common.Backend.Projects;
using Jpp.Common.Backend.Projects.Model;
using System;

namespace Jpp.Common.Backend.IntegrationTests
{
    class PhysicalFactoryFake : IPhysicalFactory
    {
        public IPhysicalProject CreateProject(ProjectModel projectModel, IProjectService projects)
        {
            return null;
        }
    }
}
