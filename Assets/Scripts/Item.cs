using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Weapon, Wheel };
    public Type type;
    public int value;


    Rigidbody RGbody;
    SphereCollider sphereCollider;

    void Start()
    {
        RGbody = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    void Update()
    {
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            RGbody.isKinematic = true;
            sphereCollider.enabled = false;
        }
    }
}
