using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
