using System;
using UnityEngine;
using KeyStore;
using InventorySystem;
using JSON;

/**
We only want to call these when reffering to money object, or to get the price of an item.

**/
namespace JSON
{
    public class MoneyHandler : ItemLoader
    {
        public class CountingCoin
        {
                public static int DisplayPrice(string folder, string filename) //Price of an Item
                {
                    //displaying price of an item
                    Item k = Item.Load(folder, filename);
                    return k.price;
                    //return JsonUtility.FromJson<Item>(j);//.price ;
                }
                public static int IncreaseMoney(string folder, string filename, int amount) //Money++;
                {
                    Item k = Item.Load(folder, filename);
                    k.numberOf += amount;
                    return k.numberOf;
                    //get money stored
                    //price += 1;
                    //return JsonUtility.FromJson<Item>(j);
                }
                public static int DecreaseMoney(string folder, string filename, int amount)//Money--;
                {
                    Item k = Item.Load(folder, filename);
                    if (k.numberOf !>= 1)
                    {
                        k.numberOf -= amount;
                    }
                    else 
                    {
                        k.numberOf = 0;
                    }
                    return k.numberOf;
                    //get money stored
                    //price -= 1;
                    //return JsonUtility.FromJson<Item>(j);
                }
        }
    }
}
