using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Jpp.Common.Backend.Projects.Model;

namespace Jpp.Common.Backend.Projects
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectModel>> GetAllProjects();

        Task<ProjectModel> GetProject(Guid id);

        Task<ItemModel> CreateItem(string name, string physicalname, ItemModel parent, string type);

        Task DeleteItem(Guid id);
    }
}
