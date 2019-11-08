using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Jpp.Common.Backend.Activty
{
    public interface IActivityService
    {
        Task<ActivityStream> GetAllActivities();

        Task<ActivityStream> GetActivitiesForEntity(Guid entityId);

        Task PushActivity(Activity activity);
    }
}
