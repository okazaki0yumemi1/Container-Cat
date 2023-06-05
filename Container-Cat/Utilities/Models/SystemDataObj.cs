using Container_Cat.Containers.Models;

namespace Container_Cat.Utilities.Models
{
    public class SystemDataObj
    {
        public Guid Id { get; set; }
        public HostAddress NetworkAddress { get; set; }
        public ContainerEngine InstalledContainerEngines { get; set; }
        public List<BaseContainer> Containers { get; set; }
        public SystemDataObj(HostAddress _networkAddr)
        {
            NetworkAddress = _networkAddr;
            NetworkAddress.SetStatus(HostAddress.HostAvailability.NotTested);
        }
        public SystemDataObj() 
        {
            Id = Guid.NewGuid();
            NetworkAddress.Ip = "127.0.0.1";
            NetworkAddress.Port = "2375";
            NetworkAddress.SetStatus(HostAddress.HostAvailability.NotTested);
            InstalledContainerEngines = ContainerEngine.Unknown;
        }
    }
}
