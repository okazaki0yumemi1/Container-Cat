using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Container_Cat.Containers.EngineAPI.Models;

namespace Container_Cat.Containers.Models
{
    public class BaseContainer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string objId { get; set; }
        
        [ForeignKey("DockerContainerModel")]
        public string Id { get; set; } = "";
        public string State { get; set; }
        public string? Name { get; set; }
        public string Image { get; set; }
        public List<Port>? Ports { get; set; } = new List<Port>();
        public List<Mount>? Mounts { get; set; } = new List<Mount>();

        public BaseContainer() { }

        public bool Equals(BaseContainer? obj)
        {
            if ((Id == obj.Id) && (Image == obj.Image))
                return true; // base.Equals(obj);
            else
                return false;
        }
        //Convert Docker to Base:
        public BaseContainer (DockerContainer dockerContainer)
        {
            Id = dockerContainer.Id;
            Name = dockerContainer.Name;
            State = dockerContainer.State;
            Image = dockerContainer.Image;
            foreach (var mountPoint in dockerContainer.Mounts)
            {
                Containers.Models.Mount mount = new Containers.Models.Mount();
                mount.Source = mountPoint.Source;
                mount.Destination = mountPoint.Destination;
                mount.Type = mountPoint.Type;
                mount.RW = mountPoint.RW;
                Mounts.Add(mount);
            }
            foreach (var containerPort in dockerContainer.Ports)
            {
                Containers.Models.Port port = new Containers.Models.Port();
                port.PrivatePort = containerPort.PrivatePort;
                port.PublicPort = containerPort.PublicPort;
                port.IP = containerPort.IP;
                port.Type = containerPort.Type;
                Ports.Add(port);
            }
        }
    }

    public class Port
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } // = "";
        public string? IP { get; set; }
        public int PrivatePort { get; set; }
        public int PublicPort { get; set; }
        public string? Type { get; set; }

        public Port() { }
    }

    public class Mount
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } // = "";
        public string? Type { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public bool? RW { get; set; }

        public Mount() { }
    }
}
