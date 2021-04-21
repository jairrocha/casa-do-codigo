using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CasaDoCodigo.Repositories;
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
             * Esse método serve para adicionar o serviços (Injeção de depências)
             */

            services.AddMvc();

            //Obtendo connection string definida no 'appsettings.json'
            string connectionString =
                            Configuration.GetConnectionString("Default");


            //Adicionando o serviço de context a aplicação
            services.AddDbContext<ApplicationContext>(options =>
                     options.UseSqlServer(connectionString));


            services.AddTransient<IDataService, DataService>();

            services.AddTransient<IProdutoRepository, ProdutoRepository>();
            services.AddTransient<IPedidoRepository, PedidoRepository>();
            services.AddTransient<ICadastroRepository, CadastroRepository>();
            services.AddTransient<IItemPedidoRepository, ItemPedidoRepository>();

            //Adicionando o serviço de seção 
            services.AddDistributedMemoryCache();
            services.AddSession();
            

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
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Pedido}/{action=Carrossel}/{codigo?}");
            });

            /*
            * O método abaixo garante que o banco esteja criado, ou seja 
            * se o banco não existir o método cria o BD com base nos 
            * arquivos de Migrations.
            */

            serviceProvider.GetService<IDataService>().InicializaDB();
        }
    }
}
