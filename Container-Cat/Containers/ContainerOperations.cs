using Newtonsoft.Json;
using Container_Cat.Containers.EngineAPI.Models;
using Container_Cat.Containers.Models;
using Container_Cat.Utilities.Models;
using Container_Cat.Containers.ApiRoutes;

namespace Container_Cat.Containers.EngineAPI
{
    public class BaseContainerOperations : IContainerOperations<BaseContainer>
    {
        public BaseContainerOperations(HttpClient _client, HostAddress _nAddr)
        {
            client = _client;
            networkAddr = _nAddr;
        }

        private readonly HttpClient client;
        private readonly HostAddress networkAddr;

        public async Task<List<BaseContainer>> ListContainersAsync()
        {
            List<BaseContainer> result = new List<BaseContainer>();
            try
            {
                HttpResponseMessage response = await client.GetAsync(
                    $"http://{networkAddr.Hostname}{networkAddr.Port}/"
                        + DockerEngineAPIEndpoints.Containers.GetAllContainers
                );
                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    
                    var jsonSerializerSettings = new JsonSerializerSettings();
                    jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    result = JsonConvert.DeserializeObject<List<BaseContainer>>(str, jsonSerializerSettings);
                    //result = JsonConvert.DeserializeObject<List<DockerContainer>>(str, jsonSerializerSettings);
                    return result;
                }
                else
                    return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException caught while testing host availability.");
                Console.WriteLine("Message :{0} ", e.Message);
                return result;
            }
        }

        public async Task<BaseContainer> GetContainerByIDAsync(string Id)
        {
            DockerContainer container = new DockerContainer();
            var uri =
                $"http://{networkAddr.Hostname}{networkAddr.Port}/"
                + DockerEngineAPIEndpoints.Containers.GetContainerByID.Replace("{id}", Id);
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                var str = await response.Content.ReadAsStringAsync();
                container = JsonConvert.DeserializeObject<DockerContainer>(str);
                return container;
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException caught while testing host availability.");
                Console.WriteLine("Message :{0} ", e.Message);
                return container;
            }
        }

        public Task<BaseContainer> GetContainerByNameAsync(string Name)
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
