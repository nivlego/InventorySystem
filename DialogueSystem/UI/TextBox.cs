using System;
using System.Collections.Generic;
using DialogueSystem.Dialogue;
using KeyStore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem.UI
{
    public class TextBox : MonoBehaviour
    {
        public TextMeshProUGUI actorText;
        public TextMeshProUGUI bodyText;
        public Transform actorModelRoot;
        public Image defaultContinueDialoguePrompt;
        public float slideInPosition = 200f;
        public float slideInPositionChoices = 640f;
        public float slideOutPosition = -200f;
        private float _slideHeightWhenStateChanged;
        public float slideSpeed = 3f;
        private KeyStoreHandler _ksh;
        private bool _previousActiveState;
        private float _timeInCurrentActiveState;
        private bool _showingDefaultContinueDialoguePrompt;
        private bool _oldShowingDefaultContinueDialoguePrompt;
        private float _timeSinceStateChange;
        public double opacityChangeSpeed = 180;
        public double minOpacity = 0.7;
        public double fadeTime = 1;
        private bool _previousChoicesState;
        private string lastLoadedActorName;

        private void Start()
        {
            lastLoadedActorName = "";
            _previousActiveState = false;
            _previousChoicesState = false;
            _timeInCurrentActiveState = 0f;
            _showingDefaultContinueDialoguePrompt = false;
            _oldShowingDefaultContinueDialoguePrompt = false;
            _timeSinceStateChange = (float) fadeTime;
            _ksh = KeyStoreHandler.Default();
            _slideHeightWhenStateChanged = slideOutPosition;
        }

        private void Update()
        {
            if (_ksh == null)
            {
                _ksh = KeyStoreHandler.Default();
            }
            _showingDefaultContinueDialoguePrompt = _ksh.Get("dialogue:defaultContinue") && !IsOptionsToBeDisplayed();
            bool active = _ksh.Get("dialogue:active");
            SetTextBoxPositions(_ksh);
            if (active)
            {
                SetBodyText(_ksh.Get("dialogue", "line"));
                SetActorModel(_ksh.Get("dialogue", "actor"));
                SetActorText(_ksh.Get("dialogue", "actor"));
                EndOfDialogueButtonPromptUpdate();
            }
        }

        private bool IsOptionsToBeDisplayed()
        {
            var ksh = KeyStoreHandler.Default();
            List<DialoguePlayerChoice> choices = ksh.Get("dialogue", "choices");
            return choices != null;
        }

        private float TextBoxTargetPosition(bool active)
        {
            // we want to figure out WHERE our target "complete" position is
                // If we're active, we want to slide us in, so it's slideInPosition
                // (ALso, if we have choices, we want to go further in!)
            // If we're inactive, we want to slide us out, so it's 0
            float targetActivePosition;
            if (active)
            {
                if (IsOptionsToBeDisplayed())
                {
                    targetActivePosition = slideInPositionChoices;
                }
                else
                {
                    targetActivePosition = slideInPosition;
                }
            }
            else
            {
                targetActivePosition = slideOutPosition;
            }

            return targetActivePosition;
        }

        private void SetTextBoxPositions(KeyStoreHandler ksh)
        {
            // so we want to FIRST slide us out if we're inactive
            // so we need a target OUT position, and a target IN position
            // our target "out" position should just be 0 pixels - if not, we've goofed it in unity
            // then, we want to calculate the time we've been in the current active state
            bool active = ksh.Get("dialogue:active");
            var panel = gameObject.GetComponent<RectTransform>();
            var position = panel.anchoredPosition;
            
            // Get the target position
            var targetActivePosition = TextBoxTargetPosition(active);

            // Now, we need to figure out if we gotta reset timers
            if (active != _previousActiveState || _previousChoicesState != IsOptionsToBeDisplayed())
            {
                _timeInCurrentActiveState = 0f;
                _slideHeightWhenStateChanged = position.y;
            }
            _timeInCurrentActiveState += Time.deltaTime;
            
            // We now need to calculate the ACTUAL distance we want to move.
            // seeing as the data is in place, we want to create a simple animation curve
            // this curve will start at our slide height when our state changed, and have the target we figured earlier
            var curve = AnimationCurve.EaseInOut(0, _slideHeightWhenStateChanged, 1, targetActivePosition);
            var currentCurveValue = curve.Evaluate(_timeInCurrentActiveState * slideSpeed);

            position = new Vector2(position.x, currentCurveValue);
            panel.anchoredPosition = position;
            _previousActiveState = active;
            _previousChoicesState = IsOptionsToBeDisplayed();
        }

        private void EndOfDialogueButtonPromptUpdate()
        {
            // And now, we multiple the sine value by the fade in value
            var c = new Color(255, 255, 255, EndOfDialogueButtonPromptOpacity());
            try
            {
                defaultContinueDialoguePrompt.color = c;
            }
            catch (NullReferenceException)
            {
                // uh, sometimes this happens?
                // not quite sure why.
                // TODO: Fix this sometimes happening.
            }
            
        }

        private float EndOfDialogueButtonPromptOpacity()
        {
            var curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

            _timeSinceStateChange += Time.deltaTime;
            // reset time if it has changed
            if (_oldShowingDefaultContinueDialoguePrompt != _showingDefaultContinueDialoguePrompt)
            {
                _timeSinceStateChange = 0f;
                _oldShowingDefaultContinueDialoguePrompt = _showingDefaultContinueDialoguePrompt;
            }
            
            // now, calculate the fade values of the enter key prompt
            double changeInOpacity = 1 - minOpacity;
            double opacityAdd = Math.Sin(Time.time * opacityChangeSpeed) * changeInOpacity + minOpacity;
            double percentageThroughFade = (Math.Min(_timeSinceStateChange, fadeTime) / fadeTime);
            float percentageThroughFadeCurve = curve.Evaluate((float) percentageThroughFade);
            
            // So! We now have the part through the curve we're at
            // and, we have the current part of the sine wave we're at
            // Our maths now involve checking if we're supposed to be fading *out*
            // if so, we want to subtract the percentage through the fade out from 1
            
            if (!_showingDefaultContinueDialoguePrompt)
            {
                percentageThroughFadeCurve = 1 - percentageThroughFadeCurve;
            }

            return (float) (percentageThroughFadeCurve * opacityAdd);
        }

        private void SetActorText(Actor.Actor actor)
        {
            actorText.text = actor.name;
        }

        private void SetBodyText(string text)
        {
            bodyText.text = text;
        }

        private void SetActorModel(Actor.Actor actor)
        {
            if (lastLoadedActorName.Equals(actor.GetName()))
            {
                // This means we haven't *actually* changed actor, so we can just, sit here
                return;
            }
            // We've changed actor!
            lastLoadedActorName = actor.GetName();
            if (actorModelRoot == null)
            {
                return;
            }
            foreach (Transform child in actorModelRoot)
            {
                Destroy(child.gameObject);
            }

            try
            {
                Instantiate(actor.prefab, actorModelRoot);
            }
            catch (MissingReferenceException)
            {
                Debug.LogWarning("Could not instantiate model for " + lastLoadedActorName + " - missing reference. Try recreating your actor JSON!");
            }
            catch (ArgumentException)
            {
                Debug.LogWarning("Could not instantiate model for " + lastLoadedActorName + " - argument exception. Try recreating your actor JSON!");
            }
        }
    }
}