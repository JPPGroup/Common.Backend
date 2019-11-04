using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jpp.Common.Backend.Projects.Model;

namespace Jpp.Common.Backend.Projects
{
    public interface IProjectService
    {
        /// <summary>
        /// Get a collection of all projects
        /// </summary>
        /// <returns>Enumerable collection of projects</returns>
        Task<IEnumerable<ProjectModel>> GetAllProjects();

        /// <summary>
        /// Get specific project
        /// </summary>
        /// <param name="id">ID of project</param>
        /// <returns>Project requested</returns>
        Task<ProjectModel> GetProject(Guid id);

        /// <summary>
        /// Create a new item that is tracked by the backend
        /// </summary>
        /// <param name="name">Name of item</param>
        /// <param name="physicalname">Physical name of item</param>
        /// <param name="parent">Parent item</param>
        /// <param name="type">Item type</param>
        /// <returns></returns>
        Task<ItemModel> CreateItem(string name, string physicalname, ItemModel parent, string type);

        /// <summary>
        /// Delete specified project
        /// </summary>
        /// <param name="id">Id of project</param>
        /// <returns>Awaitable task</returns>
        Task DeleteItem(Guid id);
    }
}
