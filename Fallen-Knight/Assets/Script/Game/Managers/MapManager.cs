using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public List<GameObject> _mapWaypoints = new List<GameObject>();
    public List<Vector3> _mapCellTrajectory = new List<Vector3>();
    public List<Vector3> _mapPath = new List<Vector3>();
}
