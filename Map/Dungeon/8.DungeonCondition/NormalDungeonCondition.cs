using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Dungeon Condition/ Normal Condition", fileName = "BDCondition_")]
public class NormalDungeonCondition : BaseDungeonCondition
{
    [SerializeField] private int minPlayerLv = -1;
    [SerializeField] private int minDamaged = -1;
    [SerializeField] private int minReputation = -1;

    [Header("�ʼ� Ŭ���� Ÿ��Ʋ")]
    [SerializeField] private BaseDungeonTitle[] requiredTitle;

    public override DungeonDetailConditionTask[] CreateDungeonConditionUIs(string detailConditionUITaskOBP)
    {
        List<DungeonDetailConditionTask> retTasks = new List<DungeonDetailConditionTask>();

        if (minPlayerLv > 0) retTasks.Add(CreateLvCondition(detailConditionUITaskOBP));
        if (minDamaged > 0) retTasks.Add(CreateDamagedCondition(detailConditionUITaskOBP));
        if (minReputation > 0) retTasks.Add(CreateReputationCondition(detailConditionUITaskOBP));
        if (requiredTitle.Length > 0)
            for (int i = 0; i < requiredTitle.Length; i++)
                retTasks.Add(CreateTitleCondition(detailConditionUITaskOBP, requiredTitle[i]));

        return retTasks.ToArray();
    }


    private DungeonDetailConditionTask CreateLvCondition(string detailConditionUITaskOBP)
    {
        DungeonDetailConditionTask lvTask = ObjectPooling.Instance.GetOBP(detailConditionUITaskOBP).GetComponent<DungeonDetailConditionTask>();
        lvTask.Setting("�ּ� ���� ���� ", minPlayerLv);
        lvTask.ConditionType = DetailConditionType.LV;
        return lvTask;
    }

    private DungeonDetailConditionTask CreateDamagedCondition(string detailConditionUITaskOBP)
    {
        DungeonDetailConditionTask damagedTask = ObjectPooling.Instance.GetOBP(detailConditionUITaskOBP).GetComponent<DungeonDetailConditionTask>();
        damagedTask.Setting("�ּ� ������ ", minDamaged);
        damagedTask.ConditionType = DetailConditionType.DAMAGED;
        return damagedTask;
    }

    private DungeonDetailConditionTask CreateReputationCondition(string detailConditionUITaskOBP)
    {
        DungeonDetailConditionTask reputationTask = ObjectPooling.Instance.GetOBP(detailConditionUITaskOBP).GetComponent<DungeonDetailConditionTask>();
        reputationTask.Setting("�ּ� ��ġ ", minReputation);
        reputationTask.ConditionType = DetailConditionType.REPUTATION;
        return reputationTask;
    }

    private DungeonDetailConditionTask CreateTitleCondition(string detailConditionUITaskOBP, BaseDungeonTitle title)
    {
        DungeonDetailConditionTask titleTask = ObjectPooling.Instance.GetOBP(detailConditionUITaskOBP).GetComponent<DungeonDetailConditionTask>();
        titleTask.Setting(title.TaskTarget.DisplayName + " Ŭ���� �ʿ�", title);
        titleTask.ConditionType = DetailConditionType.TITLE;
        return titleTask;
    }


}
