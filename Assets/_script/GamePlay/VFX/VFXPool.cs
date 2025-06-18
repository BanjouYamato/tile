
using UnityEngine;

public class VFXPool : MonoBehaviour
{
    [SerializeField] VFX _inputPre;
    ObjectPool<VFX> _pool;
    [SerializeField] Transform _inputParent;
    [SerializeField] Camera _camera;

    private void Awake()
    {
        _pool = new ObjectPool<VFX>(_inputPre, _inputParent);
    }
    private void Start()
    {
        _camera = Camera.main;
    }
    public VFX GetVFX()
    {
        var newVFX = _pool.Get();
        newVFX.SetUp(this);
        newVFX.transform.position = _camera.ScreenToViewportPoint(Input.mousePosition);
        return newVFX;
    }
    public void Return(VFX _vfx)
    {
        _pool.ReturnPool(_vfx);
    }
}
