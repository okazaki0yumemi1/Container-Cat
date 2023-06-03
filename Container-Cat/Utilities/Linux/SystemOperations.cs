using Container_Cat.EngineAPI;
using Container_Cat.EngineAPI.Models;
using Container_Cat.Podman_libpod_API;
using Container_Cat.Podman_libpod_API.Models;
using Container_Cat.Utilities.Containers;
using Container_Cat.Utilities.Models.Models;

namespace Container_Cat.Utilities
{
    public class SystemOperations<T> where T : BaseContainer
    {
        //This class is used to:
        //Add HostSystem to List<HostSystem>
        //Determine if HostSystem contains any container engines
        //...
        //Think of it as a tool to populate HostSystem entity with data.
        static List<HostAddress> Hosts = new List<HostAddress>();
        static HttpClient client = new HttpClient();
        List<HostSystem<T>> Systems;
        public SystemOperations(List<HostAddress> hosts)
        {
            //You have to provide HostAddress with IP and Port. 
            //Right now I am providing IP and Port for my local VMs.
            Hosts = new List<HostAddress>();
            Systems = new List<HostSystem<T>>();
            foreach (var testHost in hosts)
            {
                if(AddHost(testHost)) Console.WriteLine("Host was added successfully.");
                else Console.WriteLine("Unable to add host.");
            }
        }
        //The following lines should not be used because of compatibility and unnecessary code.
        //Why use bash (and then implement PowerShell and MacOS shell commands) when you can use HttpClient?
        /*
        string RunCommand(string command)
        {
            var processOutput = "";
            var processInfo = new ProcessStartInfo();
            processInfo.FileName = "/bin/bash";
            processInfo.Arguments = $"-c \"{command}\"";
            processInfo.RedirectStandardOutput = true;
            using (var process = Process.Start(processInfo))
            {
                processOutput = process.StandardOutput.ReadToEnd();
                
                process.Kill();
            }
            return processOutput;
        }
        bool IsHostReachable(HostAddress hostAddr)
        {
            string pingCommand = $"nmap -p {hostAddr.Port} {hostAddr.Ip} -Pn | grep 'Host is up' &> /dev/null && echo up || echo down";
            string result = RunCommand(pingCommand).Replace("\n", "");
            if (result == "up") return true;
            else return false;
        }
        */
        bool AddHost(HostAddress hostAddr)
        { 
            //validate IP first!
            //also make sure if container engine is correctly initialised
            if (IsAPIAvailableAsync(hostAddr).Result == HostAddress.HostAvailability.Connected)//IsHostReachable(hostAddr) == true)
            {
                hostAddr.SetStatus(HostAddress.HostAvailability.NotTested);
                Hosts.Add(hostAddr);
                return true;
            }
            else return false;
        }
        async Task<HostAddress.HostAvailability> IsAPIAvailableAsync(HostAddress hostAddr)
        {
            try
            {
                using HttpResponseMessage response = await client.GetAsync($"http://{hostAddr.Ip}:{hostAddr.Port}/info");
                switch ((int)response.StatusCode)
                {
                    case 200: return HostAddress.HostAvailability.Connected;
                    case >= 400 and < 500: return HostAddress.HostAvailability.Unreachable;
                    default: return HostAddress.HostAvailability.Unreachable;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException caught while testing host availability.");
                Console.WriteLine("Message :{0} ", e.Message);
                return HostAddress.HostAvailability.NotTested;
            }
        }
        public List<HostSystem<T>> GetHostSystems()
        {
            return Systems;
        }
        /*
         * This function makes sense if there are more than 1 host machine.
         * It works with 1 machine too.
         */
        public async Task<int> InitialiseHostSystemsAsync()
        {
            var tasks = Systems.Select(async system =>
            {
                var probe = await IsAPIAvailableAsync(system.NetworkAddress);
                if (probe == HostAddress.HostAvailability.Connected)
                {
                    system.NetworkAddress.SetStatus(HostAddress.HostAvailability.Connected);
                    if (typeof(T) == typeof(DockerContainer))
                    {
                        HostSystem<DockerContainer> _system = new HostSystem<DockerContainer>(system.NetworkAddress);
                        DockerContainerOperations cOps = new DockerContainerOperations(client, system.NetworkAddress);
                        var containers = await cOps.ListContainersAsync();
                        if (containers.Count != 0)
                        {
                            Console.WriteLine($"Failed to get container list for {system.NetworkAddress.Ip}:{system.NetworkAddress.Port}, empty container will be added.");
                        }
                        else _system.AddContainers(containers);
                    }
                    else if (typeof(T) == typeof(PodmanContainer))
                    {
                        HostSystem<PodmanContainer> _system = new HostSystem<PodmanContainer>(system.NetworkAddress);
                        PodmanContainerOperations cOps = new PodmanContainerOperations(client, system.NetworkAddress);
                        var containers = await cOps.ListContainersAsync();
                        if (containers == null)
                        {
                            Console.WriteLine($"Failed to get container list for {system.NetworkAddress.Ip}:{system.NetworkAddress.Port}, no containers will be added.");
                        }
                        else _system.AddContainers(containers);
                    }
                    else Console.WriteLine($"Unable to get any containers from {system.NetworkAddress.Ip}:{system.NetworkAddress.Port}.");
                }
                else system.NetworkAddress.SetStatus(HostAddress.HostAvailability.Unreachable);
                Systems.Add(system);
            });
            await Task.WhenAll(tasks);
            return Systems.Count;
        }
    }
}
