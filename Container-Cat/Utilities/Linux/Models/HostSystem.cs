using Container_Cat.Utilities.Containers;

namespace Container_Cat.Utilities.Linux.Models
{
    public class HostSystem
    {
        Guid Id { get; set; }
        HostAddress NetworkAddress { get; set; }
        List<BaseContainer> Containers { get; set; }
        public HostSystem() { }
        //Container engine
        //Network address
        //other stuff - maybe list of container id's?
    }
}
