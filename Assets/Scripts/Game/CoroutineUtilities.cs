using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoroutineUtilities: CustomYieldInstruction
{
    public override bool keepWaiting => throw new System.NotImplementedException();

    public static IEnumerator TypeOutText(string text, Text textComponent, float characterDelay, float initialDelay, GameObject activatedObject)
    {
        yield return new WaitForSecondsRealtime(initialDelay);

        int i = 0;
        while (i <= text.Length)
        {
            i++;
            if (i > text.Length)
            {
                if (activatedObject != null) activatedObject.SetActive(true);
                else break;
                yield return null;
            }
            else
            {
                textComponent.text = text.Substring(0, i);
                yield return new WaitForSecondsRealtime(characterDelay);
            }
        }
    }
}