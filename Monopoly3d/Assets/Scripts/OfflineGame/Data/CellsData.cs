using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CellsData
{
    private static List<Cell> _cells = new List<Cell>();
    private static List<int> _cornerCellsId = new List<int>();
    public static List<int> TeleportCellsId { get; private set; } = new List<int>();
    public static List<int> DropCellsId { get; private set; } = new List<int>();
    public static List<int> ChanceCellsId { get; private set; } = new List<int>();
    private static List<int> _propertyCellsId = new List<int>();

    private static List<CellGroupParameters> _propertyCellGroupParameters = new List<CellGroupParameters>();
    private static Dictionary<CellGroupType, List<CellProperty>> _propertyCellGroups = new Dictionary<CellGroupType, List<CellProperty>>();

    public static int StartCellId { get; private set; }
    public static int ArrestCellId { get; private set; }
    public static int PrisonCellId { get; private set; }
    public static int ShelterCellId { get; private set; }

    public static Cell GetStartCell { get => _cells[StartCellId]; }
    public static Cell GetPrisonCell { get => _cells[PrisonCellId]; }
    public static Cell GetArrestCell { get => _cells[ArrestCellId]; }
    public static Cell GetShelterCell { get => _cells[ShelterCellId]; }

    public static void AddAllCells(List<Cell> cells)
    {
        _cells = cells;
        for (int i = 0; i < _cells.Count; i++)
        {
            if (_cells[i] is CellStart) StartCellId = i;   
            else if(_cells[i] is CellArrest) ArrestCellId = i;
            else if (_cells[i] is CellShelter) ShelterCellId = i;
            else if (_cells[i] is CellPrison) PrisonCellId = i;

            else if (_cells[i] is CellChance) ChanceCellsId.Add(i);
            else if (_cells[i] is CellDrop) DropCellsId.Add(i);
            else if (_cells[i] is CellTeleport) TeleportCellsId.Add(i);

            else if (_cells[i] is CellProperty)
            {
                CellProperty cellProperty = (CellProperty)_cells[i];
                _propertyCellsId.Add(i);
                cellProperty.SetBorderColor(_propertyCellGroupParameters.First(s => s.groupType == cellProperty.GetGroupType).color);
                if (!_propertyCellGroups.ContainsKey(cellProperty.GetGroupType))
                    _propertyCellGroups[cellProperty.GetGroupType] = new List<CellProperty>();
                _propertyCellGroups[cellProperty.GetGroupType].Add(cellProperty);
            }
            _cells[i].SetId(i);
        }
        _cornerCellsId = new List<int>() { StartCellId, PrisonCellId, ShelterCellId, ArrestCellId };
    }
    public static void AddGroupParameters(List<CellGroupParameters> cellGroupParameters) => _propertyCellGroupParameters = cellGroupParameters;

    public static List<Vector3> GeneratePathToPoint(Vector3 currentPosition, Vector3 targetPosition)
    {
        int currentCellId = DefiningCellIdByPosition(currentPosition);
        int targetCellId = DefiningCellIdByPosition(targetPosition);
     
        List<Vector3> points = new List<Vector3>();

        for(int i = currentCellId + 1; i < ((targetCellId > currentCellId) ? targetCellId : targetCellId + _cells.Count); i++) 
        {
            int b = (i < _cells.Count) ? i : i - _cells.Count;
            if (_cornerCellsId.Contains(b)) points.Add(_cells[b].GetPosition);
        }
        points.Add(targetPosition);

        return points;
    }
    public static int DefiningCellIdByPosition(Vector3 position)
    {
        for(int i = 0; i < _cells.Count; i++) 
        {
            float minX = _cells[i].GetPosition.x - _cells[i].GetSize.x / 2;
            float maxX = _cells[i].GetPosition.x + _cells[i].GetSize.x / 2;
            float minZ = _cells[i].GetPosition.z - _cells[i].GetSize.y / 2;
            float maxZ = _cells[i].GetPosition.z + _cells[i].GetSize.y / 2;

            if(position.x > minX && position.x < maxX && position.z > minZ && position.z < maxZ) return _cells[i].GetId;
        }
        return 0;
    }
    public static Vector3 GetFreePointByCell(Cell cell)
    {
        for(int i = 0; i < cell.GetTargetPoints.Count; i++)
        {
            if (cell.GetTargetPoints[i].IsFree) return cell.GetTargetPoints[i].GetPoint;
        }
        return Vector3.zero;
    }
    public static Cell GetNextCell(int currentCellId, int deltaNumber)
    {
        int targetCellId = (currentCellId + deltaNumber < _cells.Count) ? currentCellId + deltaNumber : currentCellId + deltaNumber - _cells.Count;
        return _cells[targetCellId];
    }
    public static CellProperty GetCellPropertyById(int id)
    {
        if (_cells[id] is CellProperty) return (CellProperty)_cells[id];
        else return null;
    }

    public static List<CellProperty> GetCellProperties()
    {
        List<CellProperty> cellProperties = new List<CellProperty>();
        for (int i = 0; i < _propertyCellsId.Count; i++)
        {
            cellProperties.Add((CellProperty)_cells[_propertyCellsId[i]]);
        }
        return cellProperties;
    }

    public static bool CheckFullGroup(CellGroupType cellGroupType)
    {
        var owner = _propertyCellGroups[cellGroupType][0].Owner;
        return _propertyCellGroups[cellGroupType].All(s => s.Owner == owner);
    }

    public static Cell GetCellById(int id) 
    {
        return _cells[id]; 
    }
}
