using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIPhaseAttackData
{
    public string phaseName = string.Empty;
    public int phaseCount = 1;
    [Header("��ü ü�� 100% �� �� %�� ����� ���� ���� (1~100%)")]
    public float phasePercent = 0f;
    [Header("������ �ܰ�")]
    public string phaseAnimationClipName = string.Empty;
    public AnimationClip animClip = null;
    public float animFullFrame = 0f;
    public float phaseAnimationEndFrame = 0f;
    public float animationSpeed = 1f;
    public float waitEndTime = 0f;
    [TextArea(0, 2)]
    public string globalNotifier = string.Empty;
    public BaseSkillClip[] unlockPhaseSkills = null;
    public UseableObject[] applyUseableObjs = null;



}


