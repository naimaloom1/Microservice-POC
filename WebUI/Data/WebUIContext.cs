using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebUI.Models;

namespace WebUI.Data
{
    public class WebUIContext : DbContext
    {
        public WebUIContext (DbContextOptions<WebUIContext> options)
            : base(options)
        {
        }

        public DbSet<WebUI.Models.User>? User { get; set; }
    }
}
