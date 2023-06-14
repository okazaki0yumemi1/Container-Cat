using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Container_Cat.Containers.Models;

namespace Container_Cat.Data
{
    public class ContainerCatContext : DbContext
    {
        public ContainerCatContext (DbContextOptions<ContainerCatContext> options)
            : base(options)
        {
            this.Database.Migrate();
            this.Database.EnsureCreated();
        }

        public DbSet<Container_Cat.Containers.Models.BaseContainer> BaseContainer { get; set; } = default!;
        public DbSet<Container_Cat.Containers.Models.Port> Port { get; set; } = default!;
        public DbSet<Container_Cat.Containers.Models.Mount> Mount { get; set; } = default!;

        public DbSet<Container_Cat.Utilities.Models.SystemDataObj> SystemDataObj { get; set; } = default!;
        public DbSet<Container_Cat.Utilities.Models.HostAddress> HostAddress { get; set; } = default!;
    }
}
