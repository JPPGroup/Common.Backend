using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Jpp.Common.Backend.Auth;
using Jpp.Common.Backend.Projects.Model;
using Newtonsoft.Json;

namespace Jpp.Common.Backend.Projects
{
    public class ProjectService : IProjectService
    {
        public static readonly string PROJECT_ENDPOINT = $"http://{Backend.BASE_URL}/api/projects";
        public static readonly string ITEM_ENDPOINT = $"http://{Backend.BASE_URL}/api/items";

        private IAuthentication _auth;
        private IStorageProvider _storage;
        private IMessageProvider _message;
        private IPhysicalFactory _physical;

        public ProjectService(IAuthentication auth, IStorageProvider storage, IMessageProvider message, IPhysicalFactory physical)
        {
            _auth = auth;
            _storage = storage;
            _message = message;
            _physical = physical;
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
                //TODO: Review return result
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

        public async Task<ProjectModel> GetProject(Guid id)
        {
            try
            {
                //TODO: Review return result
                var task = await _auth.GetAuthenticatedClient().GetAsync($"{PROJECT_ENDPOINT}/{id}");
                var jsonString = await task.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ProjectModel>(jsonString);
                if (!result.IsTemplateCurrent)
                {
                    result = await UpdateModel(result);
                }

                result.PhysicalProject = _physical.CreateProject(result, this);
                result.BuildChildTree();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ItemModel> CreateItem(string name, string physicalName, ItemModel parent, string type)
        {
            ItemModel im = new ItemModel(name, physicalName, parent, type);
            
            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(im));
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            //TODO: Review return result
            var task = await _auth.GetAuthenticatedClient().PostAsync($"{ITEM_ENDPOINT}", httpContent);
            return im;
        }
        
        private async void ProjectModelChanged(object sender, PropertyChangedEventArgs e)
        {
            ProjectModel pm = sender as ProjectModel;
            if(e.PropertyName.Equals(nameof(ProjectModel.Favourite)))
            {
                //TODO: Review return result
                var task = await _auth.GetAuthenticatedClient().PutAsync(PROJECT_ENDPOINT + $"/{pm.Id}/Favourite", null);
            }
            if (e.PropertyName.Equals(nameof(ProjectModel.Children)))
            {
                throw new NotImplementedException();
            }
        }

        private async Task<ProjectModel> UpdateModel(ProjectModel projectModel)
        {
            if (await _message.ShowConfirmDialog("Project template is out of date. Would you like to update?"))
            {
                //TODO: Review return result
                var update = await _auth.GetAuthenticatedClient().PutAsync($"{PROJECT_ENDPOINT}/{projectModel.Id}/updatetemplate", null);
                if (update.IsSuccessStatusCode)
                {
                    return await GetProject(projectModel.Id);
                }
                else
                {
                    throw new ArgumentException("Update failed");
                }
            }
            else
            {
                return projectModel;
            }
        }

        public async Task DeleteItem(Guid id)
        {
            //TODO: Review return result
            var task = await _auth.GetAuthenticatedClient().DeleteAsync($"{ITEM_ENDPOINT}/{id}");
        }
    }
}
