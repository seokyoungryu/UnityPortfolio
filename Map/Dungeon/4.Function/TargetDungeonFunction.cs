using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Dungeon Function/Target Function ", fileName = "TargetFunction_")]
public class TargetDungeonFunction : BaseDungeonFunction
{
    private TargetSpawnData spawnData = null;

    private List<TargetDungeonEnemyInfo> targets = new List<TargetDungeonEnemyInfo>();
    //TargetEnemyInfos list ����ϱ�.  �̰Ŵ� trigger, round, �ѹ��� �� ���� �޶�.
    // trigger1���� ���, ���� ���۽� targets�� �ش� ������ Ÿ�ٵ� ����. �ش� Ÿ�ٵ��� �׾�ߵ�.
    // ���� ���� ���۽� targets �ʱ�ȭ�ϰ� , �ش� ���� Ÿ�� ����ϴ� ���.

    // 2�� ���� ��쵵 �ش� ���̺�(����1����)�� Ÿ�ٵ� �����ϰ� ������ ���� ���̺� �̷���.
    // �ѹ��� ���� ���� ��ü �����ϴϱ� -> allTargets �� ����ϱ�.

    public void SettingSpawnData(TargetSpawnData spawnData)
    {
        this.spawnData = spawnData;
    }

    public void ExcuteProcess(TargetDungeonTitle title)
    {
        SoundManager.Instance.PlayBGM_CrossFade(title.BaseBGM,4f);
        title.SpawnData.onCompleteDungeon += () => QuestManager.Instance.ReceiveReport(QuestCategoryDefines.COMPLETE_DUNGEON, title.TaskTarget, 1);
        title.SpawnData.dungeon = title.dungeonCoroutine;
        title.SpawnData.onExcuteBoss += () => { SoundManager.Instance.PlayBGM_CrossFade(title.BossBGM, 3f); };

        title.DungeonMapData.ExcuteTeleportMap();
        ScenesManager.Instance.OnExcuteAfterLoading = () => title.DungeonMapData.ExcuteTeleportController(title.ExcuteController, title.DungeonSpawnPosition);
        ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.SettingSpawnPositionList(title.DungeonSpawnPosition);
        ScenesManager.Instance.OnExcuteAfterLoading += () => GameManager.Instance.Cam.SetTarget(title.ExcuteController.gameObject);
        ScenesManager.Instance.OnExcuteAfterLoading += () => GameManager.Instance.Cam.ResetRotation();
        ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.CreateExistBarrier();
        ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.StartWave();
        ScenesManager.Instance.OnExcuteAfterLoading += () => CommonUIManager.Instance.ExcuteGlobalNotifer(title.InitGlobalNotifier);

        GameManager.Instance.Player.playerStats.OnDead_ += () => title?.SpawnData?.ExcuteFailProcess();

        // 1. mapData �����صα�. 
        // 2. ���� entry�� controller ��������. newController�� �����ϰ� category�� excuteController�� �޾ƿ�.
        // 3. controller ��ġ �̵�.
        // spawnData ����. 1. tRigger, 2. Immediate 
        // spawnData���� �����Ҷ� ���� onDead�� �ش� info�� state �����ϰ��ϱ�?.

        //Ÿ���� trigger�� ���, �ش� ���̺��� Ÿ���� ���� ��� ���� ���� ����?
        //���ϱ�. ���̺�1 -> �� ���� ����.
        //Trigger�� ��� 
        // 1. �� ���帶�� Ÿ���� ��ƾ��� ���� ����� �̵�. ( Ÿ�� �ƴѰ��� ����ִٸ� ���� ), ��� ���� Ÿ�� ������, 
        //     ���� ���̺�� �̵�. 
        // 2. ���尡 1����ٲ�����, ���̺�1�� trigger �Ǹ�  �ش� Ÿ�ٸ� ���̸� ���� ���̺�2�� �̵� �����ϰ�. 
        //�ٵ� 1,2�� �ߺ��ƴ�? �ᱹ�� ���忡�� Ÿ���� ��ƾ��� ���� ���̺�� ���°��ݾ�.

        //Ÿ���� immediate�� ���
        // �׳� ��� ���̺� enemy ���� ����.
    }





}
