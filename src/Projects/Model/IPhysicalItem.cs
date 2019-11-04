using System.Threading.Tasks;

namespace Jpp.Common.Backend.Projects.Model
{
    public interface IPhysicalItem
    {
        bool Unexpected { get;  }
        
        bool Exists { get; set; }

        Task Create(IStorageProvider storage);

        ItemModel LogicalItem { get; set; }
    }
}
