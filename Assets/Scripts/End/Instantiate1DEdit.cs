using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Instantiate1DEdit : MonoBehaviour
{
    public bool instantiateN, destoryAll;
    public Transform parent;
    public int n;
    public GameObject prefab;
    public Vector3 deltaT, deltaR, deltaS;

    Vector3 currT, currR, currS;

    private void Update()
    {
        if (instantiateN)
        {
            instantiateN = false;
            Instantiate();
        }
        if (destoryAll)
        {
            destoryAll = false;
            DestroyAll();

        }
    }

    void Instantiate()
    {
        currT = currR = Vector3.zero;
        currS = Vector3.one;
        for (int i = 0; i < n; i++)
        {
            GameObject go;
            if (parent != null)
                go = Instantiate(prefab, parent);
            else
                go = Instantiate(prefab);
            go.transform.localPosition = currT;
            go.transform.localRotation = Quaternion.Euler(currR);
            go.transform.localScale = currS;
            currT += deltaT;
            currR += deltaR;
            currS += deltaS;
        }
    }

    void DestroyAll()
    {
        int n = transform.childCount;
        List<Transform> children = new List<Transform>();
        for (int i = n - 1; i >= 0; i--)
        {
            if (Application.isPlaying)
                Destroy(transform.GetChild(i).gameObject);
            else
                DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
