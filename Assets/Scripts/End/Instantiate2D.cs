using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Instantiate2D : MonoBehaviour
{
    public Transform parent;
    public int nx, nz;
    public float offsetX, offsetZ;
    public GameObject prefab;
    int[] degrees;
    List<Transform> Tiles = new List<Transform>();
    NavMeshSurface surface;

    Vector3 startT, currT;

    private void OnEnable()
    {
        surface = GetComponentInParent<NavMeshSurface>();
        surface.BuildNavMesh();
    }


    void Start()
    {

        
        degrees = new int[4];

        for (int i = 0; i < degrees.Length; i++)
        {
            degrees[i] = i * 90;
        }


        startT = Vector3.zero;
        currT = Vector3.zero;
        for (int i = 0; i < nx; i++)
        {
            for (int j = 0; j < nz; j++)
            {
                GameObject go;
                if (parent != null)
                    go = Instantiate(prefab, parent);
                else
                    go = Instantiate(prefab);
                go.transform.localPosition = currT;
                currT = new Vector3(currT.x + offsetX, currT.y, currT.z);
                Quaternion rot = Quaternion.Euler(0, degrees[Random.Range(0, 4)], 0);
                go.transform.rotation = rot;
                Tiles.Add(go.transform);
            }
            currT = new Vector3(startT.x, currT.y, currT.z + offsetZ);
        }

         surface.RemoveData();
         surface.BuildNavMesh();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {

            for (int i = 0; i < Tiles.Count; i++)
            {
                Quaternion rot = Quaternion.Euler(0, degrees[Random.Range(0, 4)], 0);
                Tiles[i].rotation = rot;
            }

            surface.RemoveData();
            surface.BuildNavMesh();

        }
    }
}
