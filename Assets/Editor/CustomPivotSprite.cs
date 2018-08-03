using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PivotSprite))]
public class CustomPivotSprite : Editor
{
    enum typeLL { a, b };
    typeLL typel = typeLL.a;
    Vector3 poRoot, poLast;
    Sprite sprite;
    Bounds bound;
    GameObject obj;
    Vector2 pivotCutom = Vector2.zero;
    Vector2 pivotCenter = new Vector2(0.5f, 0.5f);
    Vector2 pointPivotCutom = Vector2.zero;
    bool isPivotCutom = false;
    SpriteRenderer spriterender;

    // [MenuItem("Tools/Sprite")]
    // public static void Init()
    // {
    //     GetWindow<CustomPivotSprite>("sprite pivot").Show();
    // }
    void OnEnable()
    {
        setObjSelectionTranfrom();

    }
    void setObjSelectionTranfrom()
    {
        if (Selection.activeTransform)
        {
            obj = Selection.activeTransform.gameObject;
            obj.transform.localScale = new Vector3(1,1,1);
            spriterender = obj.GetComponent<SpriteRenderer>();
            sprite = spriterender.sprite;
            bound = spriterender.bounds;
            poRoot = obj.transform.position;
         //   pointPivotCutom = poRoot;
            pivotCutom = new Vector2(0.5f, 0.5f);
            if (poLast != obj.transform.position)
            {
                poRoot = bound.center;

            }
            poLast = obj.transform.position;
            pointPivotCutom = poLast;
            float disX = (pointPivotCutom.x - poRoot.x);
            float disY = (pointPivotCutom.y - poRoot.y);
            pivotCutom.x = ((float)(disX + bound.extents.x) / (bound.extents.x * 2));
            pivotCutom.y = ((float)(disY + bound.extents.y) / (bound.extents.y * 2));
        }
        else
        {
            Reset();
        }

        Repaint();
    }
    ///<Summary>
    ///function call when selection is exsist object 
    ///</Summary>
    public void OnSelectionChange()
    {
        setObjSelectionTranfrom();

    }
    void OnFocus()
    {
        Debug.Log("windown is focus");
        setObjSelectionTranfrom();
        Debug.Log("updated poRoot complete");

    }

    void OnSceneGUI()
    {

        if (poLast != obj.transform.position)
        {
            setObjSelectionTranfrom();
        }
        Handles.BeginGUI();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(300));
        GUIStyle styple = new GUIStyle();
        styple.fontSize = 20;
        styple.alignment = TextAnchor.MiddleCenter;
        styple.normal.textColor = Color.gray;
        GUILayout.Label("Custom Pivot Sprite", styple);
        GUILayout.Space(10);
        GUILayout.Label("Select Pivot Sprite Renderer", "flow varPin tooltip");
        GUILayout.Space(10);
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

        if (obj)
        {
            EditorGUILayout.BeginVertical();




            if (GUILayout.Button("LeftTop"))
            {
                UpdatePivot(new Vector2(0, 1));
                Repaint();

            }
            if (GUILayout.Button("LeftCenter"))
            {
                UpdatePivot(new Vector2(0, 0.5f));
                Repaint();

            }
            if (GUILayout.Button("LeftBottom"))
            {
                UpdatePivot(Vector2.zero);
                Repaint();

            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();


            if (GUILayout.Button("MidTop"))
            {
                UpdatePivot(new Vector2(0.5f, 1f));
                Repaint();

            }
            if (GUILayout.Button("MidCenter"))
            {
                UpdatePivot(new Vector2(0.5f, 0.5f));
                Repaint();

            }
            if (GUILayout.Button("MidBottom"))
            {
                UpdatePivot(new Vector2(0.5f, 0f));
                Repaint();

            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();


            if (GUILayout.Button("RightTop"))
            {
                UpdatePivot(new Vector2(1, 1));
                Repaint();

            }
            if (GUILayout.Button("RightCenter"))
            {
                UpdatePivot(new Vector2(1, 0.5f));
                Repaint();

            }
            if (GUILayout.Button("RightBottom"))
            {
                UpdatePivot(new Vector2(1, 0));
                Repaint();

            }
            EditorGUILayout.EndVertical();
            if (GUILayout.Button("Cutom", GUILayout.ExpandHeight(true)))
            {
                isPivotCutom = true;
                Repaint();

            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("Infomation Sprite Renderer", "flow varPin tooltip");
            GUILayout.Space(10);

            if (isPivotCutom)
            {
                EditorGUILayout.BeginVertical();
                GUILayout.Label("---------------------------------------------------");
                GUILayout.Label("pivotCutom: " + pivotCutom);
                GUILayout.Label("pointPivotCutom: " + pointPivotCutom);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Apply", GUILayout.Width(70)))
                {
                    UpdatePivot(pivotCutom);
                    isPivotCutom = false;
                }
                if (GUILayout.Button("Cancle", GUILayout.Width(70)))
                {
                    isPivotCutom = false;
                }
                EditorGUILayout.EndHorizontal();
                GUILayout.Label("---------------------------------------------------");
                EditorGUILayout.EndVertical();
            }
            else
            {
                if (sprite)
                {
                    pointPivotCutom = poLast;
                  //  pivotCutom = new Vector2(0.5f, 0.5f);
                }

            }
            if (spriterender)
            {
                GUILayout.Label("Name Sprite: " + sprite.texture.name);
                GUILayout.Label("Center: " + bound.center);
                GUILayout.Label("Extents: " + bound.extents);
                GUILayout.Label("pivot: " + sprite.pivot);
                GUILayout.Label("Rect: " + sprite.rect.size);
                GUILayout.Label("PoRoot: " + poRoot);

            }
        }
        else
        {
            GUILayout.Label("Object is null!!");

        }
        Handles.EndGUI();
        if (isPivotCutom)
        {
            pointPivotCutom = Handles.PositionHandle(pointPivotCutom, Quaternion.identity);
            pivotCutom = new Vector2((pointPivotCutom.x * 0.5f) / poRoot.x, (pointPivotCutom.y * 0.5f) / poRoot.y);
            Handles.color = Color.red;
            Handles.DrawSphere(1, pointPivotCutom, Quaternion.identity, 0.13f);
            Handles.color = Color.green;
            Handles.DrawSphere(1, pointPivotCutom, Quaternion.identity, 0.1f);
            Handles.Label(pointPivotCutom,"Point Pivot");
            float disX = (pointPivotCutom.x - poRoot.x);
            float disY = (pointPivotCutom.y - poRoot.y);
            pivotCutom.x = ((float)(disX + bound.extents.x) / (bound.extents.x * 2));
            pivotCutom.y = ((float)(disY + bound.extents.y) / (bound.extents.y * 2));


        }

    }

    void Reset()
    {
        spriterender = null;
        obj = null;
        sprite = null;
    }

    void UpdatePivot(Vector2 pivot)
    {
        if (!spriterender)
        {
            Debug.Log("Sprite Rendering is null");
        }
        else
        {
            Sprite sp = Sprite.Create(sprite.texture, sprite.rect, pivot, sprite.pixelsPerUnit);
            string nameTexture = sprite.texture.name + " instance";
            sp.name = nameTexture;
            spriterender.sprite = sp;
            bound = sp.bounds;
            sprite = sp;
            Vector3 center = -1 * bound.center;
            Vector2 pivotCurrent = sp.pivot;
            Vector3 poResult = (poRoot + new Vector3(center.x, center.y, 0));
            obj.transform.position = poResult;
        }
    }



}
