using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Container_Cat.Containers.Models;
using Container_Cat.Utilities.Models;

namespace Container_Cat.Data
{
    public class ContainerCatContext : DbContext
    {
        public ContainerCatContext(DbContextOptions<ContainerCatContext> options)
            : base(options)
        {
            this.Database.Migrate();
            this.Database.EnsureCreated();
        }

        public DbSet<BaseContainer> BaseContainer { get; set; } =
            default!;
        public DbSet<Port> Port { get; set; } = default!;
        public DbSet<Mount> Mount { get; set; } = default!;
        public DbSet<SystemDataObj> SystemDataObj { get; set; } = default!;
        public DbSet<HostAddress> HostAddress { get; set; } = default!;
    }
}
