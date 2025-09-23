using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Attack/Melee Attack Clip", fileName ="MA_")]
public class AIMeleeAttackClip : AIAttackClip
{
    public float baseAnimSpeed = 1f;
    public AttackStrengthType[] attackStrengthType;
    public SoundList[] randomAttackVoiceSound ;
    [Header("���� ����Ʈ")]
    public ControllerEffectInfo[] attackEffect = null;

    [Header("������ ����Ʈ")]
    public RandomEffectInfo[] hitEffect = null;


    [Tooltip("�ش� ������ ���� ������ �Է�")]
    public float[] attackTimingFrame;

    public bool isFinalClip = false;
    public AIMeleeAttackClip nextAttackClip = null;

    [Header("Melee Attack Settings")]
    public float detectRange = 7f;
    public List<float> damage = new List<float>();
    public List<float> attackAngle = new List<float>();
    public List<float> attackRange = new List<float>();
    public int maxTargetCount = 0;





    public float[] GetTimingFrameToTime(float attackSpeed)
    {
        if (attackTimingFrame.Length <= 0) return null;

        float frame = 1f / (attackClip.frameRate * attackSpeed);
        float[] retTiming = new float[attackTimingFrame.Length];

        for (int i = 0; i < attackTimingFrame.Length; i++)
            retTiming[i] = attackTimingFrame[i] * frame;

        return retTiming;
    }

    
}
