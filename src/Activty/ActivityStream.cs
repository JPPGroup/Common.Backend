using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Jpp.Common.Backend.Activty
{
    public class ActivityStream : IDisposable
    {
        public ReadOnlyObservableCollection<Activity> Activities { get; }
        private ObservableCollection<Activity> _activities;

        internal ActivityStream()
        {
            Activities = new ReadOnlyObservableCollection<Activity>(_activities);
            Task.Run(() => {
                Initialize()
                });
        }

        public void Dispose()
        {


            //GC.SuppressFinalize(this);
        }

        private async void Initialize()
        {

        }
    }
}
