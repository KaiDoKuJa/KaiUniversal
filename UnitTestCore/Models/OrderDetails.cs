namespace UnitTestCore.Models {
    public partial class OrderDetails {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public double? UnitPrice { get; set; }
        public long? Quantity { get; set; }
        public double? Discount { get; set; }
    }
}
