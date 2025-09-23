using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Dungeon Database/Dungeon Entry Database ", fileName = "DungeonEntryDB")]
public class DungeonEntryDatabase : ScriptableObject
{
    [Header("����� ĸ�� ���")]
    [SerializeField] private List<DungeonTitleDatabase> chapters;

    [Header("DungeonUI Open�� ������ ĸ��")]
    public StringTaskTarget currentChapterTaskTarget = null;

    public StringTaskTarget CurrentChapterTarget { get { return currentChapterTaskTarget; } set { currentChapterTaskTarget = value; } }
    public List<DungeonTitleDatabase> Chapters => chapters;
}
