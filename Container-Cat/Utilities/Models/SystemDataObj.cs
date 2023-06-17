using Container_Cat.Containers.EngineAPI.Models;
using Container_Cat.Containers.Models;
using Container_Cat.PodmanAPI.Models;

namespace Container_Cat.Utilities.Models
{
    public class SystemDataObj
    {
        public Guid Id { get; private set; }
        public HostAddress NetworkAddress { get; set; } = new HostAddress();
        public ContainerEngine InstalledContainerEngine { get; set; } = ContainerEngine.Unknown;
        public List<BaseContainer> Containers { get; set; } = new List<BaseContainer>();
        public SystemDataObj(HostAddress _networkAddr)
        {
            Id = Guid.NewGuid();
            NetworkAddress = _networkAddr;
            NetworkAddress.SetStatus(HostAddress.HostAvailability.NotTested);
        }
        public SystemDataObj() 
        {
            Id = Guid.NewGuid();
            NetworkAddress.Availability = HostAddress.HostAvailability.NotTested;
            InstalledContainerEngine = ContainerEngine.Unknown;
        }
        public SystemDataObj(SystemDataObj newObj)
        {
            Id = newObj.Id;
            NetworkAddress = newObj.NetworkAddress;
            InstalledContainerEngine = newObj.InstalledContainerEngine;
            Containers = newObj.Containers;
        }
        public void ReplaceToBaseContainers(List<DockerContainer> containers)
        {
            Containers.Clear();
            foreach (var dockerContainer in containers)
            {
                BaseContainer container = new BaseContainer();
                container.Id = dockerContainer.Id;
                container.Name = dockerContainer.Name;
                container.State = dockerContainer.State;
                container.Image = dockerContainer.Image;
                foreach (var mountPoint in dockerContainer.Mounts)
                {
                    Containers.Models.Mount mount = new Containers.Models.Mount();
                    mount.Source = mountPoint.Source;
                    mount.Destination = mountPoint.Destination;
                    mount.Type = mountPoint.Type;
                    mount.RW = mountPoint.RW;
                    container.Mounts.Add(mount);
                }
                foreach (var containerPort in dockerContainer.Ports)
                {
                    Containers.Models.Port port = new Containers.Models.Port();
                    port.PrivatePort = containerPort.PrivatePort;
                    port.PublicPort = containerPort.PublicPort;
                    port.IP = containerPort.IP;
                    port.Type = containerPort.Type;
                    container.Ports.Add(port);
                }
                Containers.Add(container);
            }
        }
        public void AddBaseContainers(List<DockerContainer> containers)
        {
            foreach (var dockerContainer in containers) 
            { 
                BaseContainer container = new BaseContainer();
                container.Id = dockerContainer.Id;
                container.Name = dockerContainer.Name;
                container.State = dockerContainer.State;
                container.Image = dockerContainer.Image;
                foreach (var mountPoint in dockerContainer.Mounts)
                {
                    Containers.Models.Mount mount = new Containers.Models.Mount();
                    mount.Source = mountPoint.Source;
                    mount.Destination = mountPoint.Destination;
                    mount.Type = mountPoint.Type;
                    mount.RW = mountPoint.RW;
                    container.Mounts.Add(mount);
                }
                foreach (var containerPort in dockerContainer.Ports)
                {
                    Containers.Models.Port port = new Containers.Models.Port();
                    port.PrivatePort = containerPort.PrivatePort;
                    port.PublicPort = containerPort.PublicPort;
                    port.IP = containerPort.IP;
                    port.Type = containerPort.Type;
                    container.Ports.Add(port);
                }
                Containers.Add(container);
            }
        }
    }
}
