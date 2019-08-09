using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Jpp.Common.Backend.Projects;
using Jpp.Common.Backend.Auth;
using Newtonsoft.Json;

namespace Jpp.Common.Backend.Activty
{
    public class ActivityService : IActivityService
    {
        public static readonly string ACTIVITIES_ENDPOINT = $"http://{Backend.BASE_URL}/api/activities";
        private IAuthentication _auth;

        private ObservableCollection<Activity> _activities;
        public object ActivityLock { get; set; }

        public ActivityService(IAuthentication auth)
        {
            _auth = auth;
            BindingOperations.CollectionRegistering += BindingOperations_CollectionRegistering;
        }

        public MainViewModel()
        {
            Stocks = new ObservableCollection<Stock>();
            

            //BindingOperations.EnableCollectionSynchronization(Stocks, _stocksLock);
        }

        void BindingOperations_CollectionRegistering(object sender, CollectionRegisteringEventArgs e)
        {
            Debug.WriteLine("CollectionRegistering Event");
            if (e.Collection == Stocks)
            {
                BindingOperations.EnableCollectionSynchronization(Stocks, _stocksLock);
            }
        }


        public async Task<ObservableCollection<Activity>> GetAllActivities()
        {
            try
            {
                var task = await _auth.GetAuthenticatedClient().GetAsync(ACTIVITIES_ENDPOINT + $"?Page=1&Size=50");
                var jsonString = await task.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<IEnumerable<Activity>>(jsonString);

                ProcessActivities(result);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task PushActivity(Activity activity)
        {
            try
            {
                string json = JsonConvert.SerializeObject(activity);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var task = await _auth.GetAuthenticatedClient().PostAsync(ACTIVITIES_ENDPOINT, httpContent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void ProcessActivities()
        {

        }
    }
}
