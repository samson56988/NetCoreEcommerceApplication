﻿using Ecommerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductType> ProductTypes { get; set; }

        public DbSet<SpecialTags> SpecialTags { get; set; }


        public DbSet<Product> Product { get; set; } 
            }
}
