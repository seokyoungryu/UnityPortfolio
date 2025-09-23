using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potential/Max Hp Percentage", fileName = "MaxHpPecentagePotentialFunction")]
public class MaxHpPercentPotentialFunction : PotentialFunctionObject
{
    public override void Apply(float value, PlayerStatus playerStatus)
    {
        playerStatus.MaxHpPercentage += value;
        //���� ������ŭ ���簪�� ����.
        float additiveHp = playerStatus.CurrentHealth * value;
        playerStatus.SetCurrentHP(playerStatus.CurrentHealth + (int)additiveHp);
    }

    public override void Remove(float value, PlayerStatus playerStatus)
    {
        playerStatus.MaxHpPercentage -= value;
        float additiveHp = playerStatus.CurrentHealth * value;
        playerStatus.SetCurrentHP(playerStatus.CurrentHealth - (int)additiveHp);
    }

}
