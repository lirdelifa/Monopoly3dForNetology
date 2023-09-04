using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CellGroupParameters
{
    [SerializeField] public CellGroupType type;
    [SerializeField] public Material material;
    [SerializeField] public string name;
    [HideInInspector] public List<Cell> cells;
}
