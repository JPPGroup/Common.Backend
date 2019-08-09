using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Jpp.Common.Backend.Projects
{
    public class ProjectModel : ProjectFolder
    {
        public Dictionary<string, string> BaseFolders { get; set; }

        public Guid Id { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }
        public string Office { get; set; }
        public string Lead { get; set; }
        public string Discipline { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Grouping { get; set; }
        public string Status { get; set; }

        [JsonProperty("isFavourite")]
        public bool Favourite
        {
            get { return _favourite; }
            set { SetField(ref _favourite, value, nameof(Favourite)); }
        }

        private bool _favourite;

        //Needs custom implementation to handle lazy loading
        public override string Path
        {
            get { return GetRootPath(); }
        }

        public ProjectModel() : base()
        {
            BaseFolders = new Dictionary<string, string>();
            //BaseFolders.Add("Northampton", @"N:\Consulting\Projects");
            BaseFolders.Add("Milton Keynes", @"M:\Consulting\Projects");
            //BaseFolders.Add("Northampton", @"D:\Consulting\Projects");
        }

        private string GetRootPath()
        {
            string root;
            if (Office != null && BaseFolders.ContainsKey(Office))
            {
                root = BaseFolders[Office];
            }
            else
            {
                root = BaseFolders["Milton Keynes"];
            }

            return $"{root}\\{Grouping}\\{Code} - {Name}";
        }

        //TODO: This should be in the API
        public void CreateDefaultFolderStructure()
        {
            Children = new ObservableCollection<ProjectItem>();

            Children.Add(new ProjectFolder()
            {
                Name = "Correspondence",
                Children = new ObservableCollection<ProjectItem>()
            {
                    new ProjectFolder()
                    {
                        Name = "Incoming"
                    },
                    new ProjectFolder()
                    {
                        Name = "JPP"
                    }
                }
            });
            Children.Add(new ProjectFolder()
            {
                Name = "Design"
            });
            Children.Add(new ProjectFolder()
            {
                Name = "Document Control"
            });
            Children.Add(new ProjectFolder()
            {
                Name = "Drawings"
            });
            Children.Add(new ProjectFolder()
            {
                Name = "Financial",
                Children = new ObservableCollection<ProjectItem>()
            {
                    new ProjectFolder()
                    {
                        Name = "Invoices"
                    },
                    new ProjectFolder()
                    {
                        Name = "Purchase Order"
                    },
                    new ProjectFolder()
                    {
                        Name = "Quotes"
                    },
                }
            });
            Children.Add(new ProjectFolder()
            {
                Name = "Meetings"
            });
            Children.Add(new ProjectFolder()
            {
                Name = "Photographs"
            });
            Children.Add(new ProjectFolder()
            {
                Name = "Reports"
            });
            Children.Add(new ProjectFolder()
            {
                Name = "Risk Assessments"
            });
            Children.Add(new ProjectFolder()
            {
                Name = "Specifications"
            });

            foreach (ProjectItem projectItem in Children)
            {
                projectItem.SetParent(this);
            }
        }
    }
}
