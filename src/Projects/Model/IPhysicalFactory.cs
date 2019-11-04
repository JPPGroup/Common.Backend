namespace Jpp.Common.Backend.Projects.Model
{
    public interface IPhysicalFactory
    {
        IPhysicalProject CreateProject(ProjectModel projectModel, IProjectService projects);
    }
}
