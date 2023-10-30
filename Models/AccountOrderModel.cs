namespace ShopQuanAo.Models
{
    public class AccountOrderModel
    {
        public Account? account { get; set; }
        public List<Order>? orders { get; set; }
        public List<Product>? products { get; set; }
        public List<Orderdetail>? orderdetails { get; set; }
    }
}
