using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(CustomPivotMesh))]
public class CustomPivot : Editor
{

    Mesh mesh;
    MeshFilter meshfilter;
    GameObject objCurrent, objLast;
    Vector3 p, last_p;
    Vector3[] vertices;
    Vector3 poObjCurrent;
    private static readonly string[] _dontIncludeMe = new string[] { "m_Script","m_name" };

    ///<summary>
    ///Hide script in Inspector
    ///</summary>
    /// <example> 
    /// Method will active remove script current in Inspectory
    /// <code>
    ///  ...
    ///   DrawPropertiesExcluding(serializedObject, _dontIncludeMe);
    ///  ...
    /// </code>
    /// </example>
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, _dontIncludeMe);

        serializedObject.ApplyModifiedProperties();
    }
    /// <summary>  
    ///  This function update handle pivot of GameObject for Meshfilter
    /// </summary> 
	///<param name="b">Bound of mesh</param>  
    void UpdatePivotVetor()
    {
        var b = mesh.bounds;
        Vector3 offset = -1 * b.center;
        p = last_p = new Vector3(offset.x / b.extents.x, offset.y / b.extents.y, offset.z / b.extents.z);
    }

    ///<summary>
    ///Function callback when pointer mouse select GameObject in hierachy
    ///</summary>
    void OnSelectionChange()
    {
        if (Selection.activeTransform)
        {

            objCurrent = Selection.activeTransform.gameObject;
            poObjCurrent = objCurrent.transform.position;
            meshfilter = Selection.activeTransform.gameObject.GetComponent<MeshFilter>();
            if (meshfilter)
            {
                mesh = meshfilter.mesh;
                if (mesh)
                    UpdatePivotVetor();
            }
            objCurrent.GetComponent<CustomPivotMesh>().hideFlags = HideFlags.HideAndDontSave;

        }
        else
        {
            objCurrent = null;
        }
        Repaint();
    }
    void OnSceneGUI()
    {
        if (Selection.activeTransform)
        {
            OnSelectionChange();
        }

        Handles.BeginGUI();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(300));
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.gray;
        EditorGUILayout.Space();
        style.fontSize = 15;
        style.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("Handle Pivot", style);
        p.x = EditorGUILayout.Slider("X", p.x, -1, 1);
        p.y = EditorGUILayout.Slider("Y", p.y, -1, 1);
        p.z = EditorGUILayout.Slider("Z", p.z, -1, 1);


        if (objCurrent)
        {
            if (mesh)
            {
                if (p != last_p)
                {
                    ConvertMesh();
                }
                if (GUILayout.Button("Center"))
                {
                    p = Vector3.zero;
                    ConvertMesh();
                }
            }
        }

        EditorGUILayout.Space();
        objCurrent = (GameObject)EditorGUILayout.ObjectField(objCurrent, typeof(GameObject));
        if (objCurrent)
        {
            if (mesh)
            {
                GUILayout.Label("pivot: " + mesh.bounds.center);
                GUILayout.Label("vertices: " + mesh.vertices.Length);
                GUILayout.Label("triangles: " + mesh.triangles.Length);
            }


        }

        EditorGUILayout.EndVertical();
        Handles.EndGUI();
        if (objCurrent)
        {
            Handles.color = Color.green;
            Handles.DrawSphere(1, poObjCurrent, Quaternion.identity, 0.1f);
            Handles.color = Color.red;
            Handles.Label(poObjCurrent, "Pivot");
        }

    }



    void ConvertMesh()
    {
        vertices = null;
        Vector3 diff = Vector3.Scale(mesh.bounds.extents, last_p - p);
        objCurrent.transform.position -= Vector3.Scale(diff, objCurrent.transform.localScale); //Move object position
        vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] += diff;
        }
        mesh.vertices = vertices;

        mesh.RecalculateBounds();
        last_p = p;
    }
}
