using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private PlayerStateController player_prefab = null;
    [SerializeField] private PlayerStateController player = null;
    [SerializeField] private ThirdPersonCamera cam = null;
    [SerializeField] private MainCamPPController postController = null;
    [SerializeField] private RewardReputation standardReputation = null;
    [SerializeField] private RewardSkillPoint standardSkillpoint = null;
    [SerializeField] private RewardStatPoint standardStatpoint = null;
    [SerializeField] private RewardExp standardExp = null;

    [SerializeField] private int ownMoney = 0;
    [SerializeField] private int reputation = 0;
    public Image playerIcon = null;
    public bool canUseCamera = true;
    public bool canUsePlayerState = true;
    public bool ignoreItemEquipCondition = false;
    public bool ignoreSkillCondition = false;

    public bool isWriting = false;
    public string title = string.Empty;


    //���� �Ϸ��Ҷ� ǥ���� ����. �ش� �������� �������� ����?.
    public int bossDamage = 0; // ���� �Ϸ��Ҷ� �������� �� ������.
    public int getDamaged = 0; // ���� �Ϸ��Ҷ� �÷��̾ ���� ������.
    public int kill = 0;       // ���� �Ϸ��Ҷ� ���� enemy �� 

    public int Reputation { get => reputation; }
    public RewardExp StandardExp => standardExp;
    public RewardSkillPoint StandardSkillpoint => standardSkillpoint;
    public RewardStatPoint StandardStatpoint => standardStatpoint;

    public int OwnMoney { get => ownMoney; }
    public PlayerStateController Player => GetPlayerController();
    public PlayerStateController Player_Prefab => player_prefab;
    public MainCamPPController MainPP => postController;


    public PlayerStateController GetPlayerController()
    {
        if (player == null)
            player = FindObjectOfType<PlayerStateController>();
        return player;
    }

    public ThirdPersonCamera Cam => cam;

    #region Events
    public delegate void OnUpdate();
    public delegate void OnUpdatePlayerStats(PlayerStatus status);
    public delegate void OnUpdateSkill(PlayerStateController controller);

    public event OnUpdate onOwnMoneyUpdate;
    public event OnUpdatePlayerStats onPlayerStatsUpdate;
    public event OnUpdateSkill onUpdateSkillInfo;

    #endregion

    protected override void Awake()
    {
        base.Awake();
       Application.targetFrameRate = 60; // 120 FPS ����
       QualitySettings.vSyncCount = 0;    // VSync ����

        Screen.SetResolution(2560, 1440, true);
        if (player == null && player_prefab != null)
            SetPlayer(Instantiate(Player_Prefab));
        player?.gameObject?.SetActive(false);

        if (cam == null)
            cam = FindObjectOfType<ThirdPersonCamera>();
        if (postController == null)
            postController = cam.GetComponent<MainCamPPController>();
    
    }

    private void Start()
    {
        onOwnMoneyUpdate?.Invoke();
    }

    public void SetPlayer(PlayerStateController controller)
    {
        Debug.Log("Pl : " + controller);
        player = controller;
    }

    public void LoadReputation(int value)
    {
        reputation = value;
    }
    public void LoadOwnMoney(int value)
    {
        ownMoney= value;
    }
    public void AddReputation(RewardReputation reward)
    {
        AddReputation(reward.ReputationValue, reward);
    }
    public void AddReputation(int value, RewardReputation reward = null)
    {
        reputation += value;
        if (reward != null && value > 0)
            CommonUIManager.Instance.ExcuteItemGainNotifier(reward);
        else if(reward == null && value > 0)
        {
            RewardReputation tempReward = new RewardReputation();
            tempReward.Icon = standardReputation.Icon;
            tempReward.ReputationValue = value;
            tempReward.RewardName = $"��ġ +{value}";
            tempReward.Description = $"��ġ {value} ȹ��"; 
        }

        onPlayerStatsUpdate?.Invoke(player.playerStats);
        UpdateSkillInfo();
    }


    public void SetMinusOwnMoney(int inputMoney)
    {
        ownMoney -= inputMoney;
        if (ownMoney <= 0)
            ownMoney = 0;
        onOwnMoneyUpdate?.Invoke();
        UpdateSkillInfo();
    }

    public void SetPlusOwnMoney(int inputMoney, RewardMoney rewardMoney = null)
    {
        if (inputMoney <= 0) return;
        ownMoney += inputMoney;
        if (rewardMoney != null)
        {
            Debug.Log("Item Not Null");
            CommonUIManager.Instance.ExcuteItemGainNotifier(rewardMoney);
        }
        else
        {
            Debug.Log("Item Value");
            CommonUIManager.Instance.ExcuteItemGainNotifier(inputMoney);
        }

        onOwnMoneyUpdate?.Invoke();
        UpdateSkillInfo();
    }



    public void UpdateSkillInfo() => onUpdateSkillInfo?.Invoke(Player);


}

