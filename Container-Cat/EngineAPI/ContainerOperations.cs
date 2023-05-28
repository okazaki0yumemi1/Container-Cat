using Container_Cat.EngineAPI.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System.ComponentModel;

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
        //List, inspect are important to implement
        //start, stop, restart, kill - not sure if they are needed right now
        public async Task<List<DockerContainerModel>> ListContainersAsync()
        {
            List<DockerContainerModel> result = new List<DockerContainerModel>();
            HttpResponseMessage response = await client.GetAsync($"http://{networkAddr.Ip}:{networkAddr.Port}/containers/json");
            if (response.IsSuccessStatusCode) 
            {
                var settings = new JsonSerializerSettings();
                settings.TypeNameHandling = TypeNameHandling.All;
                var str = await response.Content.ReadAsStringAsync();
                var containerList = JsonConvert.DeserializeObject<List<DockerContainerModel>>(str, settings);
                return containerList;
            }
            else return result;
        }
    }
}
