using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Grabable : MonoBehaviour
{
    public SpringJoint SpringJoint;
    public bool IsGrabed;
    public GameObject HitParticule;
    [NonSerialized] public Rigidbody Rigidbody;

    private Vector3 _anchorPos;
    private float _spring;
    private float _damper;
    void Start() {
        Rigidbody = GetComponent<Rigidbody>();
        SpringJoint = GetComponent<SpringJoint>();
        _anchorPos = SpringJoint.anchor;
        _spring = SpringJoint.spring;
        _damper = SpringJoint.damper;
        Destroy(SpringJoint);
    }

    public SpringJoint GetSpringJoint()
    {
        Debug.Log("GetJoin");
        if (SpringJoint != null) return SpringJoint;
        SpringJoint = gameObject.AddComponent<SpringJoint>();
        SpringJoint.anchor = _anchorPos;
        SpringJoint.damper = _damper;
        SpringJoint.spring = _spring;
        SpringJoint.autoConfigureConnectedAnchor = false;
        return SpringJoint;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!IsGrabed && other.relativeVelocity.magnitude > 10)
        {
            GameObject go =Instantiate(HitParticule, other.contacts[0].point, Quaternion.identity);
            go.transform.up = transform.position-other.contacts[0].point;
        }
        Debug.Log(
            other.relativeVelocity.magnitude
        );
    }
}
