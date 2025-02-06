namespace Core.Entities
{
    public class SubCategory : BaseEntity
    {
        public string Name { get; set; }

        public int CategoryId { get; set; }
    }
}
