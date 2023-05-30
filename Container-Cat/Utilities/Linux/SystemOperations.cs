using Container_Cat.EngineAPI;
using Container_Cat.EngineAPI.Models;
using Container_Cat.Utilities.Linux.Models;
using System.Diagnostics;

namespace Container_Cat.Utilities
{
    public class SystemOperations
    {
        //This class is used to:
        //Add HostSystem to List<HostSystem>
        //Determine if HostSystem contains any container engines
        //...
        //Think of it as a tool to populate HostSystem entity with data.
        static List<HostAddress> Hosts = new List<HostAddress>();
        static HttpClient client = new HttpClient();
        List<HostSystem<DockerContainerModel>> Systems;
        public SystemOperations()
        {
            //You have to provide HostAddress with IP and Port. 
            //Right now I am providing IP and Port for my local VMs.
            Hosts = new List<HostAddress>();
            Systems = new List<HostSystem<DockerContainerModel>>();
            List<HostAddress> testHostLits = new List<HostAddress>()
                {new HostAddress("127.0.0.1", "3375"), new HostAddress("192.168.56.999", "3375"), new HostAddress("192.168.0.104", "3375")};
            foreach (var testHost in testHostLits)
            {
                if(AddHost(testHost)) Console.WriteLine("Host was added successfully.");
                else Console.WriteLine("Unable to add host.");
            }
        }
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
        bool IsHostReachable(HostAddress hostAddress)
        {
            string pingCommand = $"nmap -p {hostAddress.Port} {hostAddress.Ip} -Pn | grep 'Host is up' &> /dev/null && echo up || echo down";
            string result = RunCommand(pingCommand).Replace("\n", "");
            if (result == "up") return true;
            else return false;
        }
        bool AddHost(HostAddress hostAddr)
        { 
            //validate IP first!
            //also make sure if container engine is correctly initialised
            if (IsHostReachable(hostAddr) == true)
            {
                hostAddr.SetStatus(HostAddress.HostAvailability.NotTested);
                Hosts.Add(hostAddr);
                return true;
            }
            else return false;
        }
        bool IsAPIAvailable(HostAddress hostAddr)
        {
            //curl 192.168.56.99:3375 --connect-timeout 1 | grep "{" &> /dev/null && echo connected || echo no-connection
            string curlToAddress = $"curl {hostAddr.Ip}:{hostAddr.Port} --max-time 90 --connect-timeout 30 | grep '{{' &> /dev/null && echo connected || echo no-connection"; //Connect timeout = 5 seconds
            string result = RunCommand(curlToAddress).Replace("\n", "");
            if (result == "connected") return true;
            else return false;
        }
        async Task<Utilities.Linux.Models.HostAddress.HostAvailability> IsAPIAvailableAsync(HostAddress hostAddr)
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
        public List<HostSystem<DockerContainerModel>> GetHostSystems()
        {
            return Systems;
        }
        /*
         * This function makes sense if there are more than 1 host machine.
         * It works with 1 machine too.
         */
        public async Task<int> InitialiseHostSystemsAsync()
        {
            List<HostSystem<DockerContainerModel>> _systems = new List<HostSystem<DockerContainerModel>>();
            var tasks = Hosts.Select(async host =>
            {
                HostSystem<DockerContainerModel> _system = new HostSystem<DockerContainerModel>(host);
                var probe = await IsAPIAvailableAsync(host);
                if (probe == HostAddress.HostAvailability.Connected)
                {
                    host.SetStatus(HostAddress.HostAvailability.Connected);
                    ContainerOperations cOps = new ContainerOperations(client, host);
                    var containers = await cOps.ListContainersAsync();
                    if (containers == null)
                    {
                        Console.WriteLine($"Failed to get container list for {host.Ip}:{host.Port}, no containers will be added.");
                    }
                    else _system.AddContainers(containers);
                }
                else host.SetStatus(HostAddress.HostAvailability.Unreachable);
                _systems.Add(_system);
            });
            await Task.WhenAll(tasks);
            Systems.AddRange(_systems);
            Systems.Distinct();
            return _systems.Count;
        }
    }
}
