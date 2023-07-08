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
        public HostSystemDTO()
        {
            Id = Guid.NewGuid();
        }
        public HostSystemDTO(SystemEntity newObj)
        {
            Id = newObj.Id;
            NetworkAddress = newObj.NetworkAddress;
            InstalledContainerEngine = newObj.InstalledContainerEngine;
        }

        public HostSystemDTO(HostSystem<BaseContainer> newObj)
        {
            Id = newObj.Id;
            NetworkAddress = newObj.NetworkAddress;
            InstalledContainerEngine = newObj.InstalledContainerEngine;
            Containers = newObj.Containers;
        }
    }
}
