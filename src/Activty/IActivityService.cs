using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Jpp.Common.Backend.Activty
{
    public interface IActivityService
    {
        Task<ObservableCollection<Activity>> GetAllActivities();

        Task PushActivity(Activity activity);
    }
}
