using Container_Cat.EngineAPI.Models;
using Newtonsoft.Json;
using Container_Cat.Utilities.Containers;
using Container_Cat.Utilities.Models.Models;
using System.Linq.Expressions;

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
       
        //List, inspect are important to implement
        //start, stop, restart, kill - not sure if they are needed right now

        public async Task<List<DockerContainer>> ListContainersAsync()
        {
            List<DockerContainer> result = new List<DockerContainer>();
            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://{networkAddr.Ip}:{networkAddr.Port}/" + DockerEngineAPIEndpoints.Containers.GetAllContainers);
                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<List<DockerContainer>>(str);
                    return result;
                }
                else return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException caught while testing host availability.");
                Console.WriteLine("Message :{0} ", e.Message);
                return result;
            }

        }
        public async Task<DockerContainer> GetContainerByIDAsync(string Id)
        {
            DockerContainer container = new DockerContainer();
            var uri = $"http://{networkAddr.Ip}:{networkAddr.Port}/" + DockerEngineAPIEndpoints.Containers.GetContainerByID.Replace("{id}", Id);
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
