using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Jpp.Common.Backend.Auth;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Jpp.Common.Backend.Activty
{
    public class ActivityService : IActivityService, IDisposable
    {
        public static readonly string ACTIVITIES_ENDPOINT = $"http://{Backend.BASE_URL}/api/activities";
        private IAuthentication _auth;

        private List<WeakReference<ActivityStream>> _activeStreams;
        private IConnection _connection;
        private IModel _channel;

        public ActivityService(IAuthentication auth)
        {
            _auth = auth;
            _activeStreams = new List<WeakReference<ActivityStream>>();

            var factory = new ConnectionFactory() { HostName = Backend.BASE_URL };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("activities", ExchangeType.Fanout);

            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queueName, "activities", routingKey: "");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                ProcessMessage(ea);
            };
            _channel.BasicConsume(queueName, true, consumer);
        }

        public async Task<ActivityStream> GetAllActivities()
        {
            try
            {
                var task = await _auth.GetAuthenticatedClient().GetAsync(ACTIVITIES_ENDPOINT + $"?Page=1&Size=50");
                var jsonString = await task.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<IEnumerable<Activity>>(jsonString);

                /*ProcessActivities(result);

                return result;*/
                return null;
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

        private void ProcessMessage(BasicDeliverEventArgs ea)
        {

        }

        public Task<ActivityStream> GetActivitiesForEntity(Guid entityId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
