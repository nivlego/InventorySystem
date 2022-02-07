using System;
using InventorySystem;
using UnityEngine;

namespace JSON
{
    public class ItemLoader: Item
    {
        public static Item Load(String folder, String filename)
        {
            string j = ObjectLoader.GetJsonString(folder, filename);
            return JsonUtility.FromJson<Item>(j);
        }

        public static Item Save(Item I, String folder, String filename)
        {
            //havent done this part yet
            string j = ObjectLoader.GetFsPath(folder, filename);
            return JsonUtility.FromJson<Item>(j);
        }
    }
}