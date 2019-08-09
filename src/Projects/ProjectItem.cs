using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Jpp.Common;
using Newtonsoft.Json;

namespace Jpp.Common.Backend.Projects
{
    public abstract class ProjectItem : BaseNotify
    {
        [JsonIgnore]
        public ProjectItem Parent { get; set; }

        public string Name { get; set; }

        public bool Present
        {
            get { return _present; }
            set { SetField(ref _present, value, nameof(Present)); }
        }

        private bool _present = false;

        public bool Unexpected
        {
            get { return _unexpected; }
            set { SetField(ref _unexpected, value, nameof(Unexpected)); }
        }

        private bool _unexpected = false;

        public string PhysicalName
        {
            get
            {
                if (string.IsNullOrEmpty(_physicalName))
                {
                    return Name;
                }
                else
                {
                    return _physicalName;
                }
            }
            set { _physicalName = value; }
        }

        public virtual string Path
        {
            get
            {
                if (string.IsNullOrEmpty(_path))
                {
                    return $"{Parent.Path}\\{PhysicalName}";
                }
                else
                {
                    return _path;
                }
            }
            set { _path = value; }
        }

        public virtual void SetParent(ProjectItem item)
        {
            Parent = item;
        }

        public ProjectModel GetProjectModel()
        {
            if (this is ProjectModel)
            {
                return this as ProjectModel;
            }
            else
            {
                if(Parent == null)
                    throw new ArgumentOutOfRangeException("Root project item reached without finding model.");
                return this.Parent.GetProjectModel();
            }
        }

        private string _path;
        private string _physicalName;
    }
}
