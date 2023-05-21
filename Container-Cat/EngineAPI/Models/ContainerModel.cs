using System.Runtime.CompilerServices;

namespace Container_Cat.EngineAPI.Models
{
    public class ContainerModel
    {
        public class ContainerNetworkPort
        {
            public string PrivatePort { get; set; }
            public string PublicPort { get; set; }
            public string Type { get; set; }
        }
        public class ContainerHostConfig
        {
            public string NetworkSettings { get; set; }
        }
        public class ContainerNetworkSettings
        {

        }
        public class ContainerMountPoint
        {

        }
        public string Id { get; set; }
        public List<string> Names { get; set; }
        public string Image { get; set; }
        public string ImageID { get; set; }
        public string Command { get; set; }
        //public Int64 Created { get; set; }
        public Int64 SizeRW { get; set; }
        public Int64 SizeRootFs { get; set; }
        //public List<string> Labels { get; set; }
        public string State { get; set; }
        public string Status { get; set; }
        public List<ContainerNetworkPort> Ports { get; set; }
        //public ContainerHostConfig HostConfig { get; set; }
        //public ContainerNetworkSettings NetworkSettings { get; set; }
        //public List<ContainerMountPoint> Mounts { get; set; }
        public ContainerModel() { }
    }
}
