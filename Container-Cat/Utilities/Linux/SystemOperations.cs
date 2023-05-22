using System.Diagnostics;

namespace Container_Cat.Utilities
{
    public class SystemOperations
    {
        public enum ContainerEngine
        {
            Docker,
            Podman
        }
        public struct HostAddress
        {
            public string Ip { get; private set; }
            public string Port { get; private set; }
        }
        Dictionary<HostAddress, ContainerEngine> Hosts = new Dictionary<HostAddress, ContainerEngine>();
        public SystemOperations()
        {
        }
        private string RunCommand(string command)
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
        private bool AddHost(HostAddress hostAddr)
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
        private bool IsDockerInstalled()
        {
            string getDockerVersion = "docker info";
            string res = RunCommand(getDockerVersion);
            if (res.Length > 0) return true;
            else return false;
        }
        private bool IsPodmanInstalled() 
        {
            return false;
        }
    }
}
