using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Transformer_SolarSystem : MonoBehaviour
{
    private Toggle straight;
    private Toggle zoomed;
    private Toggle rotated;
    private float count = 0.0f;
    private float angle= 0f;
    private Vector3 position= new Vector3(0,4,0);
    private float scale= 0f;
    public float start=0;
    private float fromAngle;
    private float toAngle;
    private Vector3 fromPosition;
    private Vector3 toPosition;
    private float fromScale;
    private float toScale;
    private Boolean zooming = false;
    private Boolean rotating = false;
    // Start is called before the first frame update
    void Start()
    {
        straight = GameObject.Find("Straight Toggle").GetComponent<Toggle>();
        rotated = GameObject.Find("Toggle Rotate Time").GetComponent<Toggle>();
        zoomed = GameObject.Find("Zoom Toggle").GetComponent<Toggle>();
        rotated.onValueChanged.AddListener(delegate { RotateSystem(); });
        zoomed.onValueChanged.AddListener(delegate { ZoomSystem(); });
    }
    void RotateSystem()
    {
        rotating = true;
        start = Time.time;
        if (rotated.isOn)
        {
            fromAngle = -90;
            toAngle = 0f;
        }
        else
        {
            fromAngle = 0;
            toAngle = -90f;
        }
    }
    void ZoomSystem()
    {
        zooming = true;
        start = Time.time;
        if (!zoomed.isOn)
        {
            fromScale = 10;
            toScale = 1;
            fromPosition = new Vector3(0,4,0);
            toPosition = new Vector3(0,4,0);
        }
        else
        {
            fromScale = 1;
            toScale = 10;
            fromPosition = new Vector3(0,4,0);
            toPosition = new Vector3(0,4,0);
        }
    }
    void Update()
    {
        if (start!=0 && Time.time <= start + 1f)
        {
            float elapsedTime = (Time.time - start);
            if(rotating){
                angle = Mathf.SmoothStep(fromAngle, toAngle, elapsedTime );
                transform.eulerAngles = new Vector3(angle, 0, 0);
            }
            else if(zooming){
                scale = Mathf.SmoothStep(fromScale, toScale, elapsedTime );
                position = new Vector3(
                    Mathf.SmoothStep(fromPosition.x, toPosition.x, elapsedTime ),
                    Mathf.SmoothStep(fromPosition.y, toPosition.y, elapsedTime ),
                    Mathf.SmoothStep(fromPosition.z, toPosition.z, elapsedTime )
                );
                transform.localScale = new Vector3(scale,scale,scale);
                //transform.position = position;
            }
        }
        else
        {
            rotating = false;
            zooming = false;
            start = 0;
        }
        
    }
}
