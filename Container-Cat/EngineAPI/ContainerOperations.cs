using Container_Cat.EngineAPI.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Text.Json;

namespace Container_Cat.EngineAPI
{
    public class ContainerOperations
    {
        public ContainerOperations() { }
        static HttpClient client = new HttpClient();
        //List, inspect are important to implement
        //start, stop, restart, kill - not sure if they are needed right now
        public /*static*/ async Task<List<ContainerModel>> ListContainersAsync()
        {
            List<ContainerModel> result = new List<ContainerModel>();
            HttpResponseMessage response = await client.GetAsync("http://192.168.56.101:2375/containers/json");
            if (response.IsSuccessStatusCode) 
            {
                //using var containersJsonStream = response.Content.ReadAsStream();//new MemoryStream(Encoding.UTF8.GetBytes(moviesJson));
                //await foreach (var container in JsonSerializer.DeserializeAsyncEnumerable<ContainerModel>(containersJsonStream))
                //{
                //    result.Add(container);
                //}
                var str = await response.Content.ReadAsStringAsync();
                var containerList = JsonConvert.DeserializeObject<List<ContainerModel>>(str);
                result = containerList;
            }
            return result;
        }
    }
}
