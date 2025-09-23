using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ComboMouseInputType { LEFT = 0 , RIGHT = 1,}




[CreateAssetMenu(menuName = "Data/Attack Data/Combo Data", fileName = "Combo_")]
public class ComboClip : ScriptableObject
{
    [Header("Input")]
    public List<ComboMouseInputType> comboInputKey = new List<ComboMouseInputType>();
    private List<int> inputKeys = new List<int>();

    [Header("Animation")]
    public string attackName = string.Empty;
    public string animationName = "Attack";
    public AnimationClip animationClip;
    public float clipFullFrame = 0;
    [Tooltip("�޺��� ��� ����� ������ ����")]
    public float canComboInputFrame = 0f;  //��ȯ�Ϸ��� frame * 0.03333 => ����ð�
    [Tooltip("�ִϸ��̼� Ŭ�� ���� ������ ���� ")]
    public float attackEndAnimationFrame = 0f;
    [Tooltip("�ִϸ��̼� ���� �� ��� �ð�")]
    public float endForWaitTime = 0f;
    public int[] attackTimingFrame;


    [Header("Value")]
    public RandomEffectInfo[] hitEffectList = null;
    public ControllerEffectInfo[] DamageEffects = null;
    public AttackStrengthType[] attackStrengthType;
    public CameraShakeInfo[] attackShakeCam;
    [SerializeField] private TimeData hitTimeData = null;

    [Header("�ʱ� Stats")]
    [Tooltip("damage -> ���� ����. ex) damage = 1.1f�� currentDamage * 1.1f")]
    public float[] damage;
    public float attackAngle = 0f;
    public float attackRange = 0f;
    public int maxTargetCount = 0;
    public float staminaCost = 0f;


    [Header("Combo ���׷��̵�")]
    [SerializeField] private ComboUpgrade[] comboUpgrade = null;

    public TimeData GetTimeData => hitTimeData;
    public ComboUpgrade[] ComboUpgrade => comboUpgrade;


    public List<int> GetInputKeyInt()
    {
        RevertIntInputKey();
        return inputKeys;
    }


    public virtual float GetCanComboInputTime(float atkSpeed) => GetFrameToTime(atkSpeed, canComboInputFrame);

    public virtual float GetAttactEndAnimationFrameToTime(float atkSpeed) => GetFrameToTime(atkSpeed, attackEndAnimationFrame);

    public float GetFrameToTime(float speed, float frame)
    {
        float frameTime = 1f / (animationClip.frameRate * speed);
        return frameTime * frame;
    }

    public void Upgrade(int upgradeIndex)
    {
        //Debug.Log($"ComboUpgrade {upgradeIndex} / {comboUpgrade.Length}");
        if (upgradeIndex < 0 || comboUpgrade.Length <= upgradeIndex) return;

        this.damage = comboUpgrade[upgradeIndex].ComboInfo.Damage;
        this.attackAngle = comboUpgrade[upgradeIndex].ComboInfo.AttackAngle;
        this.attackRange = comboUpgrade[upgradeIndex].ComboInfo.AttackRange;
        this.maxTargetCount = comboUpgrade[upgradeIndex].ComboInfo.MaxTargetCount;
        this.staminaCost = comboUpgrade[upgradeIndex].ComboInfo.SpCost;

    }

    public void RevertIntInputKey()
    {
        if (comboInputKey == null || comboInputKey.Count == 0)
            inputKeys.Clear();

        if (inputKeys.Count != comboInputKey.Count)
        {
            inputKeys.Clear();
            for (int x = 0; x < comboInputKey.Count; x++)
                inputKeys.Add(0);
        }

        for (int i = 0; i < comboInputKey.Count; i++)
        {
            inputKeys[i] = (int)comboInputKey[i];

        }

    }

    private void OnValidate()
    {
        RevertIntInputKey();

        if (animationClip != null)
            clipFullFrame = animationClip.length * animationClip.frameRate;

        if(comboUpgrade.Length > 0)
        {
            for (int i = 0; i < comboUpgrade.Length; i++)
            {
                comboUpgrade[i].Name = "Upgrade " + (i + 1);
                comboUpgrade[i].ComboLv = (i + 1);
            }
        }
    }
}

