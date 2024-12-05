using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(GridCell))]
public class GridCellCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space(10);
        
        EditorGUILayout.LabelField("Grid Properties", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
        
        EditorGUILayout.LabelField("Cell Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_centerCell"), new GUIContent("Center Cell"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_cellParent"), new GUIContent("Cell Container"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_cellCenterSize"), new GUIContent("Cell Center Size"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_cellSize"), new GUIContent("Cell Size"));
        EditorGUILayout.Space(5);
        
        EditorGUILayout.LabelField("Grid Size", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_width"), new GUIContent("Width"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_height"), new GUIContent("Height"));
        EditorGUILayout.Space(5);
        
        EditorGUILayout.LabelField("Axe to Draw Grid", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_axis"), new GUIContent("Axis"));
        EditorGUILayout.Space(10);
        
        EditorGUILayout.LabelField("Brush Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_elements"), new GUIContent("Grid Elements"), true);
        
        serializedObject.ApplyModifiedProperties();
    }

    private void SerializeProperty(string variable)
    {
        SerializedProperty serializedProperty = serializedObject.FindProperty(variable);
        EditorGUILayout.PropertyField(serializedProperty);
    }
}
#endif
[ExecuteInEditMode]
public class GridCell : MonoBehaviour
{
    static GridCell()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }
    
    //Cell Properties
    [SerializeField] private GameObject _centerCell;
    [SerializeField] private GameObject _cellParent;
    [Range(0f, 1f)][SerializeField] private float _cellCenterSize;
    [SerializeField] private float _cellSize;
    private float CellCenterSize;
    
    //Grid Size
    [SerializeField] private int _width;
    private int Width;
    [SerializeField] private int _height;
    private int Heigth;
    
    //Axis
    [SerializeField] private Axis _axis;
    
    //Brush Properties
    [SerializeField] private List<Cell> _elements = new List<Cell>();
    private List<GameObject> _centerCells = new List<GameObject>();

    private enum Axis
    {
        XY,
        XZ,
        YZ
    }

    private void Update()
    {
        if (_width != Width)
        {
            Width = _width;
            DrawCellCenterBasedAxis();
        }
        if (_height != Heigth)
        {
            Heigth = _height;
            DrawCellCenterBasedAxis();
        }

        if (_cellCenterSize != CellCenterSize)
        {
            CellCenterSize = _cellCenterSize;
            DrawCellCenterBasedAxis();
        }
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        Event currentEvent = Event.current;
        
        if (currentEvent.type == EventType.MouseDown || currentEvent.type == EventType.MouseDrag)
        {
            Vector2 mousePosition = currentEvent.mousePosition;
            
            mousePosition.y = sceneView.camera.pixelHeight - mousePosition.y;
            
            Ray ray = sceneView.camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log($"Mouse Position in World: {hit.point}");
            }
        }
    }

    private void OnDrawGizmos()
    {
        DrawGridBasedOnAxis();
    }
    
    private void DrawGridBasedOnAxis()
    {
        Gizmos.color = Color.black;
        switch (_axis)
        {
            case Axis.XY:
                for (int x = 0; x <= _width; x++)
                {
                    Gizmos.DrawLine(new Vector3(x * _cellSize, 0, 0), new Vector3(x * _cellSize, _height * _cellSize, 0));
                }

                for (int y = 0; y <= _height; y++)
                {
                    Gizmos.DrawLine(new Vector3(0, y * _cellSize, 0), new Vector3(_width * _cellSize, y * _cellSize, 0));
                }
                break;

            case Axis.XZ:
                for (int x = 0; x <= _width; x++)
                {
                    Gizmos.DrawLine(new Vector3(x * _cellSize, 0, 0), new Vector3(x * _cellSize, 0, _height * _cellSize));
                }

                for (int z = 0; z <= _height; z++)
                {
                    Gizmos.DrawLine(new Vector3(0, 0, z * _cellSize), new Vector3(_width * _cellSize, 0, z * _cellSize));
                }
                break;

            case Axis.YZ:
                for (int y = 0; y <= _width; y++)
                {
                    Gizmos.DrawLine(new Vector3(0, y * _cellSize, 0), new Vector3(0, y * _cellSize, _height * _cellSize));
                }

                for (int z = 0; z <= _height; z++)
                {
                    Gizmos.DrawLine(new Vector3(0, 0, z * _cellSize), new Vector3(0, _width * _cellSize, z * _cellSize));
                }
                break;
        }
    }

    private void DrawCellCenterBasedAxis()
    {
        if (_centerCell == null) return;

        foreach (var centerCell in _centerCells)
        {
            DestroyImmediate(centerCell);
        }
        _centerCells.Clear();
        
        Gizmos.color = Color.grey;
        switch (_axis)
        {
            case Axis.XY:
                for (int x = 0; x < _width; x++)
                {
                    for (int y = 0; y < _height; y++)
                    {
                        GameObject centerCell =  Instantiate(_centerCell, new Vector3(x * _cellSize + _cellSize / 2, y * _cellSize + _cellSize / 2, 0), Quaternion.identity);
                        centerCell.transform.localScale = new Vector3(_cellSize * _cellCenterSize, _cellSize * _cellCenterSize, _cellSize * _cellCenterSize);
                        _centerCells.Add(centerCell);
                        centerCell.transform.parent = _cellParent.transform;
                    }
                }
                break;
            case Axis.XZ:
                for (int x = 0; x < _width; x++)
                {
                    for (int z = 0; z < _height; z++)
                    {
                        GameObject centerCell =Instantiate(_centerCell , new Vector3(x * _cellSize + _cellSize / 2, 0, z * _cellSize + _cellSize / 2), Quaternion.identity);
                        centerCell.transform.localScale = new Vector3(_cellSize * _cellCenterSize, _cellSize * _cellCenterSize, _cellSize * _cellCenterSize);
                        _centerCells.Add(centerCell);
                        centerCell.transform.parent = _cellParent.transform;
                    }
                }
                break;
            case Axis.YZ:
                for (int y = 0; y < _width; y++)
                {
                    for (int z = 0; z < _height; z++)
                    {
                        GameObject centerCell =Instantiate(_centerCell , new Vector3(0, y * _cellSize + _cellSize / 2, z * _cellSize + _cellSize / 2), Quaternion.identity);
                        centerCell.transform.localScale = new Vector3(_cellSize * _cellCenterSize, _cellSize * _cellCenterSize, _cellSize * _cellCenterSize);
                        _centerCells.Add(centerCell);
                        centerCell.transform.parent = _cellParent.transform;
                    }
                }
                break;
        }
    }
}