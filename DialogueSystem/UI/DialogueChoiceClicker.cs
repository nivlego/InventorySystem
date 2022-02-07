using System;
using System.Collections.Generic;
using DialogueSystem.Dialogue;
using KeyStore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem.UI
{
    public class DialogueChoiceClicker : MonoBehaviour
    {
        public List<Button> buttons;
        private void Start()
        {
            for (var i = 0; i < buttons.Count; i++)
            {
                var btn = buttons[i];
                var i1 = i;
                btn.onClick.AddListener(() => HandleButtonClick(i1));
            }
        }
        
        private void Update()
        {
            // we want to A: check if our choices... exist?
            List<DialoguePlayerChoice> choices = KeyStoreHandler.Default().Get("dialogue", "choices");
            if (choices == null)
            {
                return;
            }

            try
            {
                for (int i = 0; i < choices.Count; i++)
                {
                    DialoguePlayerChoice choice = choices[i];
                    Transform btnTransform = buttons[i].transform;
                    btnTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = choice.GetChoiceText();
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                // do nothing?
                //TODO: Fix this.
            }
        }

        void HandleButtonClick(int buttonId)
        {
            Debug.Log("Button " + buttonId + " clicked!");
            List<DialoguePlayerChoice> choices = KeyStoreHandler.Default().Get("dialogue", "choices");
            Debug.Log(choices);
            Debug.Log(choices.Count);
            if (buttonId >= choices.Count)
            {
                Debug.LogWarning("uh heya that button shouldn't be on right now - we don't have enough choices for it");
                return;
            }

            if (choices[buttonId].CanShow().Item1)
            {
                choices[buttonId].SetSelected(true);
                KeyStoreHandler.Default().Set("dialogue", "choices", choices);
            }
            else
            {
                Debug.LogWarning("button should be off - choice should not be available. " + choices[buttonId].choiceText);
            }
        }
    }
}
