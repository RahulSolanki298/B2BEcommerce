namespace Core.Entities
{
    public class ProductImages : BaseEntity
    {
       public string ProductId { get; set; }

        public int? ImageLgId { get; set; }

        public int? ImageMdId { get; set; }

        public int? ImageSmId { get; set; }

        public int? VideoId { get; set; }

        public bool IsDefault { get; set; }

    }
}
