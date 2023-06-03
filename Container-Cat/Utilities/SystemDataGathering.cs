using Container_Cat.Utilities.Models;
using Container_Cat.Containers.Models;
using Container_Cat.Containers.ApiRoutes;

namespace Container_Cat.Utilities
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
        async Task<ContainerEngine> DetectApiAsync(HostAddress hostAddr)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://{hostAddr.Ip}:{hostAddr.Port}/{DockerEngineAPIEndpoints.Version}");
                if (response.IsSuccessStatusCode)
                {
                    return ContainerEngine.Docker;
                }
                else if (response.StatusCode is System.Net.HttpStatusCode.NotFound)
                {
                    //response = await client.GetAsync($"http://{hostAddr.Ip}:{hostAddr.Port}/{DockerEngineAPIEndpoints.Info}");
                    //This is just for testing:
                    response = await client.GetAsync($"http://{hostAddr.Ip}:{hostAddr.Port}/libpod/info");
                    response.EnsureSuccessStatusCode();
                    return ContainerEngine.Podman;
                }
                else
                {
                    NotImplementedException e = new NotImplementedException("Unable to identify container engine from response body.");
                    throw e;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException caught while checking container engine type.");
                Console.WriteLine("Message :{0} ", ex.Message);
                return ContainerEngine.Unknown;
            }
        }
        async Task<bool> IsDockerInstalledAsync(HostAddress hostAddr)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://{hostAddr.Ip}:{hostAddr.Port}/version");
                if (response.IsSuccessStatusCode)
                {
                    //Do some checks for API version
                    return true;
                }
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
            //var dockerCheck = await IsDockerInstalledAsync(hostAddress);
            //var podmanCheck = await IsPodmanInstalledAsync(hostAddress);
            //if (dockerCheck == true) return ContainerEngine.Docker;
            //else if (podmanCheck == true) return ContainerEngine.Podman;
            var apiType = await DetectApiAsync(hostAddress);
            return apiType;
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
