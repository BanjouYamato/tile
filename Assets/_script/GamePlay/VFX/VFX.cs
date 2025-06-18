using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : MonoBehaviour, IPool
{
    VFXPool pool;
    public void OnSpawn()
    {
        var par = this.GetComponent<ParticleSystem>();
        par.Clear();
        par.Play();
    }
    public void SetUp(VFXPool _pool)
    {
        pool = _pool;
    }
    private void OnDisable()
    {
        pool.Return(this);
    }
}
