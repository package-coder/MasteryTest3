namespace MasteryTest3.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string? photo { get; set; }
        public string sku { get; set; }
        public string size { get; set; }
        public string color { get; set; }
        public Category category { get; set; }
        public UOM uom { get; set; }
        public decimal weight { get; set; }
        public decimal price { get; set; }
        public DateTime? dateAdded { get; set; }
        public DateTime? dateModified { get; set; }
        public DateTime? dateDeleted { get; set; }
        public Product() { }

        public Product(string name, string description, string photo, string sku, string size, string color, Category category, decimal weight, decimal price)
        {
            this.name = name;
            this.description = description;
            this.photo = photo;
            this.sku = sku;
            this.size = size;
            this.color = color;
            this.category = category;
            this.weight = weight;
            this.price = price;
        }

        public override string ToString()
        {
            return $"Product: {Id} {name} {description} {photo} {sku} {size} {color} {category.Id} {weight} {price}";
        }
    }
}
