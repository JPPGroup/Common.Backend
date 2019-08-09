using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Jpp.Common;

namespace Jpp.Common.Backend.Projects
{
    public class ProjectFolder : ProjectItem
    {
        public ObservableCollection<ProjectItem> Children { get; set; }

        public ProjectFolder() : base()
        {
            Children = new ObservableCollection<ProjectItem>();
        }

        public ProjectItem GetChildByPath(string s)
        {
            var found = Children.FirstOrDefault(x => x.Path.EndsWith(s));
            if (found == null)
            {
                foreach (ProjectItem projectItem in Children)
                {
                    if (projectItem is ProjectFolder)
                    {
                        var childFound = ((ProjectFolder) projectItem).GetChildByPath(s);
                        if (childFound != null)
                        {
                            return childFound;
                        }
                    }
                }
            }

            return found;
        }

        public override void SetParent(ProjectItem item)
        {
            base.SetParent(item);

            foreach (ProjectItem projectItem in Children)
            {
                projectItem.SetParent(this);
            }
        }

        public bool AcceptsFileType(string path)
        {
            //TODO: Add file type checking
            string extension = System.IO.Path.GetExtension(path);

            switch (extension)
            {
                case ".txt":
                case ".dwg":
                    return true;

                default:
                    return false;
            }
        }

        public async void AddFile(string path, IStorageProvider storage)
        {
            await storage.CopyFile(path, this.Path);
            this.Add(new ProjectDocument()
            {
                Name = System.IO.Path.GetFileNameWithoutExtension(path),
                PhysicalName = System.IO.Path.GetFileName(path)
            });
        }

        public void Add(ProjectItem child)
        {
            child.Parent = this;
            Children.Add(child);
        }
    }
}
