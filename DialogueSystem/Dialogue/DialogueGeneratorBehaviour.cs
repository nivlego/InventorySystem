using KeyStore;
using UnityEngine;

namespace DialogueSystem.Dialogue
{
    public class DialogueGeneratorBehaviour : MonoBehaviour
    {
        public Dialogue d;
        public bool active;
        public bool save;
        private Dialogue oldD;
        public string fileName;
        
        // Start is called before the first frame update
        void Update()
        {
            if (save)
            {
                if (d != oldD)
                {
                    d.Save(fileName);
                }

                oldD = d;
            }
            KeyStoreHandler ksh = KeyStoreHandler.Default();
            ksh.Set("dialogue:active", active);
        }
    }
}
