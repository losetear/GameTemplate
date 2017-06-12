using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class GenRacePath : EditorWindow
{
    private string _pointNum;
    private string _radius;
    private string _straightLength;
    private string _scale;
    private string _posy;
    private Transform _pathRoot;
    private Transform _root;
    private int _factor = 1;

    [MenuItem("Tools/生成跑道路径")]
    public static void GenTargetRacePath()
    {
        GenRacePath window = (GenRacePath)EditorWindow.GetWindow(typeof(GenRacePath));
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("生成点的数量(必须是偶数)");
        _pointNum = EditorGUILayout.TextArea(_pointNum);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("跑道半径");
        _radius = EditorGUILayout.TextArea(_radius);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("跑道直线长度");
        _straightLength = EditorGUILayout.TextArea(_straightLength);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("模型大小");
        _scale = EditorGUILayout.TextArea(_scale);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Y轴高度");
        _posy = EditorGUILayout.TextArea(_posy);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("父节点");
        _root = EditorGUILayout.ObjectField(_root, typeof(Transform)) as Transform;
        EditorGUILayout.EndHorizontal();

        if ((GUILayout.Button("生成")))
        {
            GenPath();
        }

    }

    void OnInspectorUpdate()
    {
        this.Repaint();   
    }

    void GenPath()
    {
        _pathRoot = _root.transform.Find("PathRoot");
        if (_pathRoot)
            DestroyImmediate(_pathRoot.gameObject);

        _pathRoot = new GameObject("PathRoot").transform;
        _pathRoot.transform.parent = _root;
        _pathRoot.transform.localPosition = Vector3.zero;
        _pathRoot.transform.localScale = Vector3.one;

        int pointNum = int.Parse(_pointNum);
        float radius = float.Parse(_radius);
        float straightLength = float.Parse(_straightLength);
        float scale = float.Parse(_scale);
        float posy = float.Parse(_posy);
        int index = 0;

        float startAngle = Mathf.PI/2;
        float intervalAngle = 2 * Mathf.PI / pointNum;
        float currentAngle = startAngle;

        while (currentAngle < 2.5f * Mathf.PI)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = string.Format("pos_{0:D2}", index);
            go.transform.parent = _pathRoot.transform;
            go.transform.localScale = Vector3.one * scale;
            float x = radius * Mathf.Cos(currentAngle);
            float z = radius * Mathf.Sin(currentAngle);
            index++;

//            if (Mathf.Abs(x) <= 0.01f)
//            {
//                GameObject copyGo = Instantiate(go);
//                copyGo.name = string.Format("pos_{0:D2}", index);
//                copyGo.transform.parent = _pathRoot.transform;
//                copyGo.transform.localScale = Vector3.one * scale;
//                copyGo.transform.localPosition = new Vector3(x - _factor * straightLength / 2, posy, z);
//                x += _factor * straightLength / 2;
//                index++;
//                _factor *= -_factor;
//            }
            if (x < 0)
                x -= straightLength / 2;
            else if (x > 0)
                x += straightLength / 2;

            go.transform.localPosition = new Vector3(x, posy, z);
            currentAngle += intervalAngle;

        }
    }
}
#endif
