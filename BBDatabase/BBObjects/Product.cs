namespace BBCollection.BBObjects
{
    public class Product
    {
        public string _ean { get; }
        public string _name { get; }
        public string _name2 { get; }
        public double _price { get; }
        public int _productHierarchyID { get; }

        public Product(string ean, string name, string name2, double price, int productHierachyID)
        {
            _ean = ean;
            _name = name;
            _name2 = name2;
            _price = price;
            _productHierarchyID = productHierachyID;
        }
    }
}
