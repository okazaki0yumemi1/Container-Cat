using Container_Cat.Containers.EngineAPI.Models;
using Container_Cat.Containers.Models;
using Container_Cat.PodmanAPI.Models;
using static Container_Cat.Containers.ApiRoutes.DockerEngineAPIEndpoints;

namespace Container_Cat.Utilities.Models
{
    public struct HostSystemDTO
    {
        public Guid Id { get; set; }

        public HostAddress NetworkAddress { get; set; } = new HostAddress();

        public ContainerEngine InstalledContainerEngine { get; set; } = ContainerEngine.Unknown;

        public List<BaseContainer> Containers { get; set; } = new List<BaseContainer>();

        public HostSystemDTO(HostSystem<BaseContainer> newObj)
        {
            Id = newObj.Id;
            NetworkAddress = newObj.NetworkAddress;
            InstalledContainerEngine = newObj.InstalledContainerEngine;
            Containers = newObj.Containers;
        }
        public HostSystemDTO(HostSystem<PodmanContainer> newObj)
        {
            throw new NotImplementedException("Podman container support is not yet implemented.");
            /*
            Id = newObj.Id;
            NetworkAddress = newObj.NetworkAddress;
            InstalledContainerEngine = newObj.InstalledContainerEngine;
            Containers.Clear();
            foreach (var podmanContainer in newObj.Containers)
            {
                BaseContainer container = new BaseContainer();
                container.Id = podmanContainer.Id;
                container.Name = podmanContainer.Name;
                container.State = podmanContainer.State;
                container.Image = podmanContainer.Image;
                foreach (var mountPoint in podmanContainer.Mounts)
                {
                    Containers.Models.Mount mount = new Containers.Models.Mount();
                    mount.Source = mountPoint.Source;
                    mount.Destination = mountPoint.Destination;
                    mount.Type = mountPoint.Type;
                    mount.RW = mountPoint.RW;
                    container.Mounts.Add(mount);
                }
                foreach (var containerPort in podmanContainer.Ports)
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
            */
        }

    }
}
