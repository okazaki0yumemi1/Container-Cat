using Container_Cat.Containers.Models;

namespace Container_Cat.Utilities.Models
{
    public class HostSystem<T> : SystemDataObj where T : BaseContainer
    {
        Guid Id { get; set; }
        new public HostAddress NetworkAddress { get; private set; }
        List<T> Containers { get; set; }
        public ContainerEngine InstalledContainerEngine { get; private set; }
        public HostSystem(SystemDataObj dataObj) 
        {
            Containers = new List<T>();
            NetworkAddress = dataObj.NetworkAddress;
            Id = Guid.NewGuid();
            InstalledContainerEngine = dataObj.InstalledContainerEngines;
        }
        public HostSystem(HostAddress _networkAddr)
        {
            Containers = new List<T>();
            NetworkAddress = _networkAddr;
            Id = Guid.NewGuid();
            NetworkAddress.SetStatus(_networkAddr.Availability);
        }
        public void AddContainers(List<T> containers)
        {
            Containers.AddRange(containers);
            Containers.Distinct();
        }
        public void UpdateNetworkStatus()
        {
        }
        //Container engine
        //Network address
        //other stuff - maybe list of container id's?
    }
}
