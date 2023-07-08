using Container_Cat.Containers.Models;

namespace Container_Cat.Utilities.Models
{
    public class SystemEntity
    {
        public Guid Id { get; private set; }
        public HostAddress NetworkAddress { get; set; } = new HostAddress();
        public ContainerEngine InstalledContainerEngine { get; set; } = ContainerEngine.Unknown;
        public SystemEntity(HostAddress _networkAddr)
        {
            Id = Guid.NewGuid();
            NetworkAddress = _networkAddr;
            NetworkAddress.SetStatus(HostAvailability.NotTested);
        }

        public SystemEntity()
        {
            Id = Guid.NewGuid();
            NetworkAddress.Availability = HostAvailability.NotTested;
            InstalledContainerEngine = ContainerEngine.Unknown;
        }

        public SystemEntity(SystemEntity newObj)
        {
            Id = newObj.Id;
            NetworkAddress = newObj.NetworkAddress;
            InstalledContainerEngine = newObj.InstalledContainerEngine;
        }
    }
}