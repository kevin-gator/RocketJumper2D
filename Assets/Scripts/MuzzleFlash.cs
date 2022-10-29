using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    public GameObject mzFlash;

    private Animator _mzAnimator;
    private float _mzTimer;

    private readonly float _mzwaitTime = 0.23f;

    private void Start()
    {
        _mzAnimator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        _mzTimer += Time.deltaTime;
        if (_mzTimer > _mzwaitTime)
        {
            mzFlash.SetActive(false);
            _mzAnimator.Play("None");
        }
    }

    public void ActivateMuzzleFlash()
    {
        mzFlash.SetActive(true);
        _mzTimer = 0f;
        _mzAnimator.Play("MuzzleFire_Default");
    }
}
