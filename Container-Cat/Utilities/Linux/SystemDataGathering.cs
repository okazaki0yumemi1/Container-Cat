using Container_Cat.EngineAPI.Models;
using Container_Cat.Utilities.Containers;
using Container_Cat.Podman_libpod_API.Models;
using Container_Cat.Utilities.Models.Models;
using Container_Cat.Utilities.Models;

namespace Container_Cat.Utilities.Linux
{
    public class SystemDataGathering
    {
        //public HostAddress hostAddr;
        HttpClient client;
        public SystemDataGathering()//HttpClient _client)//, HostAddress _hostAddr)
        {
            client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(20);
            //hostAddr = _hostAddr;
        }
        async Task<bool> IsDockerInstalledAsync(HostAddress hostAddr)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://{hostAddr.Ip}:{hostAddr.Port}/ping");
                if (response.IsSuccessStatusCode) return true;
                else return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException caught while testing host availability.");
                Console.WriteLine("Message :{0} ", e.Message);
                return false;
            }
        }
        async Task<bool> IsPodmanInstalledAsync(HostAddress hostAddr)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://{hostAddr.Ip}:{hostAddr.Port}/libpod/_ping");
                if (response.IsSuccessStatusCode) return true;
                else return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException caught while testing host availability.");
                Console.WriteLine("Message :{0} ", e.Message);
                return false;
            }
        }
        public async Task<ContainerEngine> ContainerEngineInstalledAsync(HostAddress hostAddress)
        {
            //This should and will be changed to a cleanier version
            var dockerCheck = await IsDockerInstalledAsync(hostAddress);
            var podmanCheck = await IsPodmanInstalledAsync(hostAddress);
            if (dockerCheck == true) return ContainerEngine.Docker;
            else if (podmanCheck == true) return ContainerEngine.Podman;
            else return ContainerEngine.Unknown;
        }
        public SystemDataObj ReturnHostSystemData(HostAddress hostAddr)
        {
            SystemDataObj dataObj = new SystemDataObj(hostAddr);
            dataObj.InstalledContainerEngines = ContainerEngineInstalledAsync(hostAddr).Result;
            return dataObj;
        }
        
        //HostSystem<T> CreateHostSystem<T>(HostAddress hostAddr) where T : BaseContainer
        //{
        //    if (IsDockerInstalled(Hosts.First()))
        //    {
        //        HostSystem<DockerContainer> box = new HostSystem<DockerContainer>(Hosts.First());
        //        return box;
        //    }
        //    else if (IsPodmanInstalled(Hosts.First()))
        //    {
        //        HostSystem<PodmanContainer> crate = new HostSystem<PodmanContainer>(Hosts.First());
        //    }
        //}
    }
}
