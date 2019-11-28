using BBCollection.BBObjects;
using BBCollection.DBConncetion;
using BBCollection.DBHandling;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BBCollection
{
    public class DatabaseConnect
    {
        public DatabaseHandling Initialize = new DatabaseHandling();
        public ProductHandling Product = new ProductHandling();
        public RecipeHandling Recipe = new RecipeHandling();
        public ShoppinglistHandling Shoppinglist = new ShoppinglistHandling();
        public StorageHandling Storage = new StorageHandling();
        public UserHandling User = new UserHandling();
    }
}
