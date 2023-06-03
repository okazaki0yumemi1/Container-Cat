using Container_Cat.EngineAPI.Models;
using Newtonsoft.Json;
using Container_Cat.Utilities.Containers;
using Container_Cat.Utilities.Models.Models;

namespace Container_Cat.EngineAPI
{
    public class DockerContainerOperations : IContainerOperations<DockerContainer>
    {
        public DockerContainerOperations(HttpClient _client, HostAddress _nAddr) 
        { 
            client = _client;
            networkAddr = _nAddr;
        }
        private readonly HttpClient client;
        private readonly HostAddress networkAddr;
        private static DockerEngineAPI.Containers cEndpoint = new DockerEngineAPI.Containers() { };
        //List, inspect are important to implement
        //start, stop, restart, kill - not sure if they are needed right now

        public async Task<List<DockerContainer>> ListContainersAsync()
        {
            List<DockerContainer> result = new List<DockerContainer>();
            HttpResponseMessage response = await client.GetAsync($"http://{networkAddr.Ip}:{networkAddr.Port}/" + cEndpoint.GetAllContainers);
            if (response.IsSuccessStatusCode)
            {
                var settings = new JsonSerializerSettings();
                var str = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<List<DockerContainer>>(str);
                return result;
            }
            else return result;
        }
        public async Task<DockerContainer> GetContainerByIDAsync(string Id)
        {
            DockerContainer container = new DockerContainer();
            var uri = $"http://{networkAddr.Ip}:{networkAddr.Port}/" + cEndpoint.GetContainerByID.Replace("{id}", Id);
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var str = await response.Content.ReadAsStringAsync();
                container = JsonConvert.DeserializeObject<DockerContainer>(str);
                return container;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                Console.WriteLine($"Got Error 400. GET-request to: {uri}.");
                Console.WriteLine("Returning null.");
                return null;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                Console.WriteLine($"Got Error 500. GET-request to: {uri}. Is host {networkAddr.Ip}:{networkAddr.Port} okay?");
                return null;
            }
            else return null;
        }

        public Task<DockerContainer> GetContainerByNameAsync(string Name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> StartContainerAsync(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> StopContainerAsync(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RestartContainerAsync(string Id)
        {
            throw new NotImplementedException();
        }
    }
}
