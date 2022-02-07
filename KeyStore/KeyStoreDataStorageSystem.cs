using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using DialogueSystem;
using UnityEngine;

namespace KeyStore
{
    internal class KeyFormatException : Exception
    {
        public KeyFormatException() { }

        public KeyFormatException(string msg) 
            : base($"invalid key format: {msg}")
        {
            
        }
    }
    
    [Serializable]
    public class KeyStoreDataStorageSystem
    {
        /*
         * Store arbitrary data for an actor
         *
         */
        // TODO: Link this to a keystore, so we can retrieve actor data through a keystore
        protected Dictionary<string, dynamic> Data;

        public KeyStoreDataStorageSystem()
        {
            Data = new Dictionary<string, dynamic>();
        }
        public void Set(string key, dynamic value)
        {
            if (Data.TryGetValue(key, out _))
            {
                Data[key] = value;
            }
            else
            {
                Data.Add(key, value);
            }
        }

        public dynamic Get(string key)
        {
            if (Data.TryGetValue(key, out var getVRes))
            {
                return getVRes;
            }

            return null;
        }
    }
}