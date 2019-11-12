namespace BBCollection.BBObjects
{
    public class Product
    {
        public string _id;
        public string _productName;
        public string _amount;
        public int _amountleft;
        public string _timeAdded;
        public double _price;
        public string _image;
        public string _storeName;

        public Product(string id, string productName, string amount, double price, string image, string storeName)
        {
            _id = id;
            _productName = productName;
            _amount = amount;
            _price = price;
            _image = image;
            _storeName = storeName;
        }

        /*public string _ean;
        public string _name;
        public string _name2;
        public double _price;
        public int _productHierarchyID;

        public Product(string ean, string name, string name2, double price, int productHierachyID)
        {
            _ean = ean;
            _name = name;
            _name2 = name2;
            _price = price;
            _productHierarchyID = productHierachyID;
        }*/
    }
}
