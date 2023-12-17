using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelsParticles : MonoBehaviour
{
    public GameObject smokePref;
    [Range(0, 2f)]public float slipLimit;

    public WheelCollider wheelCollider;
    private GameObject smoke;
    private ParticleSystem SmokeParticle;
    public TrailRenderer trail;

    void Start()
    {
        SpawnParticles();
    }

    void FixedUpdate()
    {
        RunParticles();
    }

    void SpawnParticles()
    {
        smoke = Instantiate(smokePref);
        SmokeParticle = smoke.GetComponent<ParticleSystem>();
        SmokeParticle.Stop();
        smoke.transform.parent = gameObject.transform;
        smoke.transform.position = gameObject.transform.position + new Vector3(0f, -0.3f, 0f);
        smoke.transform.rotation = Quaternion.identity;
        smoke.transform.localScale = new Vector3 (1,1,1);
    }

    private void RunParticles()
    {
        WheelHit hit;
        if(wheelCollider.GetGroundHit(out hit))
        {
            if (Mathf.Abs(hit.sidewaysSlip) + Mathf.Abs(hit.forwardSlip) >= slipLimit)
            {
                SmokeParticle.Play();
                Vector3 contactPoint = hit.point;
                contactPoint.y += 0.01f;
                trail.transform.position = contactPoint;
                trail.emitting = true;
            }
            else
            {
                SmokeParticle.Stop();
                trail.emitting = false;
            }
        }
        else
        {
            trail.emitting = false;
            SmokeParticle.Stop();
        }
    }
}
