using System;
using UnityEngine;

namespace KeyStore
{
    public class GenericKeyStore : IKeyStore
    {
        protected string _name;

        public GenericKeyStore()
        {
            _name = "generic";
        }

        public string GetKeyStoreName()
        {
            return _name;
        }
        public dynamic GetKey(String key)
        {
            Debug.LogWarning("attempted to get key using generic : source : " + GetKeyStoreName());
            return null;
        }

        public void SetKey(String key, dynamic value)
        {
            Debug.LogWarning("attempted to set key using generic : source : " + GetKeyStoreName());
            // do nothing
        }
    }
}