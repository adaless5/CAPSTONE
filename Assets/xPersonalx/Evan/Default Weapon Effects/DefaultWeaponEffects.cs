using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultWeaponEffects : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject _gunMuzzlePoint;
    public GameObject _muzzleEffects;
    public GameObject _muzzleFireSpiral;
    public GameObject[] _muzzleFireRings;

    public GameObject _hitLandedEffects;
    List<GameObject> _hitLandedBulletEffects = new List<GameObject>();
    List<GameObject> _hitLandedRingEffects = new List<GameObject>();

    bool _isFiring;

    float timer;
    private void Awake()
    {
        _hitLandedEffects = GameObject.Find("hitLandedEffects");

        _hitLandedBulletEffects.Add(GameObject.Find("hitLandedBulletEffect (1)"));
        _hitLandedBulletEffects.Add(GameObject.Find("hitLandedBulletEffect (2)"));
        _hitLandedBulletEffects.Add(GameObject.Find("hitLandedBulletEffect (3)"));

        _hitLandedRingEffects.Add(GameObject.Find("hitLandedRingEffects (1)"));
        _hitLandedRingEffects.Add(GameObject.Find("hitLandedRingEffects (2)"));

        //_gunMuzzlePoint = GameObject.Find("DefaultWeaponMuzzlePoint");
        _gunMuzzlePoint = GameObject.Find("DefaultMuzzlePoint");
        SetAllPiecesAlpha(0.0f);
    }

    public void Fire(Vector3 hitPosition, Vector3 hitNormal)
    {
        SetAllPiecesAlpha(1.0f);
        transform.position = _gunMuzzlePoint.transform.position;
        _hitLandedEffects.transform.position = hitPosition;
        _hitLandedEffects.transform.forward = hitNormal * -1;
    }
    public void Fire(Vector3 fireAngle)
    {

        SetMuzzlePieces();
        transform.position = _gunMuzzlePoint.transform.position;
        _muzzleEffects.transform.forward = -fireAngle;
        _muzzleFireSpiral.transform.forward = fireAngle;
        _isFiring = true;
        _hitLandedEffects.transform.localScale = new Vector3(1, 1, 1);
        _muzzleEffects.transform.localScale = new Vector3(1, 1, 1);
        timer = 0.5f;

    }

    void MuzzleEffect()
    {
        ImageSpin(_muzzleFireSpiral, 500.0f);
        ImageFade(_muzzleFireSpiral, 2.1F);
        for (int i = 0; i < _muzzleFireRings.Length; i++)
        {
            ImageSpin(_muzzleFireRings[i], 1000.0f);
            ImageFade(_muzzleFireRings[i], 4.0F);
        }
    }

    bool CheckFireOver()
    {
        bool fireOver = true;
        if (_muzzleFireSpiral.GetComponent<MeshRenderer>().material.color.a > 0.0f)
        {
            fireOver = false;
        }
        for (int i = 0; i < _muzzleFireRings.Length; i++)
        {
            if (_muzzleFireRings[i].GetComponent<MeshRenderer>().material.color.a > 0.0f)
            {
                fireOver = false;
            }
        }
        for (int i = 0; i < _hitLandedRingEffects.Count; i++)
        {
            if (_hitLandedRingEffects[i].GetComponent<MeshRenderer>().material.color.a > 0.0f)
            {
                fireOver = false;
            }
        }
        for (int i = 0; i < _hitLandedBulletEffects.Count; i++)
        {
            if (_hitLandedBulletEffects[i].GetComponent<MeshRenderer>().material.color.a > 0.0f)
            {
                fireOver = false;
            }
        }
        return fireOver;
    }

    void SetAllPiecesAlpha(float amount)
    {
        {
            Color col = _muzzleFireSpiral.GetComponent<MeshRenderer>().material.color;
            col.a = amount;
            _muzzleFireSpiral.GetComponent<MeshRenderer>().material.color = col;
        }
        foreach (GameObject fireRing in _muzzleFireRings)
        {
            Color col = fireRing.GetComponent<MeshRenderer>().material.color;
            col.a = amount;
            fireRing.GetComponent<MeshRenderer>().material.color = col;
        }
        foreach (GameObject Ring in _hitLandedRingEffects)
        {
            Color col = Ring.GetComponent<MeshRenderer>().material.color;
            col.a = amount;
            Ring.GetComponent<MeshRenderer>().material.color = col;
        }
        foreach (GameObject bulletEffect in _hitLandedBulletEffects)
        {
            Color col = bulletEffect.GetComponent<MeshRenderer>().material.color;
            col.a = amount;
            bulletEffect.GetComponent<MeshRenderer>().material.color = col;
        }
    }

    void SetMuzzlePieces()
    {
        {
            Color col = _muzzleFireSpiral.GetComponent<MeshRenderer>().material.color;
            col.a = 1.0f;
            _muzzleFireSpiral.GetComponent<MeshRenderer>().material.color = col;
        }
        foreach (GameObject fireRing in _muzzleFireRings)
        {
            Color col = fireRing.GetComponent<MeshRenderer>().material.color;
            col.a = 1.0f;
            fireRing.GetComponent<MeshRenderer>().material.color = col;
        }
    }

    void HitLandedEffect()
    {
        for (int i = 0; i < _hitLandedRingEffects.Count; i++)
        {
            ImageSpin(_hitLandedRingEffects[i], 1000.0f);
            ImageFade(_hitLandedRingEffects[i], 4.0F);
        }
        for (int i = 0; i < _hitLandedBulletEffects.Count; i++)
        {
            ImageSpin(_hitLandedBulletEffects[i], 1000.0f);
            ImageFade(_hitLandedBulletEffects[i], 2.1F);
        }
    }
    void ImageSpin(GameObject obj, float speed)
    {
        obj.transform.Rotate(0, 0, Time.deltaTime * speed);
    }

    void ImageFade(GameObject obj, float fadeRate)
    {
        if (obj.GetComponent<MeshRenderer>().material.color.a - (Time.deltaTime * fadeRate) > 0.0f)
        {
            Color col = obj.GetComponent<MeshRenderer>().material.color;
            col.a -= Time.deltaTime * fadeRate;
            obj.GetComponent<MeshRenderer>().material.color = col;
        }
        else
        {
            Color col = obj.GetComponent<MeshRenderer>().material.color;
            col.a = 0.0f;
            obj.GetComponent<MeshRenderer>().material.color = col;
        }
    }
    void GrowFire()
    {
        _hitLandedEffects.transform.localScale *= 1.3f;
        _muzzleEffects.transform.localScale *= 1.2f;

    }
    // Update is called once per frame
    void Update()
    {
        if (_gunMuzzlePoint != null /*&& _hitLandedBulletEffects[0] != null && _hitLandedBulletEffects[1] != null && _hitLandedBulletEffects[2] != null && _hitLandedRingEffects[0] != null && _hitLandedRingEffects[1] != null*/)
        {
            if (timer > 0.0f)
            {
                HitLandedEffect();
                MuzzleEffect();
                GrowFire();
                timer -= Time.deltaTime;
            }
            
        }
        else
        {
            _gunMuzzlePoint = GameObject.Find("DefaultWeaponMuzzlePoint");
        }
    }
}

