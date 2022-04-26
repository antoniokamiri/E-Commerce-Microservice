namespace Shopping.Aggregator.Models
{
    public class BasketItemExtendedModel
    {
        public int Quatity { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }

        // product related category fileds 

        public string Category { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string ImageFile { get; set; }
    }
}