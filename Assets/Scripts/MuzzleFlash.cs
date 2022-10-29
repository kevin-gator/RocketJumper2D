using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    public GameObject mzFlash;

    private float mzwaitTime = 0.23f;
    private float mzTimer = 0.0f;

    private Animator mzAnimator;

    void Start()
    {
        mzAnimator = gameObject.GetComponent<Animator>();
    }

    public void ActivateMuzzleFlash()
    {
        mzFlash.SetActive(true);
        mzTimer = 0f;
        mzAnimator.Play("MuzzleFire_Default");
    }

    void Update()
    {
        mzTimer += Time.deltaTime;
        if (mzTimer > mzwaitTime)
        {
            mzFlash.SetActive(false);
            mzAnimator.Play("None");
        }
    }
}
