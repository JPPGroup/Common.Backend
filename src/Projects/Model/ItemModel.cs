using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Office2013.PowerPoint;
using Newtonsoft.Json;

namespace Jpp.Common.Backend.Projects.Model
{
    public class ItemModel
    {
        [JsonProperty]
        public Guid Id { get; private set; }
        [JsonProperty]
        public string Name { get; private set; }
        [JsonProperty]
        public string PhysicalName { get; private set; }
        [JsonProperty]
        public Guid ProjectId { get; private set; }
        [JsonProperty]
        public Guid? ParentItemId { get; private set; }
        [JsonProperty]
        public string Type { get; private set; } = "folder";

        [JsonIgnore]
        public List<ItemModel> Children { get; set; } = new List<ItemModel>();

        [JsonIgnore]
        public bool LocalOnly { get; set; } = false;

        [JsonIgnore]
        public IPhysicalItem PhysicalItem { get; set; }

        [JsonIgnore]
        public ItemModel Parent { get; set; }

        [JsonConstructor]
        private ItemModel()
        { }

        internal ItemModel(string name, string physicalname, ItemModel parent, string type, bool localOnly)
        {
            Name = name;
            PhysicalName = physicalname;
            ProjectId = parent.ProjectId;
            ParentItemId = parent.Id;
            Type = type;
            LocalOnly = localOnly;
        }

        public ItemModel(string name, string physicalname, ItemModel parent, string type)
        {
            if(parent == null)
                throw new ArgumentNullException();

            Name = name;
            PhysicalName = physicalname;
            ProjectId = parent.ProjectId;
            ParentItemId = parent.Id;
            Type = type;
            LocalOnly = true;
        }

        public ItemModel(string name, string physicalname, ProjectModel parent, string type)
        {
            if (parent == null)
                throw new ArgumentNullException();

            Name = name;
            PhysicalName = physicalname;
            ProjectId = parent.Id;
            Type = type;
            LocalOnly = true;            
        }
    }
}
