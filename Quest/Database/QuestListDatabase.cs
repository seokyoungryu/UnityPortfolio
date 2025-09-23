using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Database/Quest/Quest List Database", fileName = "QuestListDatabase")]
public class QuestListDatabase : BaseFindobjectDatabase<QuestList>
{

#if UNITY_EDITOR
    [ContextMenu("����Ʈ����Ʈ �ҷ�����")]
    private void Find()
    {
        FindAddDatas();
        SetID();
        SetDirtys();
    }
#endif

}
