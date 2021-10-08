using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Common
{
    public class ItemGame
    {
        public string nameItem { get; set; }
        public string counterItem { get; set; }
        public string priceItem { get; set; }
    }
    public class IteminGame
    {
        public static List<ItemGame> listItemSeed;
        public static List<ItemGame> listItemMachine;
        public static List<ItemGame> listItemProduct;
    }
}
