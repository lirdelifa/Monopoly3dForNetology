using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameStart
{
    [Inject] private CellsData _cellsData;

    public void Start()
    {
        FirstSpawn();
    }

    private void FirstSpawn()
    {
        Vector3 position = _cellsData.GetStartCell.GetFreeTargetPoint().GetPoint;
        UnitFactory.Create(position, Quaternion.identity);
    }
}
