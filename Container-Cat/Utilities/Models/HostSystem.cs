using Container_Cat.Containers.Models;

namespace Container_Cat.Utilities.Models
{
    public class HostSystem<T> where T : BaseContainer
    {
        public Guid Id { get; private set; }
        public HostAddress NetworkAddress { get; private set; }
        public List<T> Containers { get; set; }
        public ContainerEngine InstalledContainerEngine { get; private set; }

        public HostSystem(HostSystemDTO dataObj)
        {
            Id = dataObj.Id;
            NetworkAddress = dataObj.NetworkAddress;
            Containers = new List<T>();
            InstalledContainerEngine = dataObj.InstalledContainerEngine;
        }

        public HostSystem(HostAddress _networkAddr)
        {
            Containers = new List<T>();
            NetworkAddress = _networkAddr;
            Id = Guid.NewGuid();
            NetworkAddress.SetStatus(_networkAddr.Availability);
        }
        public HostSystem() {}
        
    }
}
