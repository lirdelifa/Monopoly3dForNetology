using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerSpawn : MonoBehaviour
{
    [Inject] private MouseClickHandler _mouseClickHandler;
    [Inject] private UnitData _unitData;
    [SerializeField] private UnitMove _unitMove;
    void Start()
    {
        _mouseClickHandler.SetUnit(_unitMove);
        _unitData.SetPlayerMove(_unitMove);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
