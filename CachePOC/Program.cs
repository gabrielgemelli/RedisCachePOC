using System;

namespace CachePOC
{
    class Program
    {
        static void Main(string[] args)
        {
            InicializaObjetosPadrao();

            long id = 1;

            EscreveCategoriaDoProduto(id);

            EscreveFabricanteDoProduto(id);

            CriaCategoria("Nova_Categoria1");

            CriaFabricante("Novo_Fabricante1");

            EscreveCategoriaDoProduto(id);

            EscreveFabricanteDoProduto(id);

            Console.ReadKey();
        }

        private static void InicializaObjetosPadrao()
        {
            var categoria = CriaCategoria("Categoria1");

            var fabricante = CriaFabricante("Fabricante1");

            CriaProduto("Produto1", categoria, fabricante);
        }

        private static Categoria CriaCategoria(string nome)
        {
            long id = 1;

            //POCMemoryCache.Instance.Remove<Categoria>(id);
            POCRedisCache.Instance.Remove<Categoria>(id);

            var categoria = new Categoria() { Id = id, Nome = nome };

            //POCMemoryCache.Instance.Add(categoria, categoria.Id);
            POCRedisCache.Instance.Add(categoria, categoria.Id);

            return categoria;
        }

        private static void Update()
        {

        }

        private static Fabricante CriaFabricante(string nome)
        {
            long id = 1;

            //POCMemoryCache.Instance.Remove<Fabricante>(id);
            POCRedisCache.Instance.Remove<Fabricante>(id);

            var fabricante = new Fabricante() { Id = id, Nome = nome };

            //POCMemoryCache.Instance.Add(fabricante, fabricante.Id);
            POCRedisCache.Instance.Add(fabricante, fabricante.Id);

            return fabricante;
        }

        private static Produto CriaProduto(string nome, Categoria categoria, Fabricante fabricante)
        {
            long id = 1;

            //POCMemoryCache.Instance.Remove<Produto>(id);
            POCRedisCache.Instance.Remove<Produto>(id);

            var produto = new Produto() { Id = id, Nome = nome, Fabricante = fabricante, Categoria = categoria };

            //POCMemoryCache.Instance.Add(produto, produto.Id);
            POCRedisCache.Instance.Add(produto, produto.Id);

            return produto;
        }

        private static Categoria CarregaCategoria(long id)
        {
            //return POCMemoryCache.Instance.Get<Categoria>(id);
            return POCRedisCache.Instance.Get<Categoria>(id);
        }

        private static Fabricante CarregaFabricante(long id)
        {
            //return POCMemoryCache.Instance.Get<Fabricante>(id);
            return POCRedisCache.Instance.Get<Fabricante>(id);
        }

        private static Produto CarregaProduto(long id)
        {
            //return POCMemoryCache.Instance.Get<Produto>(id);
            return POCRedisCache.Instance.Get<Produto>(id);

            //var produto = POCMemoryCache.Instance.Get<Produto>(id);

            //if (produto != null)
            //{
            //    var categoria = CarregaCategoria(produto.Categoria.Id);

            //    if (categoria != null)
            //    {
            //        produto.Categoria = categoria;
            //    }

            //    var fabricante = CarregaFabricante(produto.Fabricante.Id);

            //    if (fabricante != null)
            //    {
            //        produto.Fabricante = fabricante;
            //    }
            //}

            //return produto;
        }

        private static void EscreveFabricanteDoProduto(long id)
        {
            var produto = CarregaProduto(id);

            Console.WriteLine("Fabricante do produto 1: " + produto.Fabricante.Nome);
        }

        private static void EscreveCategoriaDoProduto(long id)
        {
            var produto = CarregaProduto(id);

            Console.WriteLine("Categoria do produto 1: " + produto.Categoria.Nome);
        }
    }
}
