namespace BBCollection.BBObjects
{
    public class Product
    {
        public string _id;
        public string _productName;
        public string _amount;
        public double _price;
        public string _image;
        public string _storeName;
        public string _CustomReferenceField;

        public string _customname;
        public int _amountleft; //Kan navngives anderledes
        public string _timeAdded;
        public string _state;

        public Product(string id, string productName, string amount, double price, string image, string storeName, int amountleft, string customField)
        {
            _id = id;
            _productName = productName;
            _amount = amount;
            _price = price;
            _image = image;
            _storeName = storeName;
            _amountleft = amountleft;
            _CustomReferenceField = customField;
        }

        public Product(string id, string productName, string amount, double price, string image, string storeName, int amountleft)
        {
            _id = id;
            _productName = productName;
            _amount = amount;
            _price = price;
            _image = image;
            _storeName = storeName;
            _amountleft = amountleft;
        }

        public Product(string id, string productName, string amount, double price, string image, string storeName, string customField)
        {
            _id = id;
            _productName = productName;
            _amount = amount;
            _price = price;
            _image = image;
            _storeName = storeName;
            _CustomReferenceField = customField;
        }

        public Product(string id, string productName, string amount, double price, string image, string storeName)
        {
            _id = id;
            _productName = productName;
            _amount = amount;
            _price = price;
            _image = image;
            _storeName = storeName;
        }

        public Product(string id, string productName, string amount,
                       double price, string image, string storename,
                       int amountleft, string timeAdded, string state, string customField)
        {
            _id = id;
            _productName = productName;
            _amount = amount;
            _price = price;
            _image = image;
            _storeName = storename;
            _amountleft = amountleft;
            _timeAdded = timeAdded;
            _state = state;
            _CustomReferenceField = customField;
        }

        public Product(string id, string customname, int amountleft, string timeadded, string state)
        {
            _id = id;
            _customname = customname;
            _amountleft = amountleft;
            _timeAdded = timeadded;
            _state = state;
        }

        public Product(string id, string customname, int amountleft, string state)
        {
            _id = id;
            _customname = customname;
            _amountleft = amountleft;
            _state = state;
        }

        public Product() { }

    }
}
