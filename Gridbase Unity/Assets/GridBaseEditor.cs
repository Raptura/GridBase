using UnityEngine;
using System.Collections;
using UnityEditor;

[CanEditMultipleObjects]
public class GridBaseEditor : EditorWindow
{
    //Grid Object Information
    public HexGrid grid;
    int groupSize = 5; //default value of 5
    float cellSize = 1.0f; //default value of 1
    int x = 0, y = 0; //default 0, 0 values;

    Material cellMat;


    [MenuItem("Window/GridBase/HexGrid")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(GridBaseEditor));
    }


    void OnEnable()
    {

    }

    void OnGUI()
    {
        GridGUI();
        EditorGUILayout.Separator();
        CellGUI();
    }


    void GridGUI()
    {
        EditorGUILayout.LabelField("Grid Base Version: 0.01");
        EditorGUILayout.LabelField("Hex Grid Information");

        grid = (HexGrid)EditorGUILayout.ObjectField(grid, typeof(HexGrid), allowSceneObjects: true);

        if (grid != null)
        {
            foreach (Cell cell in grid.cells)
            {
                cell.drawCellDebug();
            }

            //Group Size

            EditorGUILayout.PrefixLabel("Group Size: " + groupSize);
            groupSize = EditorGUILayout.IntField(groupSize);

            //Positioning
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("X: ");
            x = EditorGUILayout.IntField(x);
            EditorGUILayout.PrefixLabel("Y: ");
            y = EditorGUILayout.IntField(y);
            EditorGUILayout.EndHorizontal();


            if (GUILayout.Button("Create Cell"))
            {
                Cell.createCell(grid, x, y, grid.cellSize);
            }
            if (GUILayout.Button("Create Cell Group"))
            {
                grid.createCellGroup(x, y, groupSize);
            }
        }
        else {

            //Cell Size
            EditorGUILayout.PrefixLabel("Cell Size");
            cellSize = EditorGUILayout.FloatField(cellSize);

            if (GUILayout.Button("Create Grid"))
            {
                GameObject newGrid = new GameObject("Hex Grid");
                HexGrid hex = newGrid.AddComponent<HexGrid>();
                hex.hasFlatTop = true;
                hex.cellSize = cellSize;

                grid = hex;
            }
        }
    }

    void CellGUI()
    {
        EditorGUILayout.LabelField("Cell Information");
        if (grid != null)
        {
            if (grid.cells.Count == 0)
            {
                EditorGUILayout.LabelField("Please Create Cells in your Hex Grid Object");
            }
            else {

            }

        }
        else {
            EditorGUILayout.LabelField("Please Specify a Hex Grid");
        }
    }

    void cellConform()
    {

    }
}
