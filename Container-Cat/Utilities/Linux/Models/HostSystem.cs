using Container_Cat.Utilities.Containers;

namespace Container_Cat.Utilities.Linux.Models
{
    public class HostSystem<T> where T : BaseContainer
    {
        Guid Id { get; set; }
        HostAddress NetworkAddress { get; set; }
        List<T> Containers { get; set; }
        List<ContainerEngine> InstalledContainerEngines { get; set; }
        public HostSystem(HostAddress _networkAddr) 
        { 
            Containers = new List<T>();
            NetworkAddress = _networkAddr;
            Id = Guid.NewGuid();
            NetworkAddress.SetStatus(HostAddress.HostAvailability.NotTested);
        }
        public List<ContainerEngine> GetInstalledContainerEngines()
        {
            return InstalledContainerEngines;  
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
