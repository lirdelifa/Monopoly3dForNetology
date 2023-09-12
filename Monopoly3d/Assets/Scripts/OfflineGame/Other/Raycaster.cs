using UnityEngine;

public class Raycaster : IDispose
{
    private Camera _camera;

    private float _maxRayLenght = 100f;

    Raycaster()
    {
        _camera = Camera.main;
        InputListener.OnDownMouse0 += Cast;
    }

    private void Cast()
    {
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, _maxRayLenght))
        {
            IRaycasterListener raycasterListener = (IRaycasterListener)hit.collider.GetComponent(typeof(IRaycasterListener));
            if(raycasterListener != null) 
            {
                OnRaycastHit?.Invoke(raycasterListener);
            }
        }
    }

    public void Dispose()
    {
        InputListener.OnDownMouse0 -= Cast;
    }

    public delegate void RaycastHitDelegate(IRaycasterListener raycasterListener);
    public static RaycastHitDelegate OnRaycastHit;
}
