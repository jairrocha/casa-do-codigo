using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CasaDoCodigo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*
             * Esse método serve para adicionar o serviços
             */

            services.AddMvc();

            //Obtendo connection string definida no 'appsettings.json'
            string connectionString = 
                            Configuration.GetConnectionString("Default");

            //Adicionando o serviço de context a aplicação
            services.AddDbContext<ApplicationContext>(options => 
                     options.UseSqlServer(connectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IServiceProvider serviceProvider)
        {

            /*
             * Esse método define os serviço que vamos utilizar
             */

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Pedido}/{action=Carrossel}/{id?}");
            });

            /*
            * O método abaixo garante que o banco esteja criado, ou seja 
            * se o banco não existir o método criar o BD com base nos 
            * arquivos de Migrations.
            */
            
            serviceProvider
                .GetService<ApplicationContext>()
                .Database
                .Migrate();

            /* O método abaixo tbm cria o BD, porém não é remomendávável
             * devído não utilizar o arquivo de migrations, ou seja, e se
             * baseia no modelo. O problema disso é uma vez que utiliza ele,
             * vc não pode aplicar nenhuma migração ao seu projeto. Utilize
             * o '.Migrate()' pois ele sim utiliza os arquivo de migração.
             */

            //.EnsureCreated(); 
        }
    }
}
