using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private Transform _transform;
    private Vector3 _eulers = Vector3.up;

    public float RotationSpeed;

    void Awake()
    {
        _transform = GetComponent<Transform>();
    }


    void Update()
    {
        _transform.Rotate(_eulers * RotationSpeed * Time.deltaTime);
    }
}
