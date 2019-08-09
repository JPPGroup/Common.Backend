using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jpp.Common.Backend.Projects
{
    public interface IProjects
    {
        IEnumerable<DocumentTypes> GetDocumentTypes();

        Task<IEnumerable<ProjectModel>> GetAllProjects();
    }
}
