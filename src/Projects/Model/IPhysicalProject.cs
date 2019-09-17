using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Jpp.Common.Backend.Projects.Model
{
    public interface IPhysicalProject : INotifyPropertyChanged
    {
        bool FolderExists { get; }
        string RootPath { get; set; }
        ProjectModel LogicalProject { get; set; }

        Task BuildItemTree();
        Task CreateFolders();
        Task OpenFolders();

        IPhysicalItem ConvertLogicalItemToPhysical(ItemModel item);
    }
}
