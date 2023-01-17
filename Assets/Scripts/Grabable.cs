using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Grabable : MonoBehaviour
{
    public SpringJoint SpringJoint;
    public bool IsGrabed;
    public GameObject HitParticule;
    public GameObject HitPlayerParticule;
    public int Damage = 2;
    public bool Throwed;
    [NonSerialized] public Rigidbody Rigidbody;

    [Range(0, 1)] public float volume = 1f;
    public List<AudioClip> objectSounds = new List<AudioClip>();
    public List<AudioClip> hitSounds = new List<AudioClip>();

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
            if (Throwed&&other.collider.CompareTag("Player"))
            {
                GameObject go =Instantiate(HitPlayerParticule, other.contacts[0].point, Quaternion.identity);
                go.transform.up = transform.position-other.contacts[0].point;
                other.gameObject.GetComponentInParent<VeryController3>().TakeDamage(Damage);
                Throwed = false;
                
                int soundIndex = Random.Range(0, objectSounds.Count);
                if (objectSounds.Count>0)SoundManager.Instance.PlayerSound(objectSounds[soundIndex], volume);
                
                int hitIndex = Random.Range(0, hitSounds.Count);
                if (objectSounds.Count>0)SoundManager.Instance.PlayerSound(hitSounds[hitIndex], volume);
                
            }
            else
            {
                GameObject go = Instantiate(HitParticule, other.contacts[0].point, Quaternion.identity);
                go.transform.up = transform.position - other.contacts[0].point;
                
                int soundIndex = Random.Range(0, objectSounds.Count);
                if (objectSounds.Count>0) SoundManager.Instance.PlayerSound(objectSounds[soundIndex], volume);
            }
        }

        if (other.relativeVelocity.magnitude < 10) {
            Throwed = false;
        }
    }
}
