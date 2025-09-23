using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum TargetType { NONE =-1, PLAYER =0, AI = 1,}

/// <summary>
/// FSM �� ���� ����.
/// </summary>
public class AIVariables : MonoBehaviour
{
    private TargetType targetType = TargetType.NONE;
    public BaseController target = null;
    public Vector3 targetVector = Vector3.zero;
    public Transform followTarget = null;

    public Color sightColor;
    public Color runSightColor;

    [Space(15f), Header("Sound")]
    public SoundList findNearSound = SoundList.Soldier_Hunter_Surprised_1;


    [Space(15f), Header("Combat Percent Settings ( 100 % )")]
    public float attackCombatPercentage = 70f;
    public float skillCombatPercentage = 30f;

    [Space(15f), Header("Nav Settings")]
    public float deadOffesetTime = 5f;

    [Space(15f),Header("Nav Settings")]
    public float walkSpeed  = 2f;
    public float runSpeed = 4.5f;
    public float angulerSpeed = 0f;
    public float stopDistance = 2f;

    [Space(15f),Header("Follow Settings")]
    public float followDistance = 3f;
    public float followRunSight = 7f;
    public float followCurrentSpeed = 0f;
    public float followWalkSpeed = 2f;
    public float followRunSpeed = 4.5f;
    public float followUpdateTime = 0.1f;
    public float followDeleyTime = 1.2f;

    [Space(15f),Header("Patrol Settings")]
    public float patrolSpeed = 0f;
    public float currentNextWayTimer = 0f;    //hide
    public float nextWayWaitTime = 0f;
    public float currentThisWayWaitTimer = 0f;  // hide
    public bool canNextWayPointTimer = false;   // hide

    [Space(15f),Header("Sight Range Settings")]
    public float sightRange = 0f;
    public float sightAngle = 0f;
    public float alertSightRange = 0f;
    public float runSightRange = 0f;
    public float currentSightRange = 0f;

    [Space(15f), Header("Attack & Skill Settings")]
    public float skillSmoothRotateAnguler = 0f;

    [Space(15f),Header("Phase Settings")]
    public int phaseLimit = 0;

   

    [Space(15f), Header("Rest Setting")]
    public float requireEnterRestTime = 3f;
    public float restWaitPerTime = 0f;
    public float restHealHealthPerValue = 0f;

    [Space(15f),Header("Alert Settings")]
    public float currentAlertTimer = 0f;
    public float maxAlertTime = 0f;
    public float minAlertTime = 0f;

    [Space(15f), Header("Groggy Settings")]
    public float groggyCoolTime = 15f;
    public float groggyDuration = 2f;
    public int groggyInAnimFullFrame = 0;
    public int groggyOutAnimFullFrame = 0;
    public int maxGroggyingCount = 0;
    [Tooltip("�׷α� �Ƿ��� HP ���ҷ�. ��ü HP�� �ۼ�Ʈ�� ���� ex) 10% => ��üü�� 100 �� 10�̻� ������������ ��� �׷α�.")]
    public float groggyingHpPercent = 0f;
    public float groggyingResetTime = 0f; // 5�ʸ� 5�ʸ��� �׷α� �� 0���� �ʱ�ȭ. (���� - �� �ð� �ȿ� �׷α� 100�� �Ǹ� �׷α� �Ǵ°���.


    [Space(15f), Header("Defense Settings")]
    public float defenseCoolTime = 3f;
    public int blockDefenseAnimFrame = 0;
    public float blockDefenseAnimSpeed = 1.2f;

    [Space(15f), Header("Standing Variables")]
    [SerializeField] private int standingAnimFrame = 120;
    [SerializeField] private float standingAnimSpeed = 1f;
    [SerializeField] private float standingCoolTime = 3f;

    [Space(15f), Header("Dash Settings")]
    public DashType dashType = DashType.NONE;
    public bool haveDashReadyMotion = false;
    public string dashReadyMotionAnimationName = string.Empty;
    public int dashReadyMotionEndFrame = 0;
    public float accelerateDashSpeed = 0f;
    public int limitDashCount = 0;  //���� ���� ��� Count ��
    public int maxDashCount = 0;
    public float maxDashCountResetCoolTime = 5f;
    public bool useRandomCoolTime = false;
    public Vector2 randomRangeDashCoolTime = new Vector2(1f, 5f);
    public float dashCoolTime = 15f;
    public Vector2 rangeDelayTeleportTime = new Vector2(0.1f , 1f); // ���� �� ������� ������ ���� �ð�.
    public Vector2 rangeCanDashTargetDistance = new Vector2(0f, 10f);  //�ּ� Ÿ�ٰ��� �Ÿ�. ( 4�̸�. �ּ� Ÿ�ٰ� �Ÿ��� 4��ŭ �������־����)
    public Vector2 rangeDashStopDistance = new Vector2(1f, 4f);
    public Vector2 rangeDashHeight = new Vector2(-1.5f, 1.5f);

    public float StandingAnimSpeed => standingAnimSpeed;
    public int StandingAnimFrame => standingAnimFrame;
    public float StandingCoolTime { get { return standingCoolTime; } set { standingCoolTime = value; } }


    private GUIStyle style = new GUIStyle();

    public BaseController Target => target;



    public void SetTarget(BaseController target)
    {
        if (target == null) targetType = TargetType.NONE;
        else if (target is AIController) targetType = TargetType.AI;
        else if (target is PlayerStateController) targetType = TargetType.PLAYER;

        this.target = target;
    }

    public void SetIfTargetIsDead()
    {
        if (target == null) return;

        if (targetType == TargetType.AI && (target as AIController).aiConditions.IsDead)
        {
            Debug.Log("AI Target Dead -> NULL");
            target = null;
        }
        else if (targetType == TargetType.PLAYER && (target as PlayerStateController).Conditions.IsDead)
        {
            Debug.Log("Player Target Dead -> NULL");
            target = null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = sightColor;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 1f, sightRange);
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = sightColor;
        Handles.Label(transform.position + Vector3.forward * sightRange, "Walk", style);

        Gizmos.color = runSightColor;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 1f, runSightRange);
        style.normal.textColor = runSightColor;
        Handles.Label(transform.position + Vector3.forward * runSightRange, "Run", style);
    }
#endif

}
