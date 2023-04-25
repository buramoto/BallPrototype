using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirflowLR : MonoBehaviour
{
    // Start is called before the first frame update
    private AreaEffector2D areaEffector;
    void Start()
    {
        areaEffector = GetComponent<AreaEffector2D>();
        areaEffector.forceMagnitude = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        areaEffector = GetComponent<AreaEffector2D>();
        areaEffector.forceMagnitude = 3f;
    }
}
