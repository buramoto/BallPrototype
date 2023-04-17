using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallMaterialsMenu : MonoBehaviour
{
    public StateReference.ballMaterial material;
    private void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(changeMaterial);
    }

    private void changeMaterial()
    {
        UIBehavior.gameUI.SetBallMaterial(material);
    }
}
