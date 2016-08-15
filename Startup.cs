using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Data.Sqlite;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


using net_assignment.Models;

namespace net_assignment
{
    public class Startup
    {
        private string dbFile = Path.GetTempPath() + "net_assignment.sqlite";

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(opts =>
                {
                    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    opts.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            services.AddOptions();
            services.Configure<DBOptions>(opts =>
            {
                opts.ConnectionString = "Data Source=" + dbFile;
            });

            services.AddSingleton<IRepository<Contact, long>, ContactRepository>();
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (!File.Exists(dbFile))
            {
                using (var c = new SqliteConnection("Data Source=" + dbFile))
                {
                    c.Open();
                    var i = c.Execute(@"CREATE TABLE Contact(
                        Id          integer primary key AUTOINCREMENT,
                        FirstName   text not null,
                        LastName    text not null,
                        Email       text not null,
                        Phone       text,
                        City        text

                    )");
                    System.Console.WriteLine("New DB created");
                }
            }
            System.Console.WriteLine("DB file location: " + dbFile);

            loggerFactory.AddConsole();
            app.UseMvc();
        }
    }
}
