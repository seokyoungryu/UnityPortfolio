using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Quest/Conditions/Require Condition/Player Level Conditon", fileName = "LevelCondition_")]
public class PlayerLvCondition : QuestCondition
{
    [Header("PlayerLv�� Value���� ���ٸ� false")]
    [SerializeField] private int playerLvValue = 1;
    public override bool IsPass(Quest quest)
    {
        if (GameManager.Instance.Player.playerStats.Level < playerLvValue)
            return false;
        return true;
    }
}
