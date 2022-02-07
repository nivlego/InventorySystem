using System;
using System.Collections.Generic;
using System.Dynamic;
using DialogueSystem.UI;
using JSON;
using KeyStore;
using UnityEngine;

namespace DialogueSystem.Dialogue
{
    [Serializable]
    public class Dialogue
    {
        public String actor; // the actor name - this references the actor's file
        private Actor.Actor _actorObj;
        public List<DialogueTextSection> sections;
        public List<DialoguePlayerChoice> choices;
        public DialoguePlayerChoice defaultNextDialogue;
        private bool _active;
        private float _timeNotChangingText;
        private bool _finishedDialogue;
        private bool _ended;
        private Dialogue _currentChild;
        private KeyStoreHandler _ksh;

        public Dialogue(String actor, List<DialogueTextSection> sections, List<DialoguePlayerChoice> choices, DialoguePlayerChoice defaultNextDialogue)
        {
            this._actorObj = Actor.Actor.Load(actor);
            this.sections = sections;
            this.choices = choices;
            this.defaultNextDialogue = defaultNextDialogue;
            this._active = false;
            this._timeNotChangingText = 0f;
            _ended = false;
        }
        
        private String CompileDialogString()
        {
            // Handle compiling the text into a single string
            // works on timing
            string compiled = "";
            float t = _timeNotChangingText;
            bool hasFinished = true;
            _finishedDialogue = false;
            for (int i = 0; i < sections.Count; i++)
            {
                DialogueTextSection section = sections[i];
                if (t > section.GetDelay())
                {
                    t -= (float) section.GetDelay();
                    string s = section.GetTextForTextBox();
                    compiled += s;
                    hasFinished = section.Finished() && hasFinished;
                    if (!hasFinished)
                    {
                        // this means this section didn't finish
                        // as such, we want to break out of the loop here
                        break;
                    }

                    else if (i == sections.Count - 1)
                    {
                        // this means we've reached the end of sections
                        // and, as such, we want to indicate that we've done that
                        _finishedDialogue = true;
                    }
                }
                else
                {
                    // if we don't have enough time in the buffer
                    // we want to break so we don't find the NEXT dialogue section with a suitable buffer
                    break;
                }
            }


            if (hasFinished)
            {
                this._timeNotChangingText += Time.deltaTime;
            }
            return compiled;
        }

        public void Save(string filePath)
        {
            JSON.DialogueLoader.Save(this, "Dialogue", filePath);
        }

        public static Dialogue Load(string filePath)
        {
            return DialogueLoader.Load("Dialogue", filePath);
        }

        public void ReloadActor()
        {
            _actorObj = Actor.Actor.Load(this.actor);
        }

        public void Update()
        {
            // If we have a child, let it do it's thing
            // We do this first for performance
            if (_currentChild != null)
            {
                _currentChild.Update();
                return;
            }
            
            // Now we know WE'RE the One True Dialogue (hopefully), we can make sure we have a KSH
            if (_ksh == null)
            {
                _ksh = KeyStoreHandler.Default();
            }
            
            // Now we have our keystore, we check if we're not active
            if (!_active)
            {
                // If this has been marked as ended, we can't do anything, so the false _active is correct
                // but! if it isn't, we're... not ended? but, not active? but something is calling our Update?
                // That doesn't sound right.
                // So! Let's do this.
                if (!_ended)
                {
                    _active = true;
                    Update();
                    return;
                }
            }
            // Oh! We are active. Wonderful!
            else
            {
                var text = CompileDialogString();
                _ksh.Set("dialogue", "line", text);
                _ksh.Set("dialogue", "actor", _actorObj);
                _ksh.Set("dialogue", "defaultContinue", false);
                if (_finishedDialogue)
                {
                    HandleSelectionOfNextDialogue();
                }
                else
                {
                    _ksh.Set("dialogue", "choices", null);
                }
            }
            _ksh.Set("dialogue", "active", _active);
        }

        private void HandleSelectionOfNextDialogue()
        {
            // TODO: Allow for selection of options
                    
            // Expected functionality: we want to first check *if* there are any valid choices.
            // If this is true, we go down that branch of displaying and rendering choices (including non.options)
            // If this is false, we go down the default default next dialogue branch
                    
            // Default next dialogue branch
            var c1 = _ksh.Get("dialogue", "choices");
            if (c1 != null)
            {
                choices = c1;
            }
            var hasChoicesToShow = false;
            foreach (var choice in choices)
            {
                var (canShow, _) = choice.CanShow();
                if (canShow)
                {
                    hasChoicesToShow = true;
                }

                if (choice.GetSelected())
                {
                    _currentChild = choice.GetDialogue();
                    return;
                }
            }

            if (hasChoicesToShow && c1 == null)
            {
                _ksh.Set("dialogue", "choices", choices);
                // We're done here! We're done! Nothing else to do! All done!
                // TODO: Check the selected option that is set in the keystore.
                return;
            }
            if (Input.GetKeyDown("return"))
            {
                if (defaultNextDialogue.exists())
                {
                    _currentChild = defaultNextDialogue.GetDialogue();
                }
                else
                {
                    _active = false;
                    _ended = true;
                }
                // We're done on this frame now!
                return;
            }
            // So. Now we're at a point where:
                // - the enter key hasn't been pressed
                // - we don't have choices to show the user
            
            // As such, we need to set that defaultContinue is being used!
            _ksh.Set("dialogue", "defaultContinue", true);
        }
    }
}