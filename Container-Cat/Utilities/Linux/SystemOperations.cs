using Container_Cat.EngineAPI;
using Container_Cat.EngineAPI.Models;
using Container_Cat.Utilities.Containers;
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
        //Dictionary<HostAddress, ContainerEngine> Hosts;// = new Dictionary<HostAddress, ContainerEngine>();
        static List<HostAddress> Hosts = new List<HostAddress>();
        static HttpClient client = new HttpClient();
        List<HostSystem<DockerContainerModel>> Systems;
        public SystemOperations()
        {
            //You have to provide HostAddress with IP and Port. 
            //Right now I am providing IP and Port for my local VM.
            Hosts = new List<HostAddress>();
            Systems = new List<HostSystem<DockerContainerModel>>();
            HostAddress testHost = new HostAddress("192.168.56.101", "2375");
            if (AddHost(testHost)) Console.WriteLine("Host was added successfully.");
            else Console.WriteLine("Unable to add host.");
        }
        string RunCommand(string command)
        {
            var processOutput = "";
            var processInfo = new ProcessStartInfo();
            processInfo.FileName = "/bin/bash";
            processInfo.Arguments = $"-c \"{command}\"";
            using (var process = Process.Start(processInfo))
            {
                processOutput = process.StandardOutput.ReadToEnd();
                process.Kill();
            }
            return processOutput;
        }
        bool AddHost(HostAddress hostAddr)
        {
            Hosts.Add(hostAddr);
            return true;
            //validate IP first!
            //also make sure if container engine is correctly initialised

            //string pingCommand = $"nmap -p {hostAddr.Port} {hostAddr.Ip} &> /dev/null && echo success || echo fail";
            //RunCommand(pingCommand);
            //if (pingCommand == "fail") return false; //Host is unreachable
            //if (IsDockerInstalled())
            //{ Hosts.Add(hostAddr); }
            //else if (IsPodmanInstalled())
            //{ Hosts.Add(hostAddr); }
            //else return false; //No Podman or Docker is available on the host.
            //return true;
        }
        bool IsDockerInstalled()
        {
            string getDockerVersion = "docker info";
            string res = RunCommand(getDockerVersion);
            if (res.Length > 0) return true;
            else return false;
        }
        bool IsPodmanInstalled() 
        {
            return false;
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
                ContainerOperations cOps = new ContainerOperations(client, host);
                var containers = await cOps.ListContainersAsync();
                _system.AddContainers(containers);
                _systems.Add(_system);
            });
            await Task.WhenAll(tasks);
            Systems.AddRange(_systems);
            Systems.Distinct();
            return _systems.Count;
            //foreach (HostAddress _host in Hosts) 
            //{
            //    HostSystem _system = new HostSystem(_host);
            //    ContainerOperations cOps = new ContainerOperations(client, _host);
            //    var containers = await cOps.ListContainersAsync();
            //    systems.Add( _system );
            //}
        }
    }
}
