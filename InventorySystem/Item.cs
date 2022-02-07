using System;
using UnityEngine;
using KeyStore;
using JSON;

namespace InventorySystem
{
    [Serializable] //want to make these values Serializable but need to have a double check on these
    public class Item
    {
        public string filename;
        public string name;
        /**
        Future features to add to item class:
        public int Increase/Decrease_Alignment //with ministry
        public Vector3 coordinates;
        public int 
        public int
        
        **/
        public bool isStackable;
        public int numberOf;
        public bool isPurchasable;
        public int price;
        public int boundRadius;
        public bool isBounded;
        public GameObject prefab;
        public KeyStoreDataStorageSystem memory;

        public Item()
        {
            /**this.filename = name;
            this.name = name;
            this.memory = new KeyStoreDataStorageSystem();
            
            this.isStackable = isStackable;
            this.numberOf = numberOf;
            this.isPurchasable = isPurchasable;
            this.price = price;
            this.boundRadius = boundRadius;
            this.isBounded = isBounded;**/

            
        }
        public Item(string filename, string name)
        {
            this.filename = filename;
            this.name = name;
            this.memory = new KeyStoreDataStorageSystem();
        }
        
        public string GetName()
        {
            return name;
        }

        public void Save()
        {
            JSON.ItemLoader.Save(this, "Items", name);
        }

        private void ReloadFromDisk()
        {
            Item a = Item.Load(name, filename);
            this.filename = a.filename;
            this.name = a.name;
            this.memory = a.memory;
            this.isStackable = a.isStackable;
            this.numberOf = a.numberOf;
            this.isPurchasable = a.isPurchasable;
            this.price = a.price;
            this.boundRadius = a.boundRadius;
            this.isBounded = a.isBounded;
        }

        public static Item Load(string item, string filename)
        {
            return JSON.ItemLoader.Load("Items", item);
        }

        public KeyStoreDataStorageSystem Memory()
        {
            ReloadFromDisk();
            return this.memory;
        }
    }
}
    