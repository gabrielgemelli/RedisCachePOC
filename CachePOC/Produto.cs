
namespace CachePOC
{
    public class Produto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public Categoria Categoria { get; set; }
        public Fabricante Fabricante { get; set; }
    }
}
