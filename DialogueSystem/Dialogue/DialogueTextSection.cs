using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Dialogue
{
    [Serializable]
    public class DialogueTextSection
    {
        public String text;
        public double delay;
        public List<DialogueAction> actions;
        private static float _textDelay = 0.02f;
        private int _charsToDisplay;
        private float _timer;
        public DialogueTextSection(string text, float delay)
        {
            this.text = text;
            this.delay = delay;
            this.actions = actions;
            this._charsToDisplay = 0;
        }
        public DialogueTextSection(string text)
        {
            this.text = text;
            this.delay = 0;
        }

        public String GetText()
        {
            return this.text;
        }

        private void AdvanceStringUntilNotInTag()
        {
            if (text.ToCharArray()[_charsToDisplay] == '<')
            {
                // we now want to loop until we find a closing angle bracket
                // this allows us to hide angle tags in our text
                bool currentCharValid = true;
                while (currentCharValid)
                {
                    _charsToDisplay += 1;
                    currentCharValid = text.ToCharArray()[_charsToDisplay] != '>';
                }
            }
        }

        public String GetTextForTextBox()
        {
            _timer += Time.deltaTime;
            // we run this before AND after, just because at low framerates / high text speeds
            // we might be in a situation where as soon as we start displaying this text, we jump over the initial character
            // so, we just check twice
            // TODO: Make this slightly more elegant / performant.
            AdvanceStringUntilNotInTag();
            // if the timer is higher than the text delay
            // we want to increase the chars to display
            if (_timer >= _textDelay)
            {
                _charsToDisplay += 1;
                _timer = 0;
                _charsToDisplay = Math.Min(_charsToDisplay, text.Length-1);
            }
            AdvanceStringUntilNotInTag();
            try
            {
                return text.Substring(0, _charsToDisplay + 1);
            }
            catch (ArgumentOutOfRangeException)
            {
                Debug.LogError("ArgumentOutOfRange when formatting text!\nText : " + text + "\nCTD: " + _charsToDisplay);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return "";
        }

        public bool Finished()
        {
            return _charsToDisplay + 1 >= text.Length;
        }

        public double GetDelay()
        {
            return this.delay;
        }
    }
}