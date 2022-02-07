using System;
using UnityEngine;
using InventorySystem;
using KeyStore;
using JSON;



namespace InventorySystem
{
    public class ItemHandler : MoneyHandler
    {
        private static ItemHandler ItemMoney(string folder, string filename)
        {
            void itemPrice(string folder, string filename) //////////These are what will be used for money item
            {
                Item ItemA = new Item(folder, filename);
                ItemA.price = MoneyHandler.CountingCoin.DisplayPrice(folder, filename);
            }   
            void AddMoney(string folder, string filename, int amount)
            {
                int ItemB = new int();
                MoneyHandler.CountingCoin.IncreaseMoney(folder, filename, amount);
                //ItemB = MoneyHandler.CountingCoin.IncreaseMoney(folder, filename, amount);
            }
            void TakeMoney(string folder, string filename, int amount)
            {
                int ItemB = new int();
                MoneyHandler.CountingCoin.DecreaseMoney(folder, filename, amount);
                //ItemB = MoneyHandler.CountingCoin.DecreaseMoney(folder, filename, amount);
            }

            string AddItemToStack(string folder, string filename) //////////These are what will be used for items 
            {
                Item stackee = Load(folder, filename);
                string j;
                if (stackee.isStackable == true) {
                    stackee.numberOf += 1;
                    j = "Number of items = " + stackee.numberOf;
                } 
                else {
                    j = "Item is not Stackable.";
                }
                return j;
            }
            string RemoveItemFromStack(string folder, string filename)
            {
                Item stackee = Load(folder, filename);
                string j = "";
                if (stackee.isStackable == true && stackee.numberOf !>= 1) {
                    stackee.numberOf -= 1;
                    j = "Number of items = " + stackee.numberOf;
                } 
                else if (stackee.numberOf == 0 ){
                    NoItemInStack(folder, filename);
                }
                else
                {
                    j = "Item is not stackable.";
                }
                return j;
                // If numberOf >= 1,  numberOf -= 1, else numberOf = 0;
                //return j
            }
            string NoItemInStack(string folder, string filename)
            {
                Item stackee = Load(folder, filename);
                string j;
                j = "There are " + stackee.numberOf + " " + filename + "Items left.";
    
               return j;
                
            }
            void DisplayIsBounded(string folder, string filename)
            {
                //Load(folder, filename);
                //return JsonUtility.FromJson<Item>(j);//.isBounded;
            }

            /**
            this.filename = a.filename;
            this.name = a.name;
            this.memory = a.memory;
            this.isStackable = a.isStackable;
            this.numberOf = a.numberOf;
            this.isPurchasable = a.isPurchasable;
            this.price = a.price;
            this.boundRadius = a.boundRadius;
            this.isBounded = a.isBounded;
            **/

        }

    }
}