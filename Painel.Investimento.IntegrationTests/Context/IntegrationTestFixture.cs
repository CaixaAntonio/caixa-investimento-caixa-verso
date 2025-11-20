using Microsoft.EntityFrameworkCore;
using Painel.investimento.Infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painel.Investimento.IntegrationTests.Context
{
    public class IntegrationTestFixture : IDisposable
    {
        public AppDbContext Context { get; }

        public IntegrationTestFixture()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("Filename=:memory:")
                .Options;

            Context = new AppDbContext(options);
            Context.Database.OpenConnection();
            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            Context.Database.CloseConnection();
            Context.Dispose();
        }
    }

}
