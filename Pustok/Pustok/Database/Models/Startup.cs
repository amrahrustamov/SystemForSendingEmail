 using System.Configuration;
 using Microsoft.Extensions.Configuration;
 using Microsoft.Extensions.DependencyInjection;

namespace Pustok.Database.Models
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));
        }
    }
        public class SmtpSettings
        {
            public string Server { get; set; }
            public int Port { get; set; }
            public bool UseSsl { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }



