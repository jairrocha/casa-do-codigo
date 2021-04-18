using CasaDoCodigo.Models;
using CasaDoCodigo.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace CasaDoCodigo
{
    class DataService : IDataService
    {
        private readonly ApplicationContext contexto;
        private readonly IProdutoRepository produtoRepository;

        public DataService(ApplicationContext contexto, 
                IProdutoRepository produtoRepository)
        {
            this.contexto = contexto;
            this.produtoRepository = produtoRepository;
        }

        public void InicializaDB()
        {

            /* O método abaixo tbm cria o BD, porém não é remomendávável
             * devído não utilizar o arquivo de migrations, ou seja, e se
             * baseia no modelo. O problema disso é uma vez que utiliza ele,
             * vc não pode aplicar nenhuma migração ao seu projeto. Utilize
             * o '.Migrate()' pois ele sim utiliza os arquivo de migração.
             */

            //contexto.Database.EnsureCreated();
            contexto.Database.Migrate();


            //Lendo arquivo json
            var livros = GetLivros();

            produtoRepository.SaveProdutos(livros);
        }



        private static List<Livro> GetLivros()
        {
            var json = File.ReadAllText("livros.json");
            var livros = JsonConvert.DeserializeObject<List<Livro>>(json);
            return livros;
        }
    }


}
