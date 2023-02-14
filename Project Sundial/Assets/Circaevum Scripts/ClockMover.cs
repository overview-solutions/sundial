using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockMover : MonoBehaviour
{    
	private Sundial sundial;
    private GameObject clock;
    public float time;
    public float start=0;
    private Vector3 position= new Vector3(0,0,0);
    private Vector3 fromPosition;
    private Vector3 toPosition;
    private float fromAngle;
    private float toAngle;
    private Toggle rotated;
    private Toggle zoomed;
    private Boolean rotating = false;
    private Boolean zooming = false;

    private void Start()
    {
        sundial = GameObject.Find("Sundial").GetComponent<Sundial>();
        rotated = GameObject.Find("Toggle Rotate Time").GetComponent<Toggle>();
        zoomed = GameObject.Find("Zoom Toggle").GetComponent<Toggle>();
        rotated.onValueChanged.AddListener(delegate { RotateSystem(); });
        zoomed.onValueChanged.AddListener(delegate { RotateSystem(); });
    }
    void Transform(bool straight, bool rotated, bool zoomed, bool twisted)
    {
        start = Time.time;
    }
    void RotateSystem()
    {
        rotating = true;
        start = Time.time;
        if (!zoomed.isOn)
        {
            if (rotated.isOn)
            {
                fromPosition = new Vector3(
                    sundial.distances[2] / sundial.horizonScale * 2 * Mathf.Sin(time * Mathf.PI / 180f  /288f), 
                    0,
                    -sundial.distances[2] / sundial.horizonScale * 2 * Mathf.Cos(time * Mathf.PI / 180f /288f)
                );
                toPosition = new Vector3(
                    sundial.distances[2] / sundial.horizonScale * 2 * Mathf.Sin(time * Mathf.PI / 180f  /288f), 
                    sundial.distances[2] / sundial.horizonScale * 2 * Mathf.Cos(time * Mathf.PI / 180f /288f),
                    0
                );
            }
            else
            {
                fromPosition = new Vector3(
                    sundial.distances[2] / sundial.horizonScale * 2 * Mathf.Sin(time * Mathf.PI / 180f  /288f), 
                    sundial.distances[2] / sundial.horizonScale * 2 * Mathf.Cos(time * Mathf.PI / 180f /288f),
                    0
                );
                toPosition = new Vector3(
                    sundial.distances[2] / sundial.horizonScale * 2 * Mathf.Sin(time * Mathf.PI / 180f /288f), 
                    0,
                    -sundial.distances[2] / sundial.horizonScale * 2 * Mathf.Cos(time * Mathf.PI / 180f/288f)
                );
            }
        }
        else
        {
            fromPosition = new Vector3(0,0,0);
            toPosition = new Vector3(0,0,0);
        }


    }
    void ZoomSystem()
    {
        zooming = true;
        start = Time.time;
        if (zoomed.isOn)
        {
            if (!rotated.isOn)
            {
                fromPosition = new Vector3(
                    sundial.distances[2] / sundial.horizonScale * 2 * Mathf.Sin(time * Mathf.PI / 180f  /288f), 
                    0,
                    -sundial.distances[2] / sundial.horizonScale * 2 * Mathf.Cos(time * Mathf.PI / 180f /288f)
                );
            }
            else
            {
                fromPosition = transform.position = new Vector3(
                    sundial.distances[2] / sundial.horizonScale * 2 * Mathf.Sin(time * Mathf.PI / 180f  /288f), 
                    sundial.distances[2] / sundial.horizonScale * 2 * Mathf.Cos(time * Mathf.PI / 180f /288f),
                    0
                );
            }
            toPosition = new Vector3(0,0,0);
        }
        else
        {
            fromPosition = new Vector3(0,0,0);
            if (!rotated.isOn)
            {
                toPosition = new Vector3(
                    sundial.distances[2] / sundial.horizonScale * 2 * Mathf.Sin(time * Mathf.PI / 180f  /288f), 
                    0,
                    -sundial.distances[2] / sundial.horizonScale * 2 * Mathf.Cos(time * Mathf.PI / 180f /288f)
                );
            }
            else
            {
                toPosition = transform.position = new Vector3(
                    sundial.distances[2] / sundial.horizonScale * 2 * Mathf.Sin(time * Mathf.PI / 180f  /288f), 
                    sundial.distances[2] / sundial.horizonScale * 2 * Mathf.Cos(time * Mathf.PI / 180f /288f),
                    0
                );
            }
        }
    }
    private void Update()
    {
        time = sundial.now*360/sundial.pDays[2];

        //This is the 1-second-long operation of zooming or rotating (doesn't work great with both at the same time yet)
        if (start!=0 && Time.time <= start + 1f)
        {
            float elapsedTime = (Time.time - start);
            if(zooming||rotating){
                position = new Vector3(
                    Mathf.SmoothStep(fromPosition.x, toPosition.x, elapsedTime / 1f),
                    Mathf.SmoothStep(fromPosition.y, toPosition.y, elapsedTime / 1f),
                    Mathf.SmoothStep(fromPosition.z, toPosition.z, elapsedTime / 1f)
                );
                transform.position = position;
            }
        }
        else
        {
            //If we are Zoomed in...
            if(zoomed.isOn){
                //Put the whole thing centered at the Origin
                transform.position = new Vector3(0,0,0);
                //Are we rotated too?
                if (rotated.isOn){
                    transform.eulerAngles = new Vector3(0,0,-time* Mathf.PI / 180f*288f/5f / 360f);
                }
            }
            else
            {
                if (!rotated.isOn){
                    transform.position = new Vector3(
                        sundial.distances[2] / sundial.horizonScale * 2 * Mathf.Sin(time * Mathf.PI / 180f  /288f), 
                        0,
                        -sundial.distances[2] / sundial.horizonScale * 2 * Mathf.Cos(time * Mathf.PI / 180f /288f)
                    );
                    //transform.eulerAngles = new Vector3(0,-time* Mathf.PI / 180f*288f/5f / 360f,0);
                }else{
                    transform.position = new Vector3(
                        sundial.distances[2] / sundial.horizonScale * 2 * Mathf.Sin(time * Mathf.PI / 180f  /288f), 
                        sundial.distances[2] / sundial.horizonScale * 2 * Mathf.Cos(time * Mathf.PI / 180f /288f),
                        0
                    );
                    transform.eulerAngles = new Vector3(0,0,-time* Mathf.PI / 180f *288f/5f/ 365f);
                }
                
            }
        }
    }
}
