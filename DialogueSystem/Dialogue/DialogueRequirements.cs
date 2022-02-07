using System;
using System.Collections.Generic;
using KeyStore;
using UnityEngine;

namespace DialogueSystem.Dialogue
{
    [Serializable]
    public class DialogueRequirements
    {
        public string key;
        public dynamic expected;
        public DialogueCompareTypes comparator;
        public string failReason;
        public int set;

        public DialogueRequirements(string key, dynamic expected, DialogueCompareTypes comparator, string failReason,
            int set)
        {
            this.key = key;
            this.expected = expected;
            this.comparator = comparator;
            this.failReason = failReason;
            this.set = set;
        }

        public DialogueRequirements(string key, dynamic expected, DialogueCompareTypes comparator, string failReason)
        {
            this.key = key;
            this.expected = expected;
            this.comparator = comparator;
            this.failReason = failReason;
            set = -1;
        }

        public bool Evaluate()
        {
            dynamic current = KeyStoreHandler.Default().Get(key);
            return DialogueCompareHelper.Compare(comparator, current, expected);
        }

        public int GetSet()
        {
            if (set < 0)
            {
                return -1;
            }
            return set;
        }

        public string GetFailReason()
        {
            return failReason;
        }

        public static Tuple<bool, string> EvaluateList(List<DialogueRequirements> l)
        {
            string failReason = "";
            List<Tuple<int, bool, string>> setResults = new List<Tuple<int, bool, string>>();
            // we want to go through and build up a list of sets from our list of requirements
            foreach (DialogueRequirements dR in l)
            {
                // first, we let the result evaluate
                bool result = dR.Evaluate();
                bool found = false;
                // now, we build the error result (if failed)
                string message;
                if (result)
                {
                    message = "";
                }
                else
                {
                    message = dR.GetFailReason() + "\n";
                }

                // now we go through the existing sets, and build our requirement checking
                for (int i = 0; i < setResults.Count; i++)
                {
                    // matching set!
                    if (setResults[i].Item1 == dR.GetSet())
                    {
                        // mark off we found the right set
                        found = true;
                        bool newValue;
                        // -1 is the "special" set for any dialogue item not in it's own set
                        // the special set requires *all* it's items to be true
                        // hence this little check
                        if (dR.GetSet() == -1)
                        {
                            newValue = setResults[i].Item2 && result;
                        }
                        else
                        {
                            newValue = setResults[i].Item2 || result;
                        }

                        // we now update the set results !
                        // the message is appended - it has a newline on any successful, so we can do this safely
                        setResults[i] = Tuple.Create(setResults[i].Item1, newValue, setResults[i].Item3 + message);
                        break;
                    }
                }

                if (!found)
                {
                    // if not found, we actually need to *make* a new set!
                    setResults.Add(Tuple.Create(dR.GetSet(), result, message));
                }
            }

            // Built up set results by this point
            // the special "-1" set has already been handled by the above code
            // so! we can just go through and make sure all sets are true.
            string errorsToDisplay = "";
            bool success = true;
            foreach (Tuple<int, bool, string> t in setResults)
            {
                success = success && t.Item2;
                if (!t.Item2)
                {
                    if (t.Item3.Length > 0)
                    {
                        // we actually have an error for this set! use it.
                        string prefix = "";
                        if (t.Item1 == -1)
                        {
                            prefix = "[REQUIRED]";
                        }

                        if (t.Item1 == -1)
                        {
                            prefix = "[REQUIREMENT SET " + t.Item1 + "]";
                        }

                        errorsToDisplay = t.Item3 + errorsToDisplay;
                    }
                }
            }
            return Tuple.Create(success, errorsToDisplay);
        }
    }
}