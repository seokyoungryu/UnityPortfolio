using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTargetMarker : MonoBehaviour
{
    //���� active�� ����Ʈ����� �˻��ؼ� ? ����  -> ����Ʈ�� active�ɶ�
    //Ÿ�� �ڽĿ� �� ��ũ��Ʈ�� �ִ� ī�װ��� , Ÿ���� �����ϸ� Ÿ�ٸ�Ŀ ��
    public TaskTarget thisTarget = null;
    private Renderer renderer = null;
    public QuestTargetMaterial[] materials;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        QuestManager.Instance.onRegister += EntryMethod;
        QuestManager.Instance.onRegister += CheckIsTargetMarker;

        QuestManager.Instance.onRegisterLoadQuest += EntryMethod;
        QuestManager.Instance.onRegisterLoadQuest += CheckIsTargetMarker;

        renderer.enabled = false;
    }


    private void OnDestroy()
    {
        QuestManager.Instance.onRegister -= EntryMethod;
        QuestManager.Instance.onRegister -= CheckIsTargetMarker;

        QuestManager.Instance.onRegisterLoadQuest -= EntryMethod;
        QuestManager.Instance.onRegisterLoadQuest -= CheckIsTargetMarker;
    }


    public void OnEnable()
    {
        CheckIsTarget();
        //���⿡ Ÿ�� Ȯ��. 
    }


    public void SetTaskTarget(TaskTarget target)
    {
        thisTarget = target;
        //���⿡ Ÿ�� Ȯ�� ,
        CheckIsTarget();
    }




    public void CheckIsTarget()
    {
        if (thisTarget == null) return;

        Quest[] quests = QuestManager.Instance.activeQuests.ToArray();
        for (int i = 0; i < quests.Length; i++)
        {
            for (int j = 0; j < materials.Length; j++)
            {
                if (quests[i].CheckIsTargetMarker(materials[j].category, thisTarget))
                {
                    renderer.material = materials[j].material;
                    renderer.enabled = true;
                }
            }
        }

    }


    public void EntryMethod(Quest quest, Task task1)
    {
       // Debug.Log(gameObject.transform.parent.name + " - EntryMethod IN");


        foreach (TaskGroup taskGroup in quest.TaskGroups)
        {
            foreach (Task task in taskGroup.Tasks)
            {
                task.OnReceiveReport += CheckIsTargetMarker;
                task.OnComplete += CheckIsDisable;
            }
        }
    }


    public void CheckIsDisable(Quest quest, Task task)
    {
        if (quest == null || task == null)
            return;

        if (task.IsOnlyTargetCheck(thisTarget))
        {
            if (task.TaskState != TaskState.RUNNING || quest.QuestState != QuestState.RUNNING)
                renderer.enabled = false;
        }
    }


    public void CheckIsTargetMarker(Quest quest, Task task)
    {
       // Debug.Log(gameObject.transform.parent.name + " - CheckIsTargetMarker IN");
        if (quest == null) return;

        foreach (QuestTargetMaterial mat in materials)
        {
            if (quest.CheckIsTargetMarker(mat.category, thisTarget))
            {
                if (renderer == null) continue;
                renderer.material = mat.material;
                renderer.enabled = true;
            }

        }

        if (task == null) return;

        if (task.TaskState != TaskState.RUNNING || quest.QuestState != QuestState.RUNNING )
        {
           // Debug.Log(gameObject.transform.parent.parent.name + " : " + thisTarget?.Value + " �����1");
            renderer.enabled = false;
        }
    }


}
