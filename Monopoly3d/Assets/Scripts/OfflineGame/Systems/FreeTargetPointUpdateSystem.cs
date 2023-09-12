using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

public class FreeTargetPointUpdateSystem : IDispose
{
    private List<TargetPoint> _targetPoints = new List<TargetPoint>();

    public void AddAllTargetPoints(List<TargetPoint> targetPoints)
    {
        _targetPoints.AddRange(targetPoints);
    }

    FreeTargetPointUpdateSystem()
    {
        Character.OnEndMove += Update;
    }

    public void Update(Character character = null)
    {
        for (int i = 0; i < _targetPoints.Count; i++)
        {
            Vector3 verificationPosition = _targetPoints[i].GetPoint;
            bool isFree = !CharactersData.GetCharacters.Any(s => s.GetPoint.x == verificationPosition.x && s.GetPoint.z == verificationPosition.z);
            _targetPoints[i].IsFree = isFree;
        }
    }

    public void Dispose()
    {
        Character.OnEndMove -= Update;
    }
}
