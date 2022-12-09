using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour
{
    public int id;
    public Vector3 position;
    public float visibility;
    [SerializeField] private Transform[] drawRayTo;
    public void Update()
    {
        transform.position = Vector3.Lerp(transform.position, position, 10*Time.deltaTime);

        GetComponent<MeshRenderer>().enabled = visibility > 0.01f;
        // foreach (var item in drawRayTo)
        // {
        //     Debug.DrawLine(transform.position, item.position, Color.red);
        // }
    }
}
