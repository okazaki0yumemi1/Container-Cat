using Container_Cat.Containers.Models;

namespace Container_Cat.Utilities.Models
{
    public class SystemDataObj
    {
        //public Guid Id { get; private set; }
        public HostAddress NetworkAddress { get; set; }
        public ContainerEngine InstalledContainerEngines { get; set; }
        public SystemDataObj(HostAddress _networkAddr)
        {
            NetworkAddress = _networkAddr;
            NetworkAddress.SetStatus(HostAddress.HostAvailability.NotTested);
        }
        public SystemDataObj() { }
    }
}
