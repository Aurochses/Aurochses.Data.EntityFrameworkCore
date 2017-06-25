﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aurochses.Data.EntityFrameworkCore.Tests.Fakes
{
    public class FakeEntityTypeConfiguration
        : EntityTypeConfiguration<Entity<int>, int>
    {
        public FakeEntityTypeConfiguration(string schemaName)
            : base(schemaName)
        {

        }

        public override void Map(EntityTypeBuilder<Entity<int>> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("Entity", SchemaName);
        }
    }
}