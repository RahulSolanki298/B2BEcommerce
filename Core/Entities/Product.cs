namespace Core.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Price { get; set; }
        public int? CaratId { get; set; }
        public int? ClarityId { get; set; }
        public int? ColorId { get; set; }
        public int? ShapeId { get; set; }
        public bool IsActivated { get; set; } = false;
    }
}
