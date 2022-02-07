using UnityEngine;

namespace DialogueSystem.Dialogue
{
    public static class DialogueCompareHelper
    {
        public static bool Compare(DialogueCompareTypes d, dynamic v1, dynamic v2)
        {
            if (d == DialogueCompareTypes.Equals)
            {
                return v1 == v2;
            }
            else if (d == DialogueCompareTypes.Lower)
            {
                return v1 > v2;
            }
            else if (d == DialogueCompareTypes.LowerEq)
            {
                return v1 >= v2;
            }
            else if (d == DialogueCompareTypes.Higher)
            {
                return v1 < v2;
            }
            else if (d == DialogueCompareTypes.HigherEq)
            {
                return v1 <= v2;
            }
            else if (d == DialogueCompareTypes.NotEquals)
            {
                return v1 != v2;
            }
            Debug.LogWarning("Unhandled DialogueCompareType : " + d + " - returning false.");
            return false;
        }
    }
}