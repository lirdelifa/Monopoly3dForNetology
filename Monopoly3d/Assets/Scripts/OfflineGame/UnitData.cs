using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData
{
    public UnitMove PlayerMove { get; private set; }
    public CameraTransformation CameraTransformation { get; private set; }

    public void SetPlayerMove(UnitMove unitMove) => PlayerMove = unitMove;
    public void SetCameraTransformation(CameraTransformation cameraTransformation) => CameraTransformation = cameraTransformation;
}
