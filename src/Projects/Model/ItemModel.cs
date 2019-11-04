using System;
using System.Collections.Generic;
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

        //TODO: Review constructors
        internal ItemModel(string name, string physicalName, ItemModel parent, string type, bool localOnly)
        {
            if (parent == null)
                throw new ArgumentNullException();

            Name = name;
            PhysicalName = physicalName;
            ProjectId = parent.ProjectId;
            ParentItemId = parent.Id;
            Type = type;
            LocalOnly = localOnly;
        }

        //TODO: Review constructors
        public ItemModel(string name, string physicalName, ItemModel parent, string type)
        {
            if(parent == null)
                throw new ArgumentNullException();

            Name = name;
            PhysicalName = physicalName;
            ProjectId = parent.ProjectId;
            ParentItemId = parent.Id;
            Type = type;
            LocalOnly = true;
        }

        //TODO: Review constructors
        public ItemModel(string name, string physicalName, ProjectModel parent, string type)
        {
            if (parent == null)
                throw new ArgumentNullException();

            Name = name;
            PhysicalName = physicalName;
            ProjectId = parent.Id;
            Type = type;
            LocalOnly = true;            
        }
    }
}
