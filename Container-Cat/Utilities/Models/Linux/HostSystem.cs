using Container_Cat.Utilities.Containers;

namespace Container_Cat.Utilities.Models.Models
{
    public class HostSystem<T> : SystemDataObj where T : BaseContainer 
    {
        Guid Id { get; set; }
        new HostAddress NetworkAddress { get; set; }
        List<T> Containers { get; set; }
        public ContainerEngine InstalledContainerEngine { get; private set; }
        public HostSystem(HostAddress _networkAddr)
        {
            Containers = new List<T>();
            NetworkAddress = _networkAddr;
            Id = Guid.NewGuid();
            NetworkAddress.SetStatus(HostAddress.HostAvailability.NotTested);
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
