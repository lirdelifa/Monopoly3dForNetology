using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private List<TargetPoint> _targetPoints;
    [SerializeField] private GameObject _markerArrow;
    public List<TargetPoint> GetTargetPoints { get => _targetPoints; }

    protected int _id;
    public Vector2 GetSize { get => new Vector2(transform.localScale.x, transform.localScale.z); }
    public Vector3 GetPosition { get => transform.position; }
    public int GetId { get => _id; }

    public void SetId(int id) => _id = id;

    public void EnableMarkerArrow(bool enable) => _markerArrow.SetActive(enable);
}
