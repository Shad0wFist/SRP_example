using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeSystem : MonoBehaviour
{
    public GameObject smokePref;
    [Range(0, 2f)]public float slipLimit;

    public Transform[] wheelTransforms;
    public WheelCollider[] wheelColliders;
    public GameObject[] smokes;
    public ParticleSystem[] SmokeParticles;
    public TrailRenderer[] trails;

    void Start()
    {
        FindValues();
        SpawnSmokes();
    }

    void FixedUpdate()
    {
        RunSmokes();
    }

    void FindValues()
    {
        smokes = new GameObject[wheelTransforms.Length];
        SmokeParticles = new ParticleSystem[wheelTransforms.Length];
    }

    void SpawnSmokes()
    {
        for (int i = 0; i < wheelTransforms.Length; i++)
        {
            smokes[i] = Instantiate(smokePref);
            SmokeParticles[i] = smokes[i].GetComponent<ParticleSystem>();
            SmokeParticles[i].Stop();
            smokes[i].transform.parent = wheelTransforms[i].transform;
            smokes[i].transform.position = wheelTransforms[i].transform.position + new Vector3(0f, -0.3f, 0f);
            smokes[i].transform.rotation = Quaternion.identity;
            smokes[i].transform.localScale = new Vector3 (1,1,1);
        }
    }

    private void RunSmokes()
    {
        WheelHit hit;
        for (int i = 0; i < wheelTransforms.Length; i++)
        {
            if(wheelColliders[i].GetGroundHit(out hit))
            {
                if (Mathf.Abs(hit.sidewaysSlip) + Mathf.Abs(hit.forwardSlip) >= slipLimit)
                {
                    SmokeParticles[i].Play();
                    Vector3 contactPoint = hit.point;
                    contactPoint.y += 0.01f;
                    trails[i].transform.position = contactPoint;
                    trails[i].emitting = true;
                }
                else
                {
                    SmokeParticles[i].Stop();
                    trails[i].emitting = false;
                }
            }
            else
            {
                
                trails[i].emitting = false;
                SmokeParticles[i].Stop();
            }
        }
    }
}
