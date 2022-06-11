using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateRotator : MonoBehaviour
{
    public Transform Player, Root;
    public GameObject Prefab;
    public float Radius = 6;
    public int Num = 16;
    // Start is called before the first frame update
    void Start()
    {

        float angle = 360 / (Num-1);
        float rad = Mathf.Deg2Rad * angle;

        for (int i = 0; i < Num; i++)
        {

            GameObject go = Instantiate(Prefab, Root);

            float posX = transform.position.x + (Mathf.Sin(i*rad) * Radius);
            float posY = transform.position.y + (Mathf.Cos(i*rad) * Radius);

            Vector3 pos = new Vector3(posX, posY, 0);
            go.transform.position = pos;
            Vector3 dir = (transform.position - go.transform.position).normalized;
            Quaternion rot = Quaternion.LookRotation(dir, go.transform.up);
            go.transform.rotation = rot;

            if (i == 0)
                go.SetActive(false);



        }

        float pX = transform.position.x + (Mathf.Sin(8*rad) * Radius);
        float pY = transform.position.y + (Mathf.Cos(8*rad) * Radius);

        Vector3 p = new Vector3(pX+1, pY+1, 0);
        Player.position = p;
        

    }

   
}
