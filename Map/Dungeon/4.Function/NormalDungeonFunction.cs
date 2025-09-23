using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Dungeon Function/Normal Function ", fileName = "NormalFunction")]
public class NormalDungeonFunction : BaseDungeonFunction
{

    public void ExcuteProcess(NormalDungeonTitle title)
    {
        //title.SpawnData.dungeon = title.dungeonCoroutine;
        //title.DungeonMapData.ExcuteTeleportMap();
        //title.DungeonMapData.ExcuteTeleportController(title.ExcuteController, title.DungeonSpawnPosition);
        //GameManager.Instance.Cam.SetTarget(title.ExcuteController.gameObject);
        //title.SpawnData.SettingSpawnPositionList(title.DungeonSpawnPosition);
        //title.SpawnData.StartWave();

        SoundManager.Instance.PlayBGM_CrossFade(title.BaseBGM, 4f);
        title.SpawnData.dungeon = title.dungeonCoroutine;
        title.DungeonMapData.ExcuteTeleportMap();
        title.SpawnData.onExcuteBoss += () => { SoundManager.Instance.PlayBGM_CrossFade(title.BossBGM, 3f); };
        ScenesManager.Instance.OnExcuteAfterLoading = () => title.DungeonMapData.ExcuteTeleportController(title.ExcuteController, title.DungeonSpawnPosition);
        ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.SettingSpawnPositionList(title.DungeonSpawnPosition);
        ScenesManager.Instance.OnExcuteAfterLoading += () => GameManager.Instance.Cam.SetTarget(title.ExcuteController.gameObject);
        ScenesManager.Instance.OnExcuteAfterLoading += () => GameManager.Instance.Cam.ResetRotation();
        ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.StartWave();
        ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.CreateExistBarrier();
        ScenesManager.Instance.OnExcuteAfterLoading += () => CommonUIManager.Instance.ExcuteGlobalNotifer(title.InitGlobalNotifier);

        title.SpawnData.onCompleteDungeon += () => QuestManager.Instance.ReceiveReport(QuestCategoryDefines.COMPLETE_DUNGEON, title.TaskTarget, 1);
        GameManager.Instance.Player.playerStats.OnDead_ += () => title?.SpawnData?.ExcuteFailProcess();


        // 1. ���� entry�� controller ��������. newController�� �����ϰ� category�� excuteController�� �޾ƿ�.
        // 2. mapData �����صα�. �̰� -> ������ �ش� ������ �̵����ֱ�. 
        // 3. controller ��ġ �̵�.
        // spawnData ����. 1. tRigger, 2. Immediate 
        // spawnData���� �����Ҷ� ���� onDead�� �ش� info�� state �����ϰ��ϱ�?.



    }

}


//��..  �����غ��°�.

//Ÿ��Ʋ - �⺻
//ī�װ� - �⺻���� �������, ���ο� �� �������, �� ���� ��������� ������.
//�� ������ - ������ �� ������, �� ��ġ. 
//���������� - �ʿ� ������ ��ġ, ���� ���� ��.
//��� - (�⺻) -> normal spawn data ����. ( trigger, �ݺ�����)  


//Ʈ����
//Ʈ���� �˻� ����. 
//�÷��̾� ����� -> 1���̺��� 1���� ����.
//�����Ҷ� ������� state���� �ٲٱ� ���.
//�����Ҷ� ������� spawnData�� �Լ� ���. ������ ���� ���� �� �׾����� �˻�.�� �׾��� ��� ���� ���� ���μ���.

//���
//�ѹ��� �� ����. 
//Ʈ���� �˻� x .
//���� ��� ���� ���µ� �˻�.