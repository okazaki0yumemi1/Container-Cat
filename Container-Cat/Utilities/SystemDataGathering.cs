using Container_Cat.Utilities.Models;
using Container_Cat.Containers.Models;
using Container_Cat.Containers.ApiRoutes;
using Container_Cat.Containers.EngineAPI.Models;
using Container_Cat.Containers.EngineAPI;

namespace Container_Cat.Utilities
{
    public class SystemDataGathering
    {
        private readonly HttpClient _client;

        public SystemDataGathering(HttpClient client) //, HostAddress _hostAddr)
        {
            _client = client;
            _client.Timeout = TimeSpan.FromSeconds(20);
        }

        async Task<ContainerEngine> DetectApiAsync(string hostAddr)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(
                    $"http://{hostAddr}/{DockerEngineAPIEndpoints.Version}"
                );
                if (response.IsSuccessStatusCode)
                {
                    return ContainerEngine.Docker;
                }
                else if (response.StatusCode is System.Net.HttpStatusCode.NotFound)
                {
                    //response = await client.GetAsync($"http://{hostAddr.Ip}{hostAddr.Port}/{DockerEngineAPIEndpoints.Info}");
                    //This is just for testing:
                    response = await _client.GetAsync(
                        $"http://{hostAddr}/libpod/info"
                    );
                    response.EnsureSuccessStatusCode();
                    return ContainerEngine.Podman;
                }
                else
                {
                    NotImplementedException e = new NotImplementedException(
                        "Unable to identify container engine from response body."
                    );
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

        //public async IAsyncEnumerable<SystemDataObj> FetchDataObjectRangeAsync(List<HostAddress> hostAddr)
        //{
        //    foreach (var host in hostAddr)
        //    {
        //        SystemDataObj dataObj = new SystemDataObj(host);
        //        dataObj.InstalledContainerEngine = await ContainerEngineInstalledAsync(host);
        //        yield return dataObj;
        //    }
        //}
        //async Task<bool> IsDockerInstalledAsync(HostAddress hostAddr)
        //{
        //    try
        //    {
        //        HttpResponseMessage response = await client.GetAsync($"http://{hostAddr.Ip}{hostAddr.Port}/version");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            //Do some checks for API version
        //            return true;
        //        }
        //        else return false;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("\nException caught while testing host availability.");
        //        Console.WriteLine("Message :{0} ", e.Message);
        //        return false;
        //    }
        //}
        //async Task<bool> IsPodmanInstalledAsync(HostAddress hostAddr)
        //{
        //    try
        //    {
        //        HttpResponseMessage response = await client.GetAsync($"http://{hostAddr.Ip}{hostAddr.Port}/libpod/_ping");
        //        if (response.IsSuccessStatusCode) return true;
        //        else return false;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("\nException caught while testing host availability.");
        //        Console.WriteLine("Message :{0} ", e.Message);
        //        return false;
        //    }
        //}
        public async Task<ContainerEngine> ContainerEngineInstalledAsync(string hostAddress)
        {
            var apiType = await DetectApiAsync(hostAddress);
            return apiType;
        }

        public async Task<HostAvailability> IsAPIAvailableAsync(string hostAddr)//)
        {
            try
            {
                using HttpResponseMessage response = await _client.GetAsync(
                    $"http://{hostAddr}/info"
                );
                switch ((int)response.StatusCode)
                {
                    case 200:
                        return HostAvailability.Connected;
                    case >= 400
                    and < 500:
                        return HostAvailability.Unreachable;
                    default:
                        return HostAvailability.Unreachable;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException caught while testing host availability.");
                Console.WriteLine("Message :{0} ", e.Message);
                return HostAvailability.Unreachable;
            }
        }

        public async Task<List<BaseContainer>> GetContainersAsync(
            HostSystem<BaseContainer> dockerHost
        )
        {
            var probe = await IsAPIAvailableAsync(dockerHost.NetworkAddress.Hostname);
            if (probe == HostAvailability.Connected)
            {
                dockerHost.NetworkAddress.SetStatus(HostAvailability.Connected);
                BaseContainerOperations cOps = new BaseContainerOperations(
                    _client,
                    dockerHost.NetworkAddress
                );
                var containers = await cOps.ListContainersAsync();
                if (containers.Count == 0)
                {
                    Console.WriteLine(
                        $"Failed to get container list for {dockerHost.NetworkAddress.Hostname}, empty container will be added."
                    );
                    return (new List<BaseContainer>());
                }
                else
                {
                    return containers;
                }
            }
            else
                return (new List<BaseContainer>());
        }
    }
}
