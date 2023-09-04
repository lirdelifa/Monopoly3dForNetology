using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private List<TargetPoint> _targetPoints;
    

    public int Id { get; private set; }

    public void SetId(int id) => Id = id;

    public TargetPoint GetFreeTargetPoint()
    {
        for(int i = 0; i < _targetPoints.Count; i++) 
        {
            if (_targetPoints[i].IsFree) return _targetPoints[i];
        }
        return null;
    }
    //public int GetId { get => _id; }
    // Start is called before the first frame update
    

    //public void SelectedTargetPint()
    //{

    //}

}
