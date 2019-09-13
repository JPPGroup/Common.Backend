using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Jpp.Common.Backend.Projects.Model
{
    public class ProjectModel : BaseNotify
    {
        public Dictionary<string, string> BaseFolders { get; set; }

        [JsonProperty]
        public Guid Id { get; private set; }
        [JsonProperty]
        public string Name { get; private set; }

        [JsonProperty]
        public string Code { get; private set; }
        [JsonProperty]
        public string Description { get; private set; }
        [JsonProperty]
        public string Office { get; private set; }
        [JsonProperty]
        public string Lead { get; private set; }
        [JsonProperty]
        public string Discipline { get; private set; }
        [JsonProperty]
        public double Longitude { get; private set; }
        [JsonProperty]
        public double Latitude { get; private set; }
        [JsonProperty]
        public string Grouping { get; private set; }
        [JsonProperty]
        public string Status { get; private set; }

        [JsonProperty]
        public bool IsTemplateCurrent { get; private set; }

        [JsonProperty("isFavourite")]
        public bool Favourite
        {
            get { return _favourite; }
            set { SetField(ref _favourite, value, nameof(Favourite)); }
        }

        [JsonProperty]
        IEnumerable<ItemModel> Items { get; set; }

        public List<ItemModel> Children { get; private set; } = new List<ItemModel>();

        private bool _favourite;

        //Needs custom implementation to handle lazy loading
        public string Path
        {
            get { return GetRootPath(); }
        }

        public ProjectModel() : base()
        {
            BaseFolders = new Dictionary<string, string>();
            //BaseFolders.Add("Northampton", @"N:\Consulting\ProjectService");
            BaseFolders.Add("Milton Keynes", @"M:\Consulting\ProjectService");
            //BaseFolders.Add("Northampton", @"D:\Consulting\ProjectService");
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

        public void BuildChildTree(ItemModel parent = null)
        {
            var roots = Items.Where(i =>
            {
                if (parent != null)
                {
                    if (!i.ParentItemId.HasValue)
                        return false;

                    return i.ParentItemId == parent.Id;
                }else
                {
                    return !i.ParentItemId.HasValue;
                }
            });

            foreach (ItemModel itemModel in roots)
            {
                if (parent == null)
                {
                    Children.Add(itemModel);
                }
                else
                {
                    parent.Children.Add(itemModel);
                }

                BuildChildTree(itemModel);
            }
        }
    }
}
