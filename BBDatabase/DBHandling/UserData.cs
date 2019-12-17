using System;
using System.Collections.Generic;
using System.Text;
using BBCollection.BBObjects;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.Net;

namespace BBCollection.DBHandling
{
    public class UserData
    {
        public UserData(string _username)
        {
            shoppinglist = new UserShoppinglist(_username);
            storage = new UserStorage(_username);
        }

        public UserData()
        {
            shoppinglist = new UserShoppinglist();
        }

        public UserShoppinglist shoppinglist;
        public UserStorage storage;

        HttpResponseMessage responseOne = new HttpResponseMessage();
        HttpResponseMessage responseTwo = new HttpResponseMessage();
        List<Product> tempList = new List<Product>();

        public async Task<HttpResponseMessage> ProductSLToStrage(Product p)
        {
            responseOne = await storage.AddProduct(p);
            shoppinglist.DeleteItem(p);

            if (OneComplete(responseOne))
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }



        public async Task<HttpResponseMessage> ProductSLToStrage(Product p, int i)
        {
            responseOne = await storage.AddProduct(p, i);
            shoppinglist.DecrementProduct(p);

            if (OneComplete(responseOne))
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        public async Task<HttpResponseMessage> ShoppinglistToStrage()
        {
            responseOne = await storage.AddList(shoppinglist.FindActiveList());
            responseTwo = await shoppinglist.DeleteAndSaveList();

            if (BothComplete())
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        public async Task<HttpResponseMessage> ShoppinglistToStrage(List<Product> sendProducts)
        {
            responseOne = await storage.AddList(sendProducts);
            responseTwo = await shoppinglist.DeleteAndSaveList();

            if (BothComplete())
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        public bool BothComplete()
        {
            return OneComplete(responseOne) && OneComplete(responseTwo);
        }

        public bool OneComplete(HttpResponseMessage response)
        {
            return response.IsSuccessStatusCode;
        }


    }
}
