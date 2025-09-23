using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameObjectsStaticSetting : MonoBehaviour
{
#if UNITY_EDITOR
    public List<GameObject> gos;
    public StaticEditorFlags flag;


    [ContextMenu("�迭 �ʱ�ȭ")]
    public void Clear() => gos.Clear();

    [ContextMenu("�����ϱ�")]
    public void Set()
    {
        for (int i = 0; i < gos.Count; i++)
            Setting(gos[i], flag);
    }


    public void Setting(GameObject go, StaticEditorFlags flag)
    {
        GameObjectUtility.SetStaticEditorFlags(go, flag);

        if (go.transform.childCount <= 0)
            return;

        foreach (Transform child in go.transform)
        {
            Setting(child.gameObject, flag);
        }
    }
#endif
}
