using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Attack Data/Damaged Data")]
public class DamagedClip : ScriptableObject
{
    [SerializeField] private bool rotateToAttacker = false;
    [SerializeField] private AttackStrengthType strengthType = AttackStrengthType.NONE;
    [SerializeField] private AnimationClip damagedClip = null;
    [SerializeField] private string animationName = string.Empty;
    [SerializeField] private int endAnimationFrame = 0;
    [SerializeField] private float clipFullFrame = 0;
    [SerializeField] private CameraShakeInfo cameraShakeInfo = null;

    [Header("FlyDown Settings")]
    [SerializeField] private bool isFlyDown = false;
    [SerializeField] private float riseTime = 0f;   //1�̸� �ٿ��� 1�ʵ� rise ����.

    [Tooltip("������ ������ �ִ� ������. ex) endFrame�� 20�̰�," +
        " canDamagedFrame�� 15�� ��� 15������ ���ʹ� �ٽ� �������� ������ ����. �ٸ� ����� 20�����ӿ���.")]
    [SerializeField] private int canDamagedFrame = 0;
    [SerializeField] private float animationPlaySpeed = 1f;

    public bool IsFlyDown => isFlyDown;
    public float RiseTime => riseTime;
    public bool RotateToAttacker => rotateToAttacker;
    public AttackStrengthType StrengthType => strengthType;
    public string AniamtionName => animationName;
    public float AnimationPlaySpeed => animationPlaySpeed;
    public int EndAnimationFrame => endAnimationFrame;
    public CameraShakeInfo CameraShakeInfo => cameraShakeInfo;
    public float EndAnimationFrameToTime() => FrameToTime(endAnimationFrame);
    public float CanDamagedFrameToTime() => FrameToTime(canDamagedFrame);

    public float FrameToTime(int frame)
    {
        float rate = 1 / (30f * animationPlaySpeed);
        return frame * rate;
    }

    private void OnValidate()
    {
        if (damagedClip != null)
            clipFullFrame = (int)(damagedClip.length * 30f);
    }
}
