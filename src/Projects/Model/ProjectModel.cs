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
        
        //TODO: Refactor into a root and child method
        public void BuildChildTree(ItemModel parent = null)
        {
            List<ItemModel> container;
            if (parent == null)
            {
                container = Children;
            }
            else
            {
                container = parent.Children;
            }
            container.Clear();

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
                ProcessItemModel(parent, container, itemModel);
            }
        }

        private void ProcessItemModel(ItemModel parent, List<ItemModel> container, ItemModel itemModel)
        {
            if (parent != null)
            {
                itemModel.Parent = parent;
            }
            
            container.Add(itemModel);
            itemModel.PhysicalItem = PhysicalProject?.ConvertLogicalItemToPhysical(itemModel);
            BuildChildTree(itemModel);
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

            ItemModel foundItem = ParsePathSegments(rootPath.Split('\\'));

            return foundItem;
        }

        private ItemModel ParsePathSegments(string[] Path)
        {
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
                    return null;
                }
            }

            return foundItem;
        }
    }
}
