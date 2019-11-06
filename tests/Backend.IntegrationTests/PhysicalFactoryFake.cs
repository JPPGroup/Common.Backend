using Jpp.Common.Backend.Projects;
using Jpp.Common.Backend.Projects.Model;

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
