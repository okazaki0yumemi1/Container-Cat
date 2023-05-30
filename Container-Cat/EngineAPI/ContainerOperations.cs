using Container_Cat.EngineAPI.Models;
using Newtonsoft.Json;
using Container_Cat.Utilities.Linux.Models;

namespace Container_Cat.EngineAPI
{
    public class ContainerOperations
    {
        public ContainerOperations(HttpClient _client, HostAddress _nAddr) 
        { 
            client = _client;
            networkAddr = _nAddr;
        }
        private readonly HttpClient client;
        private readonly HostAddress networkAddr;
        private static EngineAPIEndpoints.Containers cEndpoint;
        //List, inspect are important to implement
        //start, stop, restart, kill - not sure if they are needed right now

        public async Task<List<DockerContainerModel>> ListContainersAsync()
        {
            List<DockerContainerModel> result = new List<DockerContainerModel>();
            HttpResponseMessage response = await client.GetAsync($"http://{networkAddr.Ip}:{networkAddr.Port}/" + cEndpoint.GetAllContainers);
            if (response.IsSuccessStatusCode) 
            {
                var settings = new JsonSerializerSettings();
                settings.TypeNameHandling = TypeNameHandling.All;
                var str = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<List<DockerContainerModel>>(str, settings);
                return result;
            }
            else return null;
        }
        public async Task<DockerContainerModel> GetContainerByIDAsync(string Id)
        {
            DockerContainerModel container = new DockerContainerModel();
            HttpResponseMessage response = await client.GetAsync($"http://{networkAddr.Ip}:{networkAddr.Port}/" + cEndpoint.GetContainerByID.Replace("{id}", Id));
            if (response.IsSuccessStatusCode)
            {
                var str = await response.Content.ReadAsStringAsync();
                container = JsonConvert.DeserializeObject<DockerContainerModel>(str);
                return container;
            }
            else
            {
                Console.WriteLine($"There is no container with Id = {Id}, returning empty container.");
                return null;
            }
        }

    }
}
