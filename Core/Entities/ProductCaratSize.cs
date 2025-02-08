namespace Core.Entities
{
    public class ProductCaratSize : BaseEntity
    {
        public int CaratId { get; set; }

        public int ShapeId { get; set; }

        public string Name { get; set; }
    }
}
