using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ActionButtonConfig
{
    [SerializeField] public ActionButtonType ButtonType;
    [SerializeField] public GameObject IconPrefab;
}
