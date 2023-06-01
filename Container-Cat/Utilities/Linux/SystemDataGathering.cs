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
            //hostAddr = _hostAddr;
        }
        bool IsDockerInstalled(HostAddress hostAddr)
        {
            try
            {
                using HttpResponseMessage response = client.GetAsync($"http://{hostAddr.Ip}:{hostAddr.Port}/ping").Result;
                switch ((int)response.StatusCode)
                {
                    case 200: return true;
                    default: return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException caught while testing host availability.");
                Console.WriteLine("Message :{0} ", e.Message);
                return false;
            }
        }
        bool IsPodmanInstalled(HostAddress hostAddr)
        {
            try
            {
                using HttpResponseMessage response = client.GetAsync($"http://{hostAddr.Ip}:{hostAddr.Port}/libpod/_ping").Result;
                switch ((int)response.StatusCode)
                {
                    case 200: return true;
                    default: return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException caught while testing host availability.");
                Console.WriteLine("Message :{0} ", e.Message);
                return false;
            }
        }
        public ContainerEngine ContainerEngineInstalled(HostAddress hostAddress)
        {
            if (IsDockerInstalled(hostAddress)) return ContainerEngine.Docker;
            else if (IsPodmanInstalled(hostAddress)) return ContainerEngine.Podman;
            else return ContainerEngine.Unknown;
        }
        public SystemDataObj ReturnHostSystemData(HostAddress hostAddr)
        {
            SystemDataObj dataObj = new SystemDataObj(hostAddr);
            dataObj.InstalledContainerEngines = ContainerEngineInstalled(hostAddr);
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
