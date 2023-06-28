using Microsoft.EntityFrameworkCore;
using Container_Cat.Containers.Models;
using Container_Cat.Utilities.Models;
using Container_Cat.Containers.EngineAPI.Models;

namespace Container_Cat.Data
{
    public class ContainerCatContext : DbContext
    {
        public ContainerCatContext(DbContextOptions<ContainerCatContext> options)
            : base(options)
        {

        }

        public DbSet<BaseContainer> BaseContainer { get; set; } =
            default!;
        public DbSet<Port> Port { get; set; } = default!;
        public DbSet<Containers.Models.Mount> Mount { get; set; } = default!;
        public DbSet<SystemDataObj> SystemDataObj { get; set; } = default!;
        public DbSet<HostAddress> HostAddress { get; set; } = default!;
        public DbSet<HostSystem<DockerContainer>> DockerHost { get; set; } = default!;
        public DbSet<DockerContainer> DockerContainers { get; set; } = default!;
    }
}
