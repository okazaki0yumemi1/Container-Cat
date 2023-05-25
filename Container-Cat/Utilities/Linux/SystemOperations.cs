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
        enum ContainerEngine
        {
            Docker,
            Podman
        }
        Dictionary<HostAddress, ContainerEngine> Hosts;// = new Dictionary<HostAddress, ContainerEngine>();
        public SystemOperations()
        {
            //You have to provide HostAddress with IP and Port. 
            //Right now I am providing IP and Port for my local VM.
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
            //validate IP first!
            //also make sure if container engine is correctly initialised
            string pingCommand = $"nmap -p {hostAddr.Port} {hostAddr.Ip} &> /dev/null && echo success || echo fail";
            RunCommand(pingCommand);
            if (pingCommand == "fail") return false; //Host is unreachable
            if (IsDockerInstalled())
            { Hosts.Add(hostAddr, ContainerEngine.Docker); }
            else if (IsPodmanInstalled())
            { Hosts.Add(hostAddr, ContainerEngine.Podman); }
            else return false; //No Podman or Docker is available on the host.
            return true;
        }
        ContainerEngine GetContainerEngine()
        {

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
    }
}
