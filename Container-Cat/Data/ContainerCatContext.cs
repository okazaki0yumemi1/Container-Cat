using Microsoft.EntityFrameworkCore;
using Container_Cat.Containers.Models;
using Container_Cat.Utilities.Models;
using Container_Cat.Containers.EngineAPI.Models;
using Container_Cat.PodmanAPI.Models;

namespace Container_Cat.Data
{
    public class ContainerCatContext : DbContext
    {
        public ContainerCatContext(DbContextOptions<ContainerCatContext> options)
            : base(options)
        {

        }

        public DbSet<BaseContainer> BaseContainers { get; set; } = default!; 
        public DbSet<Containers.Models.Port> Ports { get; set; } = default!;
        public DbSet<Containers.Models.Mount> Mounts { get; set; } = default!;
        public DbSet<HostSystem<BaseContainer>> HostSystemBase { get; set; } = default!;
        public DbSet<HostAddress> HostAddresses { get; set; } = default!;
        //public DbSet<DockerContainer> DockerContainers {get; set;} = default!;
    }
}
