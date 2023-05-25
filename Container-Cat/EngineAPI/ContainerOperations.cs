using Container_Cat.EngineAPI.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Container_Cat.EngineAPI
{
    public class ContainerOperations
    {
        public ContainerOperations() { }
        static HttpClient client = new HttpClient();
        //List, inspect are important to implement
        //start, stop, restart, kill - not sure if they are needed right now
        public async Task<List<DockerContainerModel>> ListContainersAsync()
        {
            List<DockerContainerModel> result = new List<DockerContainerModel>();
            HttpResponseMessage response = await client.GetAsync("http://192.168.56.101:2375/containers/json");
            if (response.IsSuccessStatusCode) 
            {
                var str = await response.Content.ReadAsStringAsync();
                var containerList = JsonConvert.DeserializeObject<List<DockerContainerModel>>(str);
                return containerList;
            }
            else return result;
        }
    }
}
