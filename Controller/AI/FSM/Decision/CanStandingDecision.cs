using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIStandingType { ATTACK_COUNT = 0, Full_COMBO = 1}

[CreateAssetMenu(menuName = "AI/Decisions/Can Standing")]
public class CanStandingDecision : Decision
{
    //���ĵ� Ÿ��.
    //1. ���� �ѹ�.
    //2. 1 �޺�.

    public override bool Decide(AIController controller)
    {
        if (controller.aiConditions.IsGroggying) return false;
        if (controller.aiConditions.CanStanding && !controller.aiConditions.IsStanding)
            return true;

        return false;
    }

}
