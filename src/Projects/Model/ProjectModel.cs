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

        [JsonIgnore]
        public IPhysicalProject PhysicalProject { get; set; }

        public List<ItemModel> Children { get; private set; } = new List<ItemModel>();

        private bool _favourite;
        
        public void BuildChildTree(ItemModel parent = null)
        {
            if (parent == null)
            {
                Children.Clear();
            }
            else
            {
                parent.Children.Clear();
            }

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
                    itemModel.Parent = parent;
                }

                itemModel.PhysicalItem = PhysicalProject?.ConvertLogicalItemToPhysical(itemModel);

                BuildChildTree(itemModel);
            }
        }

        public ItemModel GetItemByPath(string fullpath)
        {
            string root = Name + "\\";
            string rootPath;
            if (fullpath.IndexOf(root) != -1)
            {
                rootPath = fullpath.Remove(0, fullpath.IndexOf(root) + root.Length);
            }
            else
            {
                rootPath = fullpath;
            }

            string[] Path = rootPath.Split('\\');

            ItemModel foundItem = null;

            for (int i = 0; i < Path.Length; i++)
            {
                try
                {
                    //TODO: Error handling
                    if (foundItem == null)
                    {
                        foundItem = Children.First(item => item.Name.Equals(Path[i]));
                    }
                    else
                    {
                        foundItem = foundItem.Children.First(item =>
                        {
                            bool physicalNameMatch = false;

                            if (item.PhysicalName != null)
                                physicalNameMatch = item.PhysicalName.Equals(Path[i]);

                            bool logicalNameMatch = item.Name.Equals(Path[i]);
                            return physicalNameMatch || logicalNameMatch;
                        });
                    }
                }
                catch (InvalidOperationException e)
                {
                    return  null;
                }
            }

            return foundItem;
        }
    }
}
