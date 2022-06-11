using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class cameraObjOffsetSmooth : MonoBehaviour {
	public Transform parentObj;
	public Vector3 offset;
    public float Speed;

    void Update () {
        if (parentObj)
        {
		    transform.position = Vector3.Lerp(transform.position, parentObj.position + offset, Time.deltaTime*Speed);
        }
	}
}
