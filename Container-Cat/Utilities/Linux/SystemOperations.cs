using Container_Cat.EngineAPI;
using Container_Cat.EngineAPI.Models;
using Container_Cat.Utilities.Containers;
using Container_Cat.Utilities.Linux.Models;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

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
            List<HostAddress> testHostLits = new List<HostAddress>()
                {new HostAddress("127.0.0.1", "3375"), new HostAddress("192.168.56.99", "3375"), new HostAddress("192.168.0.104", "3375")};
            foreach (var testHost in testHostLits)
            {
                if(AddHost(testHost)) Console.WriteLine("Host was added successfully.");
                else Console.WriteLine("Unable to add host.");
            }
            //if (AddHost(testHost)) Console.WriteLine("Host was added successfully.");
            //else Console.WriteLine("Unable to add host.");
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

            if ((IsHostReachable(hostAddr) == true) && (IsAPIAvailable(hostAddr) == true))
            {
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
