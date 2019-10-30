using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace B3_BiksBudget.StoreApi.CoopApi
{
    class CoopTestShoppinglist
    {
        public List<CoopProduct> coopProducts = new List<CoopProduct>
        {
            new CoopProduct { Ean = "1000001", Navn = "Kyllingebryst",  Navn2 = "400g",   Pris = 21.95, VareHierakiId = 2503 },
            new CoopProduct { Ean = "1000004", Navn = "Chokoladekage",  Navn2 = "1000g",  Pris = 9.99,  VareHierakiId = 2501 },
            new CoopProduct { Ean = "1000003", Navn = "Kyllinge pålæg", Navn2 = "80g",    Pris = 15.00, VareHierakiId = 02504 },
            new CoopProduct { Ean = "1000002", Navn = "Chokiladebar",   Navn2 = "50g",    Pris = 2.99,  VareHierakiId = 2502 }
        };

        public string TurnJsonToString()
        {
            string result = "";

            foreach (var item in coopProducts)
            {
                result += JsonConvert.SerializeObject(item).ToString();
            }

            return result;
        }
    }
}
