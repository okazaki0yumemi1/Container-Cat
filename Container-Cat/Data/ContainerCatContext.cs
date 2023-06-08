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
        }

        public DbSet<Container_Cat.Containers.Models.BaseContainer> BaseContainer { get; set; } = default!;
        public DbSet<Container_Cat.Utilities.Models.SystemDataObj> SystemDataObj { get; set; } = default!;
    }
}
