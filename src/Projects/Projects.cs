using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jpp.Common.Backend.Auth;
using Newtonsoft.Json;

namespace Jpp.Common.Backend.Projects
{
    public class Projects : IProjects
    {
        public static readonly string PROJECT_ENDPOINT = $"http://{Backend.BASE_URL}/api/projects";
        private static readonly string CACHE_LOCATION = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\JPP Consulting\\Common\\ProjectCache.json";
        
        private const int REFRESH_MINUTES = 5;

        private static DateTime lastRefreshTime;

        public static int BufferCount { get; set; } = 250;
        public static int BufferTime { get; set; } = 5; //Seconds

        private IAuthentication _auth;
        private IStorageProvider _storage;

        public Projects(IAuthentication auth, IStorageProvider storage)
        {
            _auth = auth;
            _storage = storage;
        }

        public IEnumerable<DocumentTypes> GetDocumentTypes()
        {
            return new List<DocumentTypes>()
            {
                new DocumentTypes()
                {
                    Name = "Fees and Task Schedule",
                    Class = typeof(FeeTaskSchedule)
                }/*,
                new DocumentTypes()
                {
                    Name = "Fees and Task Schedule (Domestic)",
                    Class = typeof(FeeTaskSchedule)
                },
                new DocumentTypes()
                {
                    Name = "Report",
                    Class = null
                }*/
            };
        }

        private static ObservableCollection<ProjectModel> _projectModels;

        public async Task<IEnumerable<ProjectModel>> GetAllProjects()
        {
            if (_projectModels == null)
            {
                _projectModels = new ObservableCollection<ProjectModel>();
            }

            try
            {
                var task = await _auth.GetAuthenticatedClient().GetAsync(PROJECT_ENDPOINT);
                var jsonString = await task.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<IEnumerable<ProjectModel>>(jsonString);
                _projectModels = new ObservableCollection<ProjectModel>(result);
                foreach (ProjectModel pm in _projectModels)
                {
                    pm.PropertyChanged += ProjectModelChanged;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return _projectModels;
        }

        private async void ProjectModelChanged(object sender, PropertyChangedEventArgs e)
        {
            ProjectModel pm = sender as ProjectModel;
            if(e.PropertyName.Equals(nameof(ProjectModel.Favourite)))
            {
                var task = await _auth.GetAuthenticatedClient().PutAsync(PROJECT_ENDPOINT + $"/{pm.Id}/Favourite", null);
            }
        }

        private static void ProcessProjects(IEnumerable<ProjectModel> projects)
        {
            List<ProjectModel> buffer = new List<ProjectModel>();
            DateTime lastAdded = DateTime.Now;;

            foreach (ProjectModel pm in projects)
            {
                if (_projectModels.Count(x => x.Id.Equals(pm.Id)) > 0)
                {
                    ProcessExistingProjectModel(pm);
                }
                else
                {
                    _projectModels.Add(pm);
                    if (pm.Children == null || pm.Children.Count == 0)
                    {
                        pm.CreateDefaultFolderStructure();
                    }
                }

                foreach (ProjectItem projectItem in pm.Children)
                {
                    projectItem.SetParent(pm);
                }
            }
        }

        private static void ProcessExistingProjectModel(ProjectModel pm)
        {
            ProjectModel existing = _projectModels.First(x => x.Id == pm.Id);
            existing.Description = pm.Description;
            existing.Discipline = pm.Discipline;
            existing.Code = pm.Code;
            existing.Grouping = pm.Grouping;
            existing.Latitude = pm.Latitude;
            existing.Longitude = pm.Longitude;
            existing.Lead = pm.Lead;
            existing.Office = pm.Office;

            if (pm.Children != null && pm.Children.Count != 0)
            {
                existing.Children = pm.Children;
            }
        }

        private async Task<ObservableCollection<ProjectModel>> LoadCache()
        {
            try
            {
                using (Stream inputStream = await _storage.OpenSharedForRead("ProjectCache.json"))
                {
                    if (inputStream != null)
                    {
                        TextReader tr = new StreamReader(inputStream);
                        string data = tr.ReadToEnd();
                        JsonSerializerSettings settings = new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        };
                        var collection = JsonConvert.DeserializeObject<ObservableCollection<ProjectModel>>(data, settings);
                        return collection;
                    }
                }

                return new ObservableCollection<ProjectModel>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new ObservableCollection<ProjectModel>();
            }
            
        }

        internal async void SaveCache()
        {
            using (Stream inputStream = await _storage.OpenSharedForWrite("ProjectCache.json"))
            {
                if (inputStream != null)
                {
                    StreamWriter tr = new StreamWriter(inputStream);
                    tr.AutoFlush = true;
                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    };
                    string data = JsonConvert.SerializeObject(_projectModels, settings);
                    tr.Write(data);
                    inputStream.Flush();
                }
            }
        }
    }
}
