using System;

namespace KeyStore
{
    public interface IKeyStore
    {
        public string GetKeyStoreName();
        public dynamic GetKey(string key);
        public void SetKey(string key, dynamic value);
    }
}