using Container_Cat.Containers.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Container_Cat.Containers.EngineAPI.Models
{


    //public class DockerContainer 
    //{
    //    public DockerContainer() { }
    public class DockerContainer : BaseContainer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string[] Names { get; set; }
        public string Image { get; set; }
        public string ImageID { get; set; }
        public string Command { get; set; }
        public int Created { get; set; }
        public Port[]? Ports { get; set; }
        public Labels? Labels { get; set; }
        public string State { get; set; }
        public string Status { get; set; }
        public Hostconfig? HostConfig { get; set; }
        public Networksettings? NetworkSettings { get; set; }
        public Mount[]? Mounts { get; set; }
    }
    public class Labels
    {
        public string comdockercomposeconfighash { get; set; }
        public string comdockercomposecontainernumber { get; set; }
        public string comdockercomposedepends_on { get; set; }
        public string comdockercomposeimage { get; set; }
        public string comdockercomposeoneoff { get; set; }
        public string comdockercomposeproject { get; set; }
        public string comdockercomposeprojectconfig_files { get; set; }
        public string comdockercomposeprojectworking_dir { get; set; }
        public string comdockercomposeservice { get; set; }
        public string comdockercomposeversion { get; set; }
        public string orgopencontainersimageauthors { get; set; }
        public DateTime orgopencontainersimagecreated { get; set; }
        public string orgopencontainersimagedescription { get; set; }
        public string orgopencontainersimagelicenses { get; set; }
        public string orgopencontainersimagerevision { get; set; }
        public string orgopencontainersimagesource { get; set; }
        public string orgopencontainersimagetitle { get; set; }
        public string orgopencontainersimageurl { get; set; }
        public string orgopencontainersimageversion { get; set; }
    }
    public class Hostconfig
    {
        public string NetworkMode { get; set; }
    }
    public class Networksettings
    {
        public Networks Networks { get; set; }
    }
    public class Networks
    {
        public object IPAMConfig { get; set; }
        public object Links { get; set; }
        public object Aliases { get; set; }
        public string NetworkID { get; set; }
        public string EndpointID { get; set; }
        public string Gateway { get; set; }
        public string IPAddress { get; set; }
        public int IPPrefixLen { get; set; }
        public string IPv6Gateway { get; set; }
        public string GlobalIPv6Address { get; set; }
        public int GlobalIPv6PrefixLen { get; set; }
        public string MacAddress { get; set; }
        public object DriverOpts { get; set; }
    }
    public class Mount
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Mode { get; set; }
        public bool RW { get; set; }
        public string Propagation { get; set; }
    }
}
