namespace CachePOC.Models
{
    public class DietUnit
    {
        public long Id { get; set; }
        public long DietId { get; set; }
        public long UnitId { get; set; }
        public Unit Unit { get; set; }
    }
}