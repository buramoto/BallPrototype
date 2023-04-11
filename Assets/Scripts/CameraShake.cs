using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;
using static UnityEngine.RuleTile.TilingRuleOutput;
using static UnityEngine.UI.Image;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {
        /*you can set the originalPos to transform.localPosition of the camera in
        in that instance. */

        Debug.Log("Shake Fn entered in Camera Shake");
        Vector3 originalPos = new Vector3(0, 0, 0);
        float elapsedTime = 0f;
        while (elapsedTime<duration)
        {
            Debug.Log("Inside While Loop");
            float xOffset = Random.Range(-0.5f, 0.5f) * magnitude;
            float yOffset = Random.Range(-0.5f, 0.5f) * magnitude;
            transform.localPosition = new Vector3(xOffset, yOffset, originalPos.z);
            elapsedTime += Time.deltaTime;
            //wait one frame
            yield return null;
        }
        transform.localPosition = originalPos;
    }
}
