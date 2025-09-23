using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemEditor : EditorWindow
{
    Vector2 scrollPosition1 = Vector2.zero;
    Vector2 scrollPosition2 = Vector2.zero;
    public static int insertIndex = 0;
    public static int selection = 0;
    public static int realdSelection = 0;
    public static ItemEditorCategoryType clickCategoryType = ItemEditorCategoryType.ALL;
    public int lastIndex = 0;
    public bool canRemove = false;
    private bool foldoutUseableObject = true;

    public static int moveIndexUp, moveIndexDown ,targetArrayLength , curRealIndex , insertID = 0;

    private static ItemData itemData;
    static ItemEditor itemwindow;
    CategoryEditorWindow categoriWindwow;

    private WeaponItemClip weaponClip = null;
    private ArmorItemClip armorClip = null;
    private AccessoryItemClip accessoryClip = null;
    private TitleItemClip titleClip = null;
    private PosionItemClip posionClip = null;
    private EnchantItemClip enchantClip = null;
    private CraftItemClip craftClip = null;
    private ExtraItemClip extraClip = null;
    private QuestItemClip questClip = null;

    [Header("Test Layout Color")]
    TestColor testColor;
    public static GUIStyle boxRed;
    public static GUIStyle boxWhite;
    public static GUIStyle boxBlue;
    public static GUIStyle boxYellow;
    public static GUIStyle boxGreen;
    public static GUIStyle boxPurple;
    public static GUIStyle boxBrawn;


    [MenuItem("Tools/ItemData Tool")]
    static void Initialization()
    {
        selection = 0;
        insertIndex = 0;
        moveIndexUp = 0;
        moveIndexDown = 0;
        targetArrayLength = 0;
        curRealIndex = 0;
        insertID = 0;

        itemData = ScriptableObject.CreateInstance<ItemData>();
        itemData.LoadData();

        itemwindow = GetWindow<ItemEditor>(false, "ItemData Tool");
        itemwindow.Show();
    }

    private void OnGUI()
    {
        if (itemData == null) return;
        #region color
       // if (testColor == null)
       //     testColor = FindObjectOfType<TestColor>();
       //
       // boxRed = new GUIStyle(EditorStyles.helpBox);
       // boxWhite = new GUIStyle(EditorStyles.helpBox);
       // boxBlue = new GUIStyle(EditorStyles.helpBox);
       // boxYellow = new GUIStyle(EditorStyles.helpBox);
       // boxGreen = new GUIStyle(EditorStyles.helpBox);
       // boxPurple = new GUIStyle(EditorStyles.helpBox);
       // boxBrawn = new GUIStyle(EditorStyles.helpBox);
       //
       // boxRed.normal.background = EditorHelper.MakeTextureColor(1, 1, testColor.colors[0]);
       // boxWhite.normal.background = EditorHelper.MakeTextureColor(1, 1, testColor.colors[1]);
       // boxBlue.normal.background = EditorHelper.MakeTextureColor(1, 1, testColor.colors[2]);
       // boxYellow.normal.background = EditorHelper.MakeTextureColor(1, 1, testColor.colors[3]);
       // boxGreen.normal.background = EditorHelper.MakeTextureColor(1, 1, testColor.colors[4]);
       // boxPurple.normal.background = EditorHelper.MakeTextureColor(1, 1, testColor.colors[5]);
       // boxBrawn.normal.background = EditorHelper.MakeTextureColor(1, 1, testColor.colors[6]);
        #endregion

        if (Event.current.type == EventType.MouseDown)
        {
            GUI.FocusControl(null);
        }

        Debug.Log("Categort : " + clickCategoryType);
        Debug.Log($"selection {selection}  , realIdIndex {realdSelection}");

        EditorGUILayout.BeginVertical("helpbox");
        {
            EditorGUILayout.BeginHorizontal("helpbox");  //ī�װ� container Begin
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("ī�װ�", GUILayout.Width(300)))
                {
                    categoriWindwow = GetWindow<CategoryEditorWindow>("ī�װ� ����â");
                    categoriWindwow.minSize = new Vector2(800, 250);
                    categoriWindwow.maxSize = new Vector2(800, 250);
                    categoriWindwow.position = new Rect(itemwindow.position.x + 550, itemwindow.position.y + 50, itemwindow.position.width, itemwindow.position.height);
                    categoriWindwow.Show();
                }
            }
            EditorGUILayout.EndHorizontal();      //ī�װ� container End

            EditorGUILayout.BeginHorizontal("helpbox");
            EditorGUILayout.Space(0.3f);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("helpbox");  //�� ������ �� �� container begin
            {
                GUIInsert();
                GUILayout.FlexibleSpace();

                EditorGUILayout.LabelField($"�� ������ ������ �� : {itemData.GetDataCount(itemData.allItemClips)} ��", EditorStyles.boldLabel, GUILayout.Width(320));
                if (clickCategoryType != ItemEditorCategoryType.ALL)
                    EditorGUILayout.LabelField($"{clickCategoryType} ������ ������ �� : {itemData.GetClipArray(clickCategoryType).Length.ToString()} ��", EditorStyles.boldLabel);

            }
            EditorGUILayout.EndHorizontal();       //�� ������ �� �� container End

            EditorGUILayout.BeginHorizontal("helpbox");
            EditorGUILayout.Space(0.3f);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("helpbox"); //ž ��ư < , > , Add , Copy , Remove  Container Begin
            {
                if (GUILayout.Button("Add", GUILayout.Width(150)))
                {
                    switch (clickCategoryType)
                    {
                        case ItemEditorCategoryType.ALL:
                            itemData.AddData("BaseDataItem", (int)ItemSortType.WEAPON);
                            itemData.UpdateSortClips();
                            if (itemData.GetDataCount(itemData.allItemClips) > 0)
                                selection = itemData.GetDataCount(itemData.allItemClips) - 1;

                            break;
                        case ItemEditorCategoryType.EQUIPMENT:
                            itemData.AddData("EquipmnetItemData", (int)ItemSortType.WEAPON);
                            itemData.UpdateSortClips();
                            if (itemData.GetDataCount(itemData.equipmentItemClips) > 0)
                                selection = itemData.GetDataCount(itemData.equipmentItemClips) - 1;

                            break;
                        case ItemEditorCategoryType.WEAPON:
                            itemData.AddData("WeaponItemData", (int)ItemSortType.WEAPON);
                            itemData.UpdateSortClips();
                            if (itemData.GetDataCount(itemData.weaponItemClips) > 0)
                                selection = itemData.GetDataCount(itemData.weaponItemClips) - 1;

                            break;
                        case ItemEditorCategoryType.ARMOR:
                            itemData.AddData("ArmorItemData", (int)ItemSortType.ARMOR);
                            itemData.UpdateSortClips();
                            if (itemData.GetDataCount(itemData.armorItemClips) > 0)
                                selection = itemData.GetDataCount(itemData.armorItemClips) - 1;

                            break;
                        case ItemEditorCategoryType.ACCESSORY:
                            itemData.AddData("AccessoryItemData", (int)ItemSortType.ACCESSORY);
                            itemData.UpdateSortClips();
                            if (itemData.GetDataCount(itemData.accessoryItemClips) > 0)
                                selection = itemData.GetDataCount(itemData.accessoryItemClips) - 1;

                            break;
                        case ItemEditorCategoryType.TITLE:
                            itemData.AddData("TitleItemData", (int)ItemSortType.TITLE);
                            itemData.UpdateSortClips();
                            if (itemData.GetDataCount(itemData.titleItemClips) > 0)
                                selection = itemData.GetDataCount(itemData.titleItemClips) - 1;

                            break;
                        case ItemEditorCategoryType.POSION:
                            itemData.AddData("PosionItemData", (int)ItemSortType.POSION);
                            itemData.UpdateSortClips();
                            if (itemData.GetDataCount(itemData.posionItemClips) > 0)
                                selection = itemData.GetDataCount(itemData.posionItemClips) - 1;

                            break;
                        case ItemEditorCategoryType.ENCHANT:
                            itemData.AddData("EnchantItemData", (int)ItemSortType.ENCHANT);
                            itemData.UpdateSortClips();
                            if (itemData.GetDataCount(itemData.enchantItemClips) > 0)
                                selection = itemData.GetDataCount(itemData.enchantItemClips) - 1;

                            break;
                        case ItemEditorCategoryType.CRAFT:
                            itemData.AddData("CraftItemData", (int)ItemSortType.CRAFT);
                            itemData.UpdateSortClips();
                            if (itemData.GetDataCount(itemData.craftItemClips) > 0)
                                selection = itemData.GetDataCount(itemData.craftItemClips) - 1;
                            break;
                        case ItemEditorCategoryType.EXTRA:
                            itemData.AddData("ExtraItemData", (int)ItemSortType.EXTRA);
                            itemData.UpdateSortClips();
                            if (itemData.GetDataCount(itemData.extraItemClips) > 0)
                                selection = itemData.GetDataCount(itemData.extraItemClips) - 1;
                            break;
                        case ItemEditorCategoryType.QUEST:
                            itemData.AddData("QuestItemData", (int)ItemSortType.QUEST);
                            itemData.UpdateSortClips();
                            if (itemData.GetDataCount(itemData.questItemClips) > 0)
                                selection = itemData.GetDataCount(itemData.questItemClips) - 1;

                            break;
                        case ItemEditorCategoryType.CONSUMABLE:
                            itemData.AddData("ConsumableItemData", (int)ItemSortType.POSION);
                            itemData.UpdateSortClips();
                            if (itemData.GetDataCount(itemData.consumableItemClips) > 0)
                                selection = itemData.GetDataCount(itemData.consumableItemClips) - 1;

                            break;
                        case ItemEditorCategoryType.MATERIAL:
                            itemData.AddData("MaterialItemData", (int)ItemSortType.EXTRA);
                            itemData.UpdateSortClips();
                            if (itemData.GetDataCount(itemData.materialItemClips) > 0)
                                selection = itemData.GetDataCount(itemData.materialItemClips) - 1;

                            break;

                    }
                    itemData.UpdateSortClips();

                }

                if (itemData.GetClipArray(clickCategoryType).Length > 0)
                {
                    if (GUILayout.Button("Copy", GUILayout.Width(150)))
                    {
                        itemData.Copy(realdSelection);
                        itemData.UpdateSortClips();
                        if (itemData.GetClipArray(clickCategoryType).Length != lastIndex)
                            selection = itemData.GetClipArray(clickCategoryType).Length - 1;
                    }
                }

                if (itemData.GetClipArray(clickCategoryType).Length > 0)
                {
                    if (GUILayout.Button("Remove", GUILayout.Width(150)))
                    {
                        itemData.RemoveData(realdSelection);
                        itemData.UpdateSortClips();
                        if (itemData.GetClipArray(clickCategoryType).Length != lastIndex)
                            selection = itemData.GetClipArray(clickCategoryType).Length - 1;
                    }
                }
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("<", GUILayout.Width(150)))
                {
                    if (selection > 0)
                        selection -= 1;
                }
                if (GUILayout.Button(">", GUILayout.Width(150)))
                {
                    if (selection + 1 < itemData.GetClipArray(clickCategoryType).Length)
                        selection += 1;
                }
            }
            EditorGUILayout.EndHorizontal();   //ž ��ư < , > , Add , Copy , Remove  Container End


            EditorGUILayout.BeginHorizontal("helpbox");     // Scroll  Container Begin
            {
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        //list scrollview
                        scrollPosition1 = EditorGUILayout.BeginScrollView(scrollPosition1, "helpbox", GUILayout.Width(250));
                        {
                            EditorGUILayout.BeginVertical();   // ��� ����Ʈ container Begin
                            {
                                int lastSelection = 0;
                                MakeSelectionGridDataList(ref lastSelection, clickCategoryType);
                            }
                            EditorGUILayout.EndVertical();       // ��� ����Ʈ container End
                        }
                        EditorGUILayout.EndScrollView();

                        //data info scrollview
                        scrollPosition2 = EditorGUILayout.BeginScrollView(scrollPosition2, "helpbox");
                        {
                            EditorGUILayout.BeginVertical("box");
                            {
                                if (itemData.GetClipArray(clickCategoryType).Length > 0)
                                {
                                    EditorGUILayout.LabelField("realIdSecltion", realdSelection.ToString());
                                    EditorGUILayout.LabelField("AllClipIndex", itemData.FindIDToIndex(itemData.allItemClips[realdSelection].id).ToString());
                                    EditorGUILayout.LabelField("Selection", selection.ToString());
                                    EditorGUILayout.LabelField("ID", itemData.allItemClips[realdSelection].id.ToString());
                                    itemData.allItemClips[realdSelection].itemName = EditorGUILayout.TextField("Enum �̸�", itemData.allItemClips[realdSelection].itemName);
                                    itemData.allItemClips[realdSelection].itemCategoryType = (ItemCategoryType)EditorGUILayout.EnumPopup("������ Ÿ��", itemData.allItemClips[realdSelection].itemCategoryType);

                                    if (itemData.allItemClips[realdSelection].itemCategoryType == ItemCategoryType.EQUIPMENT)
                                        itemData.allItemClips[realdSelection].equipmentTpye = (EquipmentTpye)EditorGUILayout.EnumPopup("���� Ÿ��", itemData.allItemClips[realdSelection].equipmentTpye);
                                    else if (itemData.allItemClips[realdSelection].itemCategoryType == ItemCategoryType.CONSUMABLE)
                                        itemData.allItemClips[realdSelection].consumableType = (ConsumableType)EditorGUILayout.EnumPopup("�Һ�ǰ Ÿ��", itemData.allItemClips[realdSelection].consumableType);
                                    else if (itemData.allItemClips[realdSelection].itemCategoryType == ItemCategoryType.MATERIAL)
                                        itemData.allItemClips[realdSelection].materialType = (MaterialType)EditorGUILayout.EnumPopup("��� Ÿ��", itemData.allItemClips[realdSelection].materialType);
                                    else if (itemData.allItemClips[realdSelection].itemCategoryType == ItemCategoryType.QUESTITEM)
                                        itemData.allItemClips[realdSelection].questIType = (QuestIType)EditorGUILayout.EnumPopup("����Ʈ Ÿ��", itemData.allItemClips[realdSelection].questIType);

                                    EditorGUILayout.BeginHorizontal("helpbox"); //sprite setting
                                    {
                                        EditorGUILayout.LabelField("Item Icon", EditorStyles.boldLabel);

                                        if (itemData.allItemClips[realdSelection].itemPath != string.Empty || itemData.allItemClips[realdSelection].texturePath != string.Empty)
                                            itemData.allItemClips[realdSelection].ReloadResource();

                                        GUILayout.FlexibleSpace();
                                        itemData.allItemClips[realdSelection].itemTexture = (Sprite)EditorGUILayout.ObjectField(itemData.allItemClips[realdSelection].itemTexture, typeof(Sprite), false, GUILayout.Width(80), GUILayout.Height(80));
                                        if (itemData.allItemClips[realdSelection].itemTexture != null)
                                        {
                                            string spriteName = itemData.allItemClips[realdSelection].itemTexture.name;
                                            spriteName = spriteName.Replace(" ", "");
                                            itemData.allItemClips[realdSelection].textureName = spriteName;
                                            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(itemData.allItemClips[realdSelection].itemTexture), spriteName);
                                            itemData.allItemClips[realdSelection].texturePath = EditorHelper.GetObjectPath(itemData.allItemClips[realdSelection].itemTexture);
                                           // Debug.Log(itemData.allItemClips[realdSelection].texturePath);
                                        }
                                        else
                                        {
                                            itemData.allItemClips[realdSelection].itemTexture = null;
                                            itemData.allItemClips[realdSelection].texturePath = string.Empty;
                                            itemData.allItemClips[realdSelection].textureName = string.Empty;
                                        }
                                    }
                                    EditorGUILayout.EndHorizontal();

                                    itemData.allItemClips[realdSelection].isUnbreakable = EditorGUILayout.Toggle("�ı��Ұ�", itemData.allItemClips[realdSelection].isUnbreakable);
                                    itemData.allItemClips[realdSelection].isOverlap = EditorGUILayout.Toggle("Is Overlap", itemData.allItemClips[realdSelection].isOverlap);
                                    if (itemData.allItemClips[realdSelection].isOverlap)
                                    {
                                        EditorGUILayout.BeginHorizontal("helpbox");
                                        itemData.allItemClips[realdSelection].maxOverlapCount = EditorGUILayout.IntField("Max Overlap Count", itemData.allItemClips[realdSelection].maxOverlapCount);
                                        EditorGUILayout.EndHorizontal();
                                    }

                                    itemData.allItemClips[realdSelection].havePrefab = EditorGUILayout.Toggle("Have Prefab", itemData.allItemClips[realdSelection].havePrefab);
                                    if (itemData.allItemClips[realdSelection].havePrefab)
                                    {
                                        EditorGUILayout.BeginHorizontal("helpbox");
                                        itemData.allItemClips[realdSelection].itemPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab", itemData.allItemClips[realdSelection].itemPrefab, typeof(GameObject), false);
                                        EditorGUILayout.EndHorizontal();
                                        if (itemData.allItemClips[realdSelection].itemPrefab != null)
                                        {
                                            string itemName = itemData.allItemClips[realdSelection].itemPrefab.name;
                                            itemName = itemName.Replace(" ", "");
                                            itemData.allItemClips[realdSelection].itemPath = EditorHelper.GetObjectPath(itemData.allItemClips[realdSelection].itemPrefab);
                                            itemData.allItemClips[realdSelection].itemName = itemName;
                                            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(itemData.allItemClips[realdSelection].itemPrefab), itemName);
                                        }
                                        else
                                        {
                                            itemData.allItemClips[realdSelection].itemPrefab = null;
                                            itemData.allItemClips[realdSelection].itemPath = string.Empty;
                                        }
                                    }   //������ �κ�
                                    else
                                    {
                                        itemData.allItemClips[realdSelection].itemPrefab = null;
                                        itemData.allItemClips[realdSelection].itemPath = string.Empty;
                                    }

                                    EditorHelper.GetSpcae(1);

                                    EditorGUILayout.BeginHorizontal("helpbox");
                                    {
                                        EditorGUILayout.LabelField("����", GUILayout.Width(40));
                                        itemData.allItemClips[realdSelection].description = EditorGUILayout.TextArea(itemData.allItemClips[realdSelection].description, GUILayout.Height(40));
                                    }
                                    EditorGUILayout.EndHorizontal();


                                    itemData.allItemClips[realdSelection].SettingSortType();
                                    SetSortTypeAndClassType();

                                    //Ÿ�� Ȯ��.
                                    //EditorGUILayout.LabelField("Sort Type ", itemData.allItemClips[realdSelection].sortType.ToString());
                                    //EditorGUILayout.LabelField("Class Type ", itemData.allItemClips[realdSelection].GetType().ToString());

                                    EditorGUILayout.BeginHorizontal("box");   //Item Cool Time 
                                    {
                                        GUIContent content = new GUIContent("Item Cool Time", "������ ���� ��� �ð��� �Է��ϼ���.");
                                        EditorGUILayout.LabelField(content);
                                        itemData.allItemClips[realdSelection].itemCoolTime = EditorGUILayout.FloatField(itemData.allItemClips[realdSelection].itemCoolTime);
                                        GUILayout.FlexibleSpace();
                                    }
                                    EditorGUILayout.EndHorizontal();

                                    EditorGUILayout.BeginHorizontal("box");  // Useable Object Count
                                    {
                                        EditorGUILayout.LabelField("UseableObject ����");
                                        itemData.allItemClips[realdSelection].useableObjectCount = EditorGUILayout.IntField(itemData.allItemClips[realdSelection].useableObjectCount);
                                        GUILayout.FlexibleSpace();
                                    }
                                    EditorGUILayout.EndHorizontal();

                                    EditorGUILayout.BeginVertical("helpbox");  //   Useable Object Container 
                                    {
                                        if (itemData.allItemClips[realdSelection].useableObjectCount > 0)
                                        {
                                            foldoutUseableObject = EditorGUILayout.Foldout(foldoutUseableObject, "Useable Objects");
                                            if (foldoutUseableObject == true)
                                            {
                                                if (itemData.allItemClips[realdSelection].useableObject == null)
                                                    itemData.allItemClips[realdSelection].useableObject = new UseableObject[itemData.allItemClips[realdSelection].useableObjectCount];
                                                else if (itemData.allItemClips[realdSelection].useableObjectCount != itemData.allItemClips[realdSelection].useableObject.Length)
                                                    itemData.allItemClips[realdSelection].useableObject = new UseableObject[itemData.allItemClips[realdSelection].useableObjectCount];

                                                for (int i = 0; i < itemData.allItemClips[realdSelection].useableObjectCount; i++)
                                                {
                                                    EditorGUILayout.BeginHorizontal();
                                                    itemData.allItemClips[realdSelection].useableObject[i] = (UseableObject)EditorGUILayout.ObjectField("UseableObject ����"
                                                        , itemData.allItemClips[realdSelection].useableObject[i], typeof(UseableObject), false);
                                                    EditorGUILayout.EndHorizontal();
                                                }
                                                for (int i = 0; i < itemData.allItemClips[realdSelection].useableObject.Length; i++)
                                                {
                                                    if (itemData.allItemClips[realdSelection].useableObject[i] == null) continue;
                                                    for (int x = 0; x < itemData.allItemClips[realdSelection].useableObject.Length; x++)
                                                    {
                                                        if (itemData.allItemClips[realdSelection].useableObject[x] != null
                                                            && i != x
                                                            && itemData.allItemClips[realdSelection].useableObject[x].GetType() != itemData.allItemClips[realdSelection].useableObject[i].GetType()
                                                            && itemData.allItemClips[realdSelection].useableObject[x].ID == itemData.allItemClips[realdSelection].useableObject[i].ID)
                                                            itemData.allItemClips[realdSelection].useableObject[x] = null;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    EditorGUILayout.EndVertical();

                                    EditorHelper.GetSpcae(2);

                                    switch (itemData.allItemClips[realdSelection].sortType)
                                    {
                                        case ItemSortType.WEAPON:
                                            WeaponGUI();
                                            break;
                                        case ItemSortType.ARMOR:
                                            ArmorGUI();
                                            break;
                                        case ItemSortType.ACCESSORY:
                                            AccessoryGUI();
                                            break;
                                        case ItemSortType.TITLE:
                                            TitleGUI();
                                            break;
                                        case ItemSortType.POSION:
                                            PosionGUI();
                                            break;
                                        case ItemSortType.ENCHANT:
                                            EnchantGUI();
                                            break;
                                        case ItemSortType.CRAFT:
                                            CraftGUI();
                                            break;
                                        case ItemSortType.EXTRA:
                                            ExtraGUI();
                                            break;
                                        case ItemSortType.QUEST:
                                            QuestGUI();
                                            break;
                                    }  //�� Ÿ�Ժ� GUI ���� 

                                }
                            }
                            EditorGUILayout.EndVertical();
                        }
                        EditorGUILayout.EndScrollView();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Reload", GUILayout.Height(35)) )
                {
                    itemData.LoadData();
                    selection = 0;
                }
                if(GUILayout.Button("Save", GUILayout.Height(35)))
                {
                    itemData.SaveData();
                    itemData.CreateEnumList();

                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                }

            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

    }

    private void OnDestroy()
    {
        if (categoriWindwow != null)
            categoriWindwow.Close();
    }

    private void OnFocus()
    {
        if (categoriWindwow != null)
            categoriWindwow.Close();
    }

    /// <summary>
    /// sortType�� �������� ���� Ŭ���� realIndex�� Ŭ���� Ÿ���� ����.
    /// </summary>
    public void SetSortTypeAndClassType()    // �ش� ������ �ּҸ� �־����� �ش� ���� �����ϸ� �ּ��� ������ ������.
    {                                        // �׸��� selectGrid���� lastselect�� select�� ���ؼ� �ٸ��� �ϴ� ���� null�� ���Ѱ� �ؿ��� �ٽ� ����
        if (itemData.GetDataCount(itemData.allItemClips) <= 0)
            return;

        itemData.allItemClips[realdSelection].SettingSortType();
        if (itemData.allItemClips[realdSelection].sortType == ItemSortType.WEAPON)
        {
            if (itemData.allItemClips[realdSelection] != weaponClip)
            {
                if (itemData.allItemClips[realdSelection] is WeaponItemClip)
                    weaponClip = itemData.allItemClips[realdSelection] as WeaponItemClip;
                else
                {
                    weaponClip = new WeaponItemClip(itemData.allItemClips[realdSelection]);
                    itemData.allItemClips[realdSelection] = weaponClip;
                }
            }
            SetNullClips(itemData.allItemClips[realdSelection].sortType);
        }
        else if (itemData.allItemClips[realdSelection].sortType == ItemSortType.ARMOR)
        {
            if (itemData.allItemClips[realdSelection] != armorClip)
            {
                if (itemData.allItemClips[realdSelection] is ArmorItemClip)
                    armorClip = itemData.allItemClips[realdSelection] as ArmorItemClip;
                else
                {
                    armorClip = new ArmorItemClip(itemData.allItemClips[realdSelection]);
                    itemData.allItemClips[realdSelection] = armorClip;
                }
            }
            SetNullClips(itemData.allItemClips[realdSelection].sortType);

        }
        else if (itemData.allItemClips[realdSelection].sortType == ItemSortType.ACCESSORY)
        {
            if (itemData.allItemClips[realdSelection] != accessoryClip)
            {
                if (itemData.allItemClips[realdSelection] is AccessoryItemClip)
                    accessoryClip = itemData.allItemClips[realdSelection] as AccessoryItemClip;
                else
                {
                    accessoryClip = new AccessoryItemClip(itemData.allItemClips[realdSelection]);
                    itemData.allItemClips[realdSelection] = accessoryClip;
                }
            }
            SetNullClips(itemData.allItemClips[realdSelection].sortType);
        }
        else if (itemData.allItemClips[realdSelection].sortType == ItemSortType.TITLE)
        {
            if (itemData.allItemClips[realdSelection] != titleClip)
            {
                if (itemData.allItemClips[realdSelection] is TitleItemClip)
                    titleClip = itemData.allItemClips[realdSelection] as TitleItemClip;
                else
                {
                    titleClip = new TitleItemClip(itemData.allItemClips[realdSelection]);
                    itemData.allItemClips[realdSelection] = titleClip;
                }
            }
            SetNullClips(itemData.allItemClips[realdSelection].sortType);
        }
        else if (itemData.allItemClips[realdSelection].sortType == ItemSortType.POSION)
        {
            if (itemData.allItemClips[realdSelection] != posionClip)
            {
                if (itemData.allItemClips[realdSelection] is PosionItemClip)
                    posionClip = itemData.allItemClips[realdSelection] as PosionItemClip;
                else
                {
                    posionClip = new PosionItemClip(itemData.allItemClips[realdSelection]);
                    itemData.allItemClips[realdSelection] = posionClip;
                }
            }
            SetNullClips(itemData.allItemClips[realdSelection].sortType);
        }
        else if (itemData.allItemClips[realdSelection].sortType == ItemSortType.ENCHANT)
        {
            if (itemData.allItemClips[realdSelection] != enchantClip)
            {
                if (itemData.allItemClips[realdSelection] is EnchantItemClip)
                    enchantClip = itemData.allItemClips[realdSelection] as EnchantItemClip;
                else
                {
                    enchantClip = new EnchantItemClip(itemData.allItemClips[realdSelection]);
                    itemData.allItemClips[realdSelection] = enchantClip;
                }
            }
            SetNullClips(itemData.allItemClips[realdSelection].sortType);
        }
        else if (itemData.allItemClips[realdSelection].sortType == ItemSortType.CRAFT)
        {
            if (itemData.allItemClips[realdSelection] != craftClip)
            {
                if (itemData.allItemClips[realdSelection] is CraftItemClip)
                    craftClip = itemData.allItemClips[realdSelection] as CraftItemClip;
                else
                {
                    craftClip = new CraftItemClip(itemData.allItemClips[realdSelection]);
                    itemData.allItemClips[realdSelection] = craftClip;
                }
            }
            SetNullClips(itemData.allItemClips[realdSelection].sortType);
        }
        else if (itemData.allItemClips[realdSelection].sortType == ItemSortType.EXTRA)
        {
            if (itemData.allItemClips[realdSelection] != extraClip)
            {
                if (itemData.allItemClips[realdSelection] is ExtraItemClip)
                    extraClip = itemData.allItemClips[realdSelection] as ExtraItemClip;
                else
                {
                    extraClip = new ExtraItemClip(itemData.allItemClips[realdSelection]);
                    itemData.allItemClips[realdSelection] = extraClip;
                }
            }
            SetNullClips(itemData.allItemClips[realdSelection].sortType);
        }
        else if (itemData.allItemClips[realdSelection].sortType == ItemSortType.QUEST)
        {
            if (itemData.allItemClips[realdSelection] != questClip)
            {
                if (itemData.allItemClips[realdSelection] is QuestItemClip)
                    questClip = itemData.allItemClips[realdSelection] as QuestItemClip;
                else
                {
                    questClip = new QuestItemClip(itemData.allItemClips[realdSelection]);
                    itemData.allItemClips[realdSelection] = questClip;
                }
            }
            SetNullClips(itemData.allItemClips[realdSelection].sortType);
        }
    }


    /// <summary>
    ///  sortType ����� ���� sortType�� ������ clips�� null�� ����
    /// </summary>
    private void SetNullClips(ItemSortType type)
    {
        if (type == ItemSortType.WEAPON)
        {
            armorClip = null;
            accessoryClip = null;
            titleClip = null;
            posionClip = null;
            enchantClip = null;
            craftClip = null;
            extraClip = null;
            questClip = null;
        }
        else if (type == ItemSortType.ARMOR)
        {
            weaponClip = null;
            accessoryClip = null;
            titleClip = null;
            posionClip = null;
            enchantClip = null;
            craftClip = null;
            extraClip = null;
            questClip = null;

        }
        else if (type == ItemSortType.ACCESSORY)
        {
            weaponClip = null;
            armorClip = null;
            titleClip = null;
            posionClip = null;
            enchantClip = null;
            craftClip = null;
            extraClip = null;
            questClip = null;
        }
        else if (type == ItemSortType.TITLE)
        {
            weaponClip = null;
            armorClip = null;
            accessoryClip = null;
            posionClip = null;
            enchantClip = null;
            craftClip = null;
            extraClip = null;
            questClip = null;
        }
        else if (type == ItemSortType.POSION)
        {
            weaponClip = null;
            armorClip = null;
            accessoryClip = null;
            titleClip = null;
            enchantClip = null;
            craftClip = null;
            extraClip = null;
            questClip = null;
        }
        else if (type == ItemSortType.ENCHANT)
        {
            weaponClip = null;
            armorClip = null;
            accessoryClip = null;
            titleClip = null;
            posionClip = null;
            craftClip = null;
            extraClip = null;
            questClip = null;
        }
        else if (type == ItemSortType.CRAFT)
        {
            weaponClip = null;
            armorClip = null;
            accessoryClip = null;
            titleClip = null;
            posionClip = null;
            enchantClip = null;
            extraClip = null;
            questClip = null;

        }
        else if (type == ItemSortType.EXTRA)
        {
            weaponClip = null;
            armorClip = null;
            accessoryClip = null;
            titleClip = null;
            posionClip = null;
            enchantClip = null;
            craftClip = null;
            questClip = null;
        }
        else if (type == ItemSortType.QUEST)
        {
            weaponClip = null;
            armorClip = null;
            accessoryClip = null;
            titleClip = null;
            posionClip = null;
            enchantClip = null;
            craftClip = null;
            extraClip = null;
        }
    }



    /// <summary>
    /// selectgrid ��� ����.
    /// </summary>
    private void MakeSelectionGridDataList(ref int lastSelection, ItemEditorCategoryType type)
    {
        lastIndex = itemData.GetClipArray(type).Length;    //���� ī�װ� ���� ����. [ 5] 
        if (itemData.GetClipArray(type).Length > 0)
        {
            lastSelection = selection;
            selection = GUILayout.SelectionGrid(selection, itemData.ReturnTypeNameList(type), 1);
            realdSelection = itemData.FindIDToIndex( itemData.GetClipArray(type)[selection].id);
            if (lastSelection != selection)
            {
                itemData.UpdateSortClips();
                if (lastIndex != itemData.GetClipArray(type).Length)    // ���� ������ �Ǿ ���� ī�װ� ������ �ٸ��Ͽ�. 
                {
                    selection = itemData.GetClipArray(type).Length - 1;  // ���� ī�װ��� �������� select�� �ش�.
                }
            }                                                                // �� ���� �����ӿ� ���� lastindex�� ���� ī�װ� ������ �ٽ� ������Ʈ��.
        }
        else   //������ ���� ���
        {
            string[] tmp = new string[1] { "Empty" };
            lastSelection = selection;
            selection = GUILayout.SelectionGrid(selection, tmp, 1, GUILayout.Width(200));
        } 


    }


    private void GUIInsert()
    {
        if (clickCategoryType == ItemEditorCategoryType.ALL)
            Insert(itemData.allItemClips, itemData.allItemClips.Length, selection);
         else if (clickCategoryType == ItemEditorCategoryType.EQUIPMENT)
            Insert(itemData.equipmentItemClips, itemData.equipmentItemClips.Length, selection);
        else if (clickCategoryType == ItemEditorCategoryType.CONSUMABLE)
            Insert(itemData.consumableItemClips, itemData.consumableItemClips.Length, selection);
        else if (clickCategoryType == ItemEditorCategoryType.MATERIAL)
            Insert(itemData.materialItemClips, itemData.materialItemClips.Length, selection);
        else if (clickCategoryType == ItemEditorCategoryType.QUEST)
            Insert(itemData.questItemClips, itemData.questItemClips.Length, selection);
        else if (clickCategoryType == ItemEditorCategoryType.WEAPON)
            Insert(itemData.weaponItemClips, itemData.weaponItemClips.Length, selection);
        else if (clickCategoryType == ItemEditorCategoryType.ARMOR)
            Insert(itemData.armorItemClips, itemData.armorItemClips.Length, selection);
        else if (clickCategoryType == ItemEditorCategoryType.ACCESSORY)
            Insert(itemData.accessoryItemClips, itemData.accessoryItemClips.Length, selection);
        else if (clickCategoryType == ItemEditorCategoryType.TITLE)
            Insert(itemData.titleItemClips, itemData.titleItemClips.Length, selection);
        else if (clickCategoryType == ItemEditorCategoryType.POSION)
            Insert(itemData.posionItemClips, itemData.posionItemClips.Length, selection);
        else if (clickCategoryType == ItemEditorCategoryType.ENCHANT)
            Insert(itemData.enchantItemClips, itemData.enchantItemClips.Length, selection);
        else if (clickCategoryType == ItemEditorCategoryType.CRAFT)
            Insert(itemData.craftItemClips, itemData.craftItemClips.Length, selection);
        else if (clickCategoryType == ItemEditorCategoryType.EXTRA)
            Insert(itemData.extraItemClips, itemData.extraItemClips.Length, selection);


        EditorHelper.InsertMulti(ref itemData.allItemClips, targetArrayLength, ref selection, curRealIndex, moveIndexUp, moveIndexDown,ref insertIndex, insertID);
        itemData.UpdateSortClips();
    }

    private void Insert(BaseItemClip[] array, int length, int selection)
    {
        EditorHelper.InsertSetting(array, itemData, length, selection, ref curRealIndex, ref moveIndexUp, ref moveIndexDown, ref targetArrayLength, ref insertIndex, ref insertID);
    }

    public static void UpdateSort()
    {
        itemData.UpdateSortClips();
    }


    #region SortType GUI
    private void WeaponGUI()
    {
        if (weaponClip == null || weaponClip != itemData.allItemClips[realdSelection])
            SetSortTypeAndClassType();

        if (weaponClip != null)
        {
            EditorGUILayout.BeginVertical("helpbox");
            {
                GUIStyle statsSettingGUi = new GUIStyle(EditorStyles.boldLabel);
                statsSettingGUi.fontSize = 14;
                EditorGUILayout.LabelField("Stats Settings", statsSettingGUi);
                EditorHelper.GetSpcae(3);

                EditorGUILayout.BeginHorizontal();
                {
                    weaponClip.uiItemName = EditorGUILayout.TextField("���� UI�̸� ", weaponClip.uiItemName);
                }
                EditorGUILayout.EndHorizontal();

                EditorHelper.GetSpcae(1);

                EditorGUILayout.BeginHorizontal();
                {
                    weaponClip.itemLevel = EditorGUILayout.IntField("���� ���� ", weaponClip.itemLevel);
                    GUILayout.FlexibleSpace();
                    weaponClip.requiredLevel = EditorGUILayout.IntField("�ʿ� ���� ����", weaponClip.requiredLevel);
                }
                EditorGUILayout.EndHorizontal();

                EditorHelper.GetSpcae(1);
                EditorGUILayout.BeginVertical("helpbox");
                {
                    //STR
                    EditorGUILayout.BeginHorizontal();
                    {
                        weaponClip.minStr = EditorGUILayout.IntField("Min STR", (int)weaponClip.minStr);
                        GUILayout.FlexibleSpace();
                        weaponClip.maxStr = EditorGUILayout.IntField("Max STR", (int)weaponClip.maxStr);
                    }
                    EditorGUILayout.EndHorizontal();

                    //DEX
                    EditorGUILayout.BeginHorizontal();
                    {
                        weaponClip.minDex = EditorGUILayout.IntField("Min DEX", (int)weaponClip.minDex);
                        GUILayout.FlexibleSpace();
                        weaponClip.maxDex = EditorGUILayout.IntField("Max DEX", (int)weaponClip.maxDex);
                    }
                    EditorGUILayout.EndHorizontal();

                    //LUCK
                    EditorGUILayout.BeginHorizontal();
                    {
                        weaponClip.minLuc = EditorGUILayout.IntField("Min LUC", (int)weaponClip.minLuc);
                        GUILayout.FlexibleSpace();
                        weaponClip.maxLuc = EditorGUILayout.IntField("Max LUC", (int)weaponClip.maxLuc);
                    }
                    EditorGUILayout.EndHorizontal();

                    //INT
                    EditorGUILayout.BeginHorizontal();
                    {
                        weaponClip.minInt = EditorGUILayout.IntField("Min INT", (int)weaponClip.minInt);
                        GUILayout.FlexibleSpace();
                        weaponClip.maxInt = EditorGUILayout.IntField("Max INT", (int)weaponClip.maxInt);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                EditorHelper.GetSpcae(1);
                EditorGUILayout.BeginVertical("helpbox");
                {
                    //���ݷ� 
                    EditorGUILayout.BeginHorizontal();
                    {
                        weaponClip.minAtk = EditorGUILayout.FloatField("�ּ� ���ݷ� ", weaponClip.minAtk);
                        GUILayout.FlexibleSpace();
                        weaponClip.maxAtk = EditorGUILayout.FloatField("�ִ� ���ݷ�", weaponClip.maxAtk);
                    }
                    EditorGUILayout.EndHorizontal();

                    //���� �ӵ� 
                    EditorGUILayout.BeginHorizontal();
                    {
                        weaponClip.minAtkSpeed = EditorGUILayout.FloatField("�ּ� ���ݼӵ�", weaponClip.minAtkSpeed);
                        GUILayout.FlexibleSpace();
                        weaponClip.maxAtkSpeed = EditorGUILayout.FloatField("�ִ� ���ݼӵ�", weaponClip.maxAtkSpeed);
                    }
                    EditorGUILayout.EndHorizontal();

                    //ũ��Ƽ�� Ȯ�� 
                    EditorGUILayout.BeginHorizontal();
                    {
                        weaponClip.minCriChance = EditorGUILayout.FloatField("�ּ� ũ��Ƽ�� Ȯ��", weaponClip.minCriChance);
                        GUILayout.FlexibleSpace();
                        weaponClip.maxCriChance = EditorGUILayout.FloatField("�ִ� ũ��Ƽ�� Ȯ��", weaponClip.maxCriChance);
                    }
                    EditorGUILayout.EndHorizontal(); 

                    //ũ��Ƽ�� ������ 
                    EditorGUILayout.BeginHorizontal();
                    {
                        weaponClip.minCriDmg = EditorGUILayout.FloatField("�ּ� ũ��Ƽ�� ������", weaponClip.minCriDmg);
                        GUILayout.FlexibleSpace();
                        weaponClip.maxCriDmg = EditorGUILayout.FloatField("�ִ� ũ��Ƽ�� ������", weaponClip.maxCriDmg);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();


                EditorHelper.GetSpcae(1);

                EditorGUILayout.BeginHorizontal();
                {
                    weaponClip.enchantLeftCount = EditorGUILayout.IntField("��þƮ ���� Ƚ��", weaponClip.enchantLeftCount);
                    GUILayout.FlexibleSpace();
                    weaponClip.enchantLimitCount = EditorGUILayout.IntField("��þƮ ���� ����", weaponClip.enchantLimitCount);
                }
                EditorGUILayout.EndHorizontal();

                EditorHelper.GetSpcae(1);

                EditorGUILayout.BeginHorizontal();
                {
                    weaponClip.buyCost = EditorGUILayout.IntField("���� ����", weaponClip.buyCost, GUILayout.Width(300));
                    GUILayout.FlexibleSpace();
                    weaponClip.sellCost = EditorGUILayout.IntField("�Ǹ� ����", weaponClip.sellCost, GUILayout.Width(300));
                }
                EditorGUILayout.EndHorizontal();

                weaponClip.repurchaseCost = EditorGUILayout.IntField("�籸�� ����", weaponClip.repurchaseCost, GUILayout.Width(300));

                EditorHelper.GetSpcae(2);

                EditorGUILayout.BeginVertical("helpbox");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        weaponClip.isDefaultPotentialPercent = EditorGUILayout.Toggle("����ɷ� ��� % �ڵ� ����", weaponClip.isDefaultPotentialPercent, GUILayout.Width(160));
                    }
                    EditorGUILayout.EndHorizontal();


                    if (!weaponClip.isDefaultPotentialPercent)
                    {
                        EditorGUILayout.LabelField("�� 100%  [ ���� : " + (weaponClip.potentialPercent[0] + weaponClip.potentialPercent[1]
                            + weaponClip.potentialPercent[2] + weaponClip.potentialPercent[3] + weaponClip.potentialPercent[4]) + " % ]");
                        EditorGUILayout.BeginVertical("helpbox");
                        {
                            weaponClip.potentialPercent[0] = EditorGUILayout.IntField("None ��� ", weaponClip.potentialPercent[0]);
                            weaponClip.potentialPercent[1] = EditorGUILayout.IntField("Normal ��� ", weaponClip.potentialPercent[1]);
                            weaponClip.potentialPercent[2] = EditorGUILayout.IntField("Rare ��� ", weaponClip.potentialPercent[2]);
                            weaponClip.potentialPercent[3] = EditorGUILayout.IntField("Unique ��� ", weaponClip.potentialPercent[3]);
                            weaponClip.potentialPercent[4] = EditorGUILayout.IntField("Legendary ��� ", weaponClip.potentialPercent[4]);

                        }
                        EditorGUILayout.EndVertical();
                    }
                    else
                    {
                        EditorGUILayout.LabelField(" �⺻ �� [  None[30%]  Normal[40%]  Rare[20%]  Unique[6%]  Legendary[4%]  ]", EditorStyles.boldLabel);
                    }
                }
                EditorGUILayout.EndVertical();

            }
            EditorGUILayout.EndVertical();
        }
    }

    private void ArmorGUI()
    {
        if (armorClip == null || armorClip != itemData.allItemClips[realdSelection])
            SetSortTypeAndClassType();

        if (armorClip != null)
        {
            EditorGUILayout.BeginVertical("helpbox");
            {
                GUIStyle statsSettingGUi = new GUIStyle(EditorStyles.boldLabel);
                statsSettingGUi.fontSize = 14;
                EditorGUILayout.LabelField("Stats Settings", statsSettingGUi);
                EditorHelper.GetSpcae(3);


                EditorGUILayout.BeginHorizontal();
                {
                    armorClip.uiItemName = EditorGUILayout.TextField("�� UI �̸� ", armorClip.uiItemName);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    armorClip.armorType = (ArmorType)EditorGUILayout.EnumPopup("Armor Parts Type", armorClip.armorType);

                }
                EditorGUILayout.EndHorizontal();

                EditorHelper.GetSpcae(1);

                EditorGUILayout.BeginHorizontal();
                {
                    armorClip.itemLevel = EditorGUILayout.IntField("���� ���� ", armorClip.itemLevel);
                    GUILayout.FlexibleSpace();
                    armorClip.requiredLevel = EditorGUILayout.IntField("�ʿ� ���� ����", armorClip.requiredLevel);
                }
                EditorGUILayout.EndHorizontal();

                EditorHelper.GetSpcae(1);
                EditorGUILayout.BeginVertical("helpbox");
                {
                    //STR
                    EditorGUILayout.BeginHorizontal();
                    {
                        armorClip.minStr = EditorGUILayout.IntField("Min STR", (int)armorClip.minStr);
                        GUILayout.FlexibleSpace();
                        armorClip.maxStr = EditorGUILayout.IntField("Max STR", (int)armorClip.maxStr);
                    }
                    EditorGUILayout.EndHorizontal();

                    //DEX
                    EditorGUILayout.BeginHorizontal();
                    {
                        armorClip.minDex = EditorGUILayout.IntField("Min DEX", (int)armorClip.minDex);
                        GUILayout.FlexibleSpace();
                        armorClip.maxDex = EditorGUILayout.IntField("Max DEX", (int)armorClip.maxDex);
                    }
                    EditorGUILayout.EndHorizontal();

                    //LUCK
                    EditorGUILayout.BeginHorizontal();
                    {
                        armorClip.minLuc = EditorGUILayout.IntField("Min LUC", (int)armorClip.minLuc);
                        GUILayout.FlexibleSpace();
                        armorClip.maxLuc = EditorGUILayout.IntField("Max LUC", (int)armorClip.maxLuc);
                    }
                    EditorGUILayout.EndHorizontal();

                    //INT
                    EditorGUILayout.BeginHorizontal();
                    {
                        armorClip.minInt = EditorGUILayout.IntField("Min INT", (int)armorClip.minInt);
                        GUILayout.FlexibleSpace();
                        armorClip.maxInt = EditorGUILayout.IntField("Max INT", (int)armorClip.maxInt);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                EditorHelper.GetSpcae(1);

                EditorGUILayout.BeginVertical("helpbox");
                {
                    //����
                    EditorGUILayout.BeginHorizontal();
                    {
                        armorClip.minDef = EditorGUILayout.IntField("�ּ� ����", armorClip.minDef);
                        GUILayout.FlexibleSpace();
                        armorClip.maxDef = EditorGUILayout.IntField("�ִ� ����", armorClip.maxDef);
                    }
                    EditorGUILayout.EndHorizontal();

                    //���� ����
                    EditorGUILayout.BeginHorizontal();
                    {
                        armorClip.minMagicDef = EditorGUILayout.IntField("�ּ� ��������", armorClip.minMagicDef);
                        GUILayout.FlexibleSpace();
                        armorClip.maxMagicDef = EditorGUILayout.IntField("�ִ� ��������", armorClip.maxMagicDef);
                    }
                    EditorGUILayout.EndHorizontal();

                    //ü�� ���
                    EditorGUILayout.BeginHorizontal();
                    {
                        armorClip.minHpRegeneration = EditorGUILayout.FloatField("�ּ� �ʴ� ü�����", armorClip.minHpRegeneration);
                        GUILayout.FlexibleSpace();
                        armorClip.maxHpRegeneration = EditorGUILayout.FloatField("�ִ� �ʴ� ü�����", armorClip.maxHpRegeneration);
                    }
                    EditorGUILayout.EndHorizontal();

                    //���׹̳� ���
                    EditorGUILayout.BeginHorizontal();
                    {
                        armorClip.minStRegeneration = EditorGUILayout.FloatField("�ּ� �ʴ� ���׹̳���� ", armorClip.minStRegeneration);
                        GUILayout.FlexibleSpace();
                        armorClip.maxStRegeneration = EditorGUILayout.FloatField("�ִ� �ʴ� ���׹̳����", armorClip.maxStRegeneration);
                    }
                    EditorGUILayout.EndHorizontal();

                    //ȸ����
                    EditorGUILayout.BeginHorizontal();
                    {
                        armorClip.minEvasion = EditorGUILayout.FloatField("�ּ� ȸ����", armorClip.minEvasion);
                        GUILayout.FlexibleSpace();
                        armorClip.maxEvasion = EditorGUILayout.FloatField("�ִ� ȸ����", armorClip.maxEvasion);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();


                EditorHelper.GetSpcae(1);

                EditorGUILayout.BeginHorizontal();
                {
                    armorClip.enchantLeftCount = EditorGUILayout.IntField("��þƮ ���� Ƚ��", armorClip.enchantLeftCount);
                    GUILayout.FlexibleSpace();
                    armorClip.enchantLimitCount = EditorGUILayout.IntField("��þƮ ���� ����", armorClip.enchantLimitCount);
                }
                EditorGUILayout.EndHorizontal();

                EditorHelper.GetSpcae(1);

                EditorGUILayout.BeginHorizontal();
                {
                    armorClip.buyCost = EditorGUILayout.IntField("���� ����", armorClip.buyCost, GUILayout.Width(300));
                    GUILayout.FlexibleSpace();
                    armorClip.sellCost = EditorGUILayout.IntField("�Ǹ� ����", armorClip.sellCost, GUILayout.Width(300));
                }
                EditorGUILayout.EndHorizontal();

                armorClip.repurchaseCost = EditorGUILayout.IntField("�籸�� ����", armorClip.repurchaseCost, GUILayout.Width(300));


                EditorHelper.GetSpcae(1);

                EditorGUILayout.BeginVertical("helpbox");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        armorClip.isDefaultPotentialPercent = EditorGUILayout.Toggle("����ɷ� ��� % �ڵ� ����", armorClip.isDefaultPotentialPercent, GUILayout.Width(160));
                    }
                    EditorGUILayout.EndHorizontal();


                    if (!armorClip.isDefaultPotentialPercent)
                    {
                        EditorGUILayout.LabelField("�� 100%  [ ���� : " + (armorClip.potentialPercent[0] + armorClip.potentialPercent[1]
                            + armorClip.potentialPercent[2] + armorClip.potentialPercent[3] + armorClip.potentialPercent[4]) + " % ]");
                        EditorGUILayout.BeginVertical("helpbox");
                        {
                            armorClip.potentialPercent[0] = EditorGUILayout.IntField("None ��� ", armorClip.potentialPercent[0]);
                            armorClip.potentialPercent[1] = EditorGUILayout.IntField("Normal ��� ", armorClip.potentialPercent[1]);
                            armorClip.potentialPercent[2] = EditorGUILayout.IntField("Rare ��� ", armorClip.potentialPercent[2]);
                            armorClip.potentialPercent[3] = EditorGUILayout.IntField("Unique ��� ", armorClip.potentialPercent[3]);
                            armorClip.potentialPercent[4] = EditorGUILayout.IntField("Legendary ��� ", armorClip.potentialPercent[4]);
                        }
                        EditorGUILayout.EndVertical();
                    }
                    else
                    {
                        EditorGUILayout.LabelField(" �⺻ �� [  None[30%]  Normal[40%]  Rare[20%]  Unique[6%]  Legendary[4%]  ]", EditorStyles.boldLabel);
                    }
                }
                EditorGUILayout.EndVertical();

            }
            EditorGUILayout.EndVertical();
        }
    }

    private void AccessoryGUI()
    {
        if (accessoryClip == null || accessoryClip != itemData.allItemClips[realdSelection])
            SetSortTypeAndClassType();

        if (accessoryClip != null)
        {
            EditorGUILayout.BeginVertical("helpbox");
            {
                GUIStyle statsSettingGUi = new GUIStyle(EditorStyles.boldLabel);
                statsSettingGUi.fontSize = 14;
                EditorGUILayout.LabelField("Stats Settings", statsSettingGUi);
                EditorHelper.GetSpcae(3);


                EditorGUILayout.BeginHorizontal();
                {
                    accessoryClip.uiItemName = EditorGUILayout.TextField("�Ǽ��縮 UI �̸� ", accessoryClip.uiItemName);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    accessoryClip.accessoryType = (AccessoryType)EditorGUILayout.EnumPopup("Accessory Parts Type", accessoryClip.accessoryType);

                }
                EditorGUILayout.EndHorizontal();

                EditorHelper.GetSpcae(1);

                EditorGUILayout.BeginHorizontal();
                {
                    accessoryClip.itemLevel = EditorGUILayout.IntField("�Ǽ��縮 ���� ", accessoryClip.itemLevel);
                    GUILayout.FlexibleSpace();
                    accessoryClip.requiredLevel = EditorGUILayout.IntField("�ʿ� ���� ����", accessoryClip.requiredLevel);
                }
                EditorGUILayout.EndHorizontal();

                EditorHelper.GetSpcae(1);
                EditorGUILayout.BeginVertical("helpbox");
                {
                    //All Stats
                    EditorGUILayout.BeginHorizontal();
                    {
                        accessoryClip.minAllStats = EditorGUILayout.IntField("Min AllStats", accessoryClip.minAllStats);
                        GUILayout.FlexibleSpace();
                        accessoryClip.maxAllStats = EditorGUILayout.IntField("Max AllStats", accessoryClip.maxAllStats);
                    }
                    EditorGUILayout.EndHorizontal();
                    //STR
                    EditorGUILayout.BeginHorizontal();
                    {
                        accessoryClip.minStr = EditorGUILayout.IntField("Min STR", (int)accessoryClip.minStr);
                        GUILayout.FlexibleSpace();
                        accessoryClip.maxStr = EditorGUILayout.IntField("Max STR", (int)accessoryClip.maxStr);
                    }
                    EditorGUILayout.EndHorizontal();

                    //DEX
                    EditorGUILayout.BeginHorizontal();
                    {
                        accessoryClip.minDex = EditorGUILayout.IntField("Min DEX", (int)accessoryClip.minDex);
                        GUILayout.FlexibleSpace();
                        accessoryClip.maxDex = EditorGUILayout.IntField("Max DEX", (int)accessoryClip.maxDex);
                    }
                    EditorGUILayout.EndHorizontal();

                    //LUCK
                    EditorGUILayout.BeginHorizontal();
                    {
                        accessoryClip.minLuc = EditorGUILayout.IntField("Min LUC", (int)accessoryClip.minLuc);
                        GUILayout.FlexibleSpace();
                        accessoryClip.maxLuc = EditorGUILayout.IntField("Max LUC", (int)accessoryClip.maxLuc);
                    }
                    EditorGUILayout.EndHorizontal();

                    //INT
                    EditorGUILayout.BeginHorizontal();
                    {
                        accessoryClip.minInt = EditorGUILayout.IntField("Min INT", (int)accessoryClip.minInt);
                        GUILayout.FlexibleSpace();
                        accessoryClip.maxInt = EditorGUILayout.IntField("Max INT", (int)accessoryClip.maxInt);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                EditorHelper.GetSpcae(1);


                EditorGUILayout.BeginVertical("helpbox");
                {
                    //���ݷ�
                    EditorGUILayout.BeginHorizontal();
                    {
                        accessoryClip.minAtk = EditorGUILayout.FloatField("�ּ� ���ݷ�", accessoryClip.minAtk);
                        GUILayout.FlexibleSpace();
                        accessoryClip.maxAtk = EditorGUILayout.FloatField("�ִ� ���ݷ�", accessoryClip.maxAtk);
                    }
                    EditorGUILayout.EndHorizontal();

                    //���ݼӵ�
                    EditorGUILayout.BeginHorizontal();
                    {
                        accessoryClip.minAtkSpeed = EditorGUILayout.FloatField("�ּ� ���ݷ¼ӵ�", accessoryClip.minAtkSpeed);
                        GUILayout.FlexibleSpace();
                        accessoryClip.maxAtkSpeed = EditorGUILayout.FloatField("�ִ� ���ݷ¼ӵ�", accessoryClip.maxAtkSpeed);

                    }
                    EditorGUILayout.EndHorizontal();

                    EditorHelper.GetSpcae(1);

                    //ũ��Ƽ�� Ȯ��
                    EditorGUILayout.BeginHorizontal();
                    {
                        accessoryClip.minCriChance = EditorGUILayout.FloatField("�ּ� ũ��Ƽ��Ȯ��", accessoryClip.minCriChance);
                        GUILayout.FlexibleSpace();
                        accessoryClip.maxCriChance = EditorGUILayout.FloatField("�ִ� ũ��Ƽ��Ȯ��", accessoryClip.maxCriChance);
                    }
                    EditorGUILayout.EndHorizontal();

                    //ũ��Ƽ�� ������
                    EditorGUILayout.BeginHorizontal();
                    {
                        accessoryClip.minCriDmg = EditorGUILayout.FloatField("�ּ� ũ��Ƽ�õ�����", accessoryClip.minCriDmg);
                        GUILayout.FlexibleSpace();
                        accessoryClip.maxCriDmg = EditorGUILayout.FloatField("�ִ� ũ��Ƽ�õ�����", accessoryClip.maxCriDmg);
                    }
                    EditorGUILayout.EndHorizontal();

                    //ü�� ���
                    EditorGUILayout.BeginHorizontal();
                    {
                        accessoryClip.minHpRegeneration = EditorGUILayout.FloatField("�ּ� �ʴ� ü�����", accessoryClip.minHpRegeneration);
                        GUILayout.FlexibleSpace();
                        accessoryClip.maxHpRegeneration = EditorGUILayout.FloatField("�ִ� �ʴ� ü�����", accessoryClip.maxHpRegeneration);
                    }
                    EditorGUILayout.EndHorizontal();

                    //���¹̳� ���
                    EditorGUILayout.BeginHorizontal();
                    {
                        accessoryClip.minStRegeneration = EditorGUILayout.FloatField("�ּ� �ʴ� ���׹̳����", accessoryClip.minStRegeneration);
                        GUILayout.FlexibleSpace();
                        accessoryClip.maxStRegeneration = EditorGUILayout.FloatField("�ִ� �ʴ� ���׹̳����", accessoryClip.maxStRegeneration);
                    }
                    EditorGUILayout.EndHorizontal();

                    //����
                    EditorGUILayout.BeginHorizontal();
                    {
                        accessoryClip.minDef = EditorGUILayout.IntField("�ּ� ����", accessoryClip.minDef);
                        GUILayout.FlexibleSpace();
                        accessoryClip.maxDef = EditorGUILayout.IntField("�ִ� ����", accessoryClip.maxDef);
                    }
                    EditorGUILayout.EndHorizontal();

                    //��������
                    EditorGUILayout.BeginHorizontal();
                    {
                        accessoryClip.minMagicDef = EditorGUILayout.IntField("�ּ� ��������", accessoryClip.minMagicDef);
                        GUILayout.FlexibleSpace();
                        accessoryClip.maxMagicDef = EditorGUILayout.IntField("�ִ� ��������", accessoryClip.maxMagicDef);
                    }
                    EditorGUILayout.EndHorizontal();

                    //ȸ����
                    EditorGUILayout.BeginHorizontal();
                    {
                        accessoryClip.minEvasion = EditorGUILayout.FloatField("�ּ� ȸ����", accessoryClip.minEvasion);
                        GUILayout.FlexibleSpace();
                        accessoryClip.maxEvasion = EditorGUILayout.FloatField("�ִ� ȸ����", accessoryClip.maxEvasion);

                    }
                    EditorGUILayout.EndHorizontal();

                }
                EditorGUILayout.EndVertical();

                EditorHelper.GetSpcae(1);

                EditorGUILayout.BeginHorizontal();
                {
                    accessoryClip.enchantLeftCount = EditorGUILayout.IntField("��þƮ ���� Ƚ��", accessoryClip.enchantLeftCount);
                    GUILayout.FlexibleSpace();
                    accessoryClip.enchantLimitCount = EditorGUILayout.IntField("��þƮ ���� ����", accessoryClip.enchantLimitCount);
                }
                EditorGUILayout.EndHorizontal();

                EditorHelper.GetSpcae(1);

                EditorGUILayout.BeginHorizontal();
                {
                    accessoryClip.buyCost = EditorGUILayout.IntField("���� ����", accessoryClip.buyCost, GUILayout.Width(300));
                    GUILayout.FlexibleSpace();
                    accessoryClip.sellCost = EditorGUILayout.IntField("�Ǹ� ����", accessoryClip.sellCost, GUILayout.Width(300));
                }
                EditorGUILayout.EndHorizontal();

                accessoryClip.repurchaseCost = EditorGUILayout.IntField("�籸�� ����", accessoryClip.repurchaseCost, GUILayout.Width(300));

                EditorHelper.GetSpcae(1);

                EditorGUILayout.BeginVertical("helpbox");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        accessoryClip.isDefaultPotentialPercent = EditorGUILayout.Toggle("����ɷ� ��� % �ڵ� ����", accessoryClip.isDefaultPotentialPercent, GUILayout.Width(160));
                    }
                    EditorGUILayout.EndHorizontal();


                    if (!accessoryClip.isDefaultPotentialPercent)
                    {
                        EditorGUILayout.LabelField("�� 100%  [ ���� : " + (accessoryClip.potentialPercent[0] + accessoryClip.potentialPercent[1]
                            + accessoryClip.potentialPercent[2] + accessoryClip.potentialPercent[3] + accessoryClip.potentialPercent[4]) + " % ]");
                        EditorGUILayout.BeginVertical("helpbox");
                        {
                            accessoryClip.potentialPercent[0] = EditorGUILayout.IntField("None ��� ", accessoryClip.potentialPercent[0]);
                            accessoryClip.potentialPercent[1] = EditorGUILayout.IntField("Normal ��� ", accessoryClip.potentialPercent[1]);
                            accessoryClip.potentialPercent[2] = EditorGUILayout.IntField("Rare ��� ", accessoryClip.potentialPercent[2]);
                            accessoryClip.potentialPercent[3] = EditorGUILayout.IntField("Unique ��� ", accessoryClip.potentialPercent[3]);
                            accessoryClip.potentialPercent[4] = EditorGUILayout.IntField("Legendary ��� ", accessoryClip.potentialPercent[4]);
                        }
                        EditorGUILayout.EndVertical();
                    }
                    else
                    {
                        EditorGUILayout.LabelField(" �⺻ �� [  None[30%]  Normal[40%]  Rare[20%]  Unique[6%]  Legendary[4%]  ]", EditorStyles.boldLabel);
                    }
                }
                EditorGUILayout.EndVertical();

            }
            EditorGUILayout.EndVertical();
        }
    }

    private void TitleGUI()
    {
        if (titleClip == null || titleClip != itemData.allItemClips[realdSelection])
            SetSortTypeAndClassType();

        if (titleClip != null)
        {
            EditorGUILayout.BeginVertical("helpbox");
            {
                GUIStyle statsSettingGUi = new GUIStyle(EditorStyles.boldLabel);
                statsSettingGUi.fontSize = 14;
                EditorGUILayout.LabelField("Stats Settings", statsSettingGUi);
                EditorHelper.GetSpcae(3);

                EditorHelper.GetSpcae(1);


                EditorGUILayout.BeginHorizontal();
                {
                    titleClip.uiItemName = EditorGUILayout.TextField("Īȣ�� ", titleClip.uiItemName);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    titleClip.itemLevel = EditorGUILayout.IntField("Īȣ ���� ", titleClip.itemLevel);
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();

                EditorHelper.GetSpcae(1);
                EditorGUILayout.BeginVertical("helpbox");
                {
                    //All Stats
                    EditorGUILayout.BeginHorizontal();
                    {
                        titleClip.minAllStats = EditorGUILayout.IntField("Min AllStats", titleClip.minAllStats);
                        GUILayout.FlexibleSpace();
                        titleClip.maxAllStats = EditorGUILayout.IntField("Max AllStats", titleClip.maxAllStats);
                    }
                    EditorGUILayout.EndHorizontal();
                    //STR
                    EditorGUILayout.BeginHorizontal();
                    {
                        titleClip.minStr = EditorGUILayout.IntField("Min STR", (int)titleClip.minStr);
                        GUILayout.FlexibleSpace();
                        titleClip.maxStr = EditorGUILayout.IntField("Max STR", (int)titleClip.maxStr);
                    }
                    EditorGUILayout.EndHorizontal();

                    //DEX
                    EditorGUILayout.BeginHorizontal();
                    {
                        titleClip.minDex = EditorGUILayout.IntField("Min DEX", (int)titleClip.minDex);
                        GUILayout.FlexibleSpace();
                        titleClip.maxDex = EditorGUILayout.IntField("Max DEX", (int)titleClip.maxDex);
                    }
                    EditorGUILayout.EndHorizontal();

                    //LUCK
                    EditorGUILayout.BeginHorizontal();
                    {
                        titleClip.minLuc = EditorGUILayout.IntField("Min LUC", (int)titleClip.minLuc);
                        GUILayout.FlexibleSpace();
                        titleClip.maxLuc = EditorGUILayout.IntField("Max LUC", (int)titleClip.maxLuc);
                    }
                    EditorGUILayout.EndHorizontal();

                    //INT
                    EditorGUILayout.BeginHorizontal();
                    {
                        titleClip.minInt = EditorGUILayout.IntField("Min INT", (int)titleClip.minInt);
                        GUILayout.FlexibleSpace();
                        titleClip.maxInt = EditorGUILayout.IntField("Max INT", (int)titleClip.maxInt);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                EditorHelper.GetSpcae(1);
                EditorGUILayout.BeginVertical("helpbox");
                {      
                    //���ݷ�
                    EditorGUILayout.BeginHorizontal();
                    {
                        titleClip.minAtk = EditorGUILayout.FloatField("�ּ� ���ݷ�", titleClip.minAtk);
                        GUILayout.FlexibleSpace();
                        titleClip.maxAtk = EditorGUILayout.FloatField("�ִ� ���ݷ�", titleClip.maxAtk);
                    }
                    EditorGUILayout.EndHorizontal();

                    //���ݼӵ�
                    EditorGUILayout.BeginHorizontal();
                    {
                        titleClip.minAtkSpeed = EditorGUILayout.FloatField("�ּ� ���ݷ¼ӵ�", titleClip.minAtkSpeed);
                        GUILayout.FlexibleSpace();
                        titleClip.maxAtkSpeed = EditorGUILayout.FloatField("�ִ� ���ݷ¼ӵ�", titleClip.maxAtkSpeed);

                    }
                    EditorGUILayout.EndHorizontal();

                    EditorHelper.GetSpcae(1);

                    //ũ��Ƽ�� Ȯ��
                    EditorGUILayout.BeginHorizontal();
                    {
                        titleClip.minCriChance = EditorGUILayout.FloatField("�ּ� ũ��Ƽ��Ȯ��", titleClip.minCriChance);
                        GUILayout.FlexibleSpace();
                        titleClip.maxCriChance = EditorGUILayout.FloatField("�ִ� ũ��Ƽ��Ȯ��", titleClip.maxCriChance);
                    }
                    EditorGUILayout.EndHorizontal();

                    //ũ��Ƽ�� ������
                    EditorGUILayout.BeginHorizontal();
                    {
                        titleClip.minCriDmg = EditorGUILayout.FloatField("�ּ� ũ��Ƽ�õ�����", titleClip.minCriDmg);
                        GUILayout.FlexibleSpace();
                        titleClip.maxCriDmg = EditorGUILayout.FloatField("�ִ� ũ��Ƽ�õ�����", titleClip.maxCriDmg);
                    }
                    EditorGUILayout.EndHorizontal();

                    //ü�� ���
                    EditorGUILayout.BeginHorizontal();
                    {
                        titleClip.minHpRegeneration = EditorGUILayout.FloatField("�ּ� �ʴ� ü�����", titleClip.minHpRegeneration);
                        GUILayout.FlexibleSpace();
                        titleClip.maxHpRegeneration = EditorGUILayout.FloatField("�ִ� �ʴ� ü�����", titleClip.maxHpRegeneration);
                    }
                    EditorGUILayout.EndHorizontal();

                    //���¹̳� ���
                    EditorGUILayout.BeginHorizontal();
                    {
                        titleClip.minStRegeneration = EditorGUILayout.FloatField("�ּ� �ʴ� ���׹̳����", titleClip.minStRegeneration);
                        GUILayout.FlexibleSpace();
                        titleClip.maxStRegeneration = EditorGUILayout.FloatField("�ִ� �ʴ� ���׹̳����", titleClip.maxStRegeneration);
                    }
                    EditorGUILayout.EndHorizontal();

                    //����
                    EditorGUILayout.BeginHorizontal();
                    {
                        titleClip.minDef = EditorGUILayout.IntField("�ּ� ����", titleClip.minDef);
                        GUILayout.FlexibleSpace();
                        titleClip.maxDef = EditorGUILayout.IntField("�ִ� ����", titleClip.maxDef);
                    }
                    EditorGUILayout.EndHorizontal();

                    //��������
                    EditorGUILayout.BeginHorizontal();
                    {
                        titleClip.minMagicDef = EditorGUILayout.IntField("�ּ� ��������", titleClip.minMagicDef);
                        GUILayout.FlexibleSpace();
                        titleClip.maxMagicDef = EditorGUILayout.IntField("�ִ� ��������", titleClip.maxMagicDef);
                    }
                    EditorGUILayout.EndHorizontal();

                }
                EditorGUILayout.EndVertical();
                EditorHelper.GetSpcae(1);

                EditorGUILayout.BeginVertical("helpbox");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        titleClip.isDefaultPotentialPercent = EditorGUILayout.Toggle("����ɷ� ��� % �ڵ� ����", titleClip.isDefaultPotentialPercent, GUILayout.Width(160));
                    }
                    EditorGUILayout.EndHorizontal();


                    if (!titleClip.isDefaultPotentialPercent)
                    {
                        EditorGUILayout.LabelField("�� 100%  [ ���� : " + (titleClip.potentialPercent[0] + titleClip.potentialPercent[1]
                            + titleClip.potentialPercent[2] + titleClip.potentialPercent[3] + titleClip.potentialPercent[4]) + " % ]");
                        EditorGUILayout.BeginVertical("helpbox");
                        {
                            titleClip.potentialPercent[0] = EditorGUILayout.IntField("None ��� ", titleClip.potentialPercent[0]);
                            titleClip.potentialPercent[1] = EditorGUILayout.IntField("Normal ��� ", titleClip.potentialPercent[1]);
                            titleClip.potentialPercent[2] = EditorGUILayout.IntField("Rare ��� ", titleClip.potentialPercent[2]);
                            titleClip.potentialPercent[3] = EditorGUILayout.IntField("Unique ��� ", titleClip.potentialPercent[3]);
                            titleClip.potentialPercent[4] = EditorGUILayout.IntField("Legendary ��� ", titleClip.potentialPercent[4]);
                        }
                        EditorGUILayout.EndVertical();
                    }
                    else
                    {
                        EditorGUILayout.LabelField(" �⺻ �� [  None[30%]  Normal[40%]  Rare[20%]  Unique[6%]  Legendary[4%]  ]", EditorStyles.boldLabel);
                    }
                }
                EditorGUILayout.EndVertical();

            }
            EditorGUILayout.EndVertical();
        }
    }

    private void PosionGUI()
    {
        if (posionClip == null || posionClip != itemData.allItemClips[realdSelection])
            SetSortTypeAndClassType();

        if (posionClip != null)
        {
            EditorGUILayout.BeginVertical("helpbox");
            {
                GUIStyle statsSettingGUi = new GUIStyle(EditorStyles.boldLabel);
                statsSettingGUi.fontSize = 14;
                EditorGUILayout.LabelField("Posion Settings", statsSettingGUi);
                EditorHelper.GetSpcae(3);

                EditorHelper.GetSpcae(1);


                EditorGUILayout.BeginHorizontal();
                {
                    posionClip.uiItemName = EditorGUILayout.TextField("���� UI �̸� ", posionClip.uiItemName);
                }
                EditorGUILayout.EndHorizontal();

                EditorHelper.GetSpcae(1);

                EditorGUILayout.BeginHorizontal();
                {
                 //   posionClip.posionType = (BuffType)EditorGUILayout.EnumPopup("���� Ÿ��", posionClip.posionType);
                }
                EditorGUILayout.EndHorizontal();
                EditorHelper.GetSpcae(1);

                EditorGUILayout.BeginHorizontal();
                {
                    //posionClip.posionValue = EditorGUILayout.FloatField("���� �� ", posionClip.posionValue);
                    GUILayout.FlexibleSpace();
                    //posionClip.durationTime = EditorGUILayout.FloatField("���� �ð�", posionClip.durationTime);
                }
                EditorGUILayout.EndHorizontal();
                EditorHelper.GetSpcae(1);
                EditorGUILayout.BeginHorizontal();
                {
                    posionClip.buyCost = EditorGUILayout.IntField("���� ���� ", posionClip.buyCost, GUILayout.Width(300));
                    GUILayout.FlexibleSpace();
                    posionClip.sellCost = EditorGUILayout.IntField("�Ǹ� ���� ", posionClip.sellCost, GUILayout.Width(300));
                }
                EditorGUILayout.EndHorizontal();
                posionClip.repurchaseCost = EditorGUILayout.IntField("�籸�� ����", posionClip.repurchaseCost, GUILayout.Width(300));

                EditorHelper.GetSpcae(1);
            }
            EditorGUILayout.EndVertical();
        }
    }
    private void EnchantGUI()
    {
        if (enchantClip == null || enchantClip != itemData.allItemClips[realdSelection])
            SetSortTypeAndClassType();

        if (enchantClip != null)
        {
            EditorGUILayout.BeginVertical("helpbox");
            {
                GUIStyle statsSettingGUi = new GUIStyle(EditorStyles.boldLabel);
                statsSettingGUi.fontSize = 14;
                EditorGUILayout.LabelField("Enchant Settings", statsSettingGUi);
                EditorHelper.GetSpcae(3);

                EditorGUILayout.BeginHorizontal();
                {
                    enchantClip.uiItemName = EditorGUILayout.TextField("��þƮ UI �̸� ", enchantClip.uiItemName);
                }
                EditorGUILayout.EndHorizontal();
                EditorHelper.GetSpcae(1);
                EditorGUILayout.BeginHorizontal();
                {
                    enchantClip.enchantType = (EnchantType)EditorGUILayout.EnumPopup("��þƮ Ÿ��", enchantClip.enchantType);
                }
                EditorGUILayout.EndHorizontal();
                EditorHelper.GetSpcae(1);
                EditorGUILayout.BeginHorizontal("helpbox");
                {
                    enchantClip.minValue = EditorGUILayout.FloatField("�ּ� �� ", enchantClip.minValue);
                    GUILayout.FlexibleSpace();
                    enchantClip.maxValue = EditorGUILayout.FloatField("�ִ� ��", enchantClip.maxValue);
                }
                EditorGUILayout.EndHorizontal();
                EditorHelper.GetSpcae(1);
                EditorGUILayout.BeginHorizontal();
                {
                    enchantClip.successPercent = EditorGUILayout.FloatField("���� �ۼ�Ʈ ", enchantClip.successPercent);
                    EditorGUILayout.LabelField("%");
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();


                EditorHelper.GetSpcae(1);
                EditorGUILayout.BeginHorizontal();
                {
                    enchantClip.buyCost = EditorGUILayout.IntField("���� ���� ", enchantClip.buyCost, GUILayout.Width(300));
                    GUILayout.FlexibleSpace();
                    enchantClip.sellCost = EditorGUILayout.IntField("�Ǹ� ���� ", enchantClip.sellCost, GUILayout.Width(300));
                }
                EditorGUILayout.EndHorizontal();
                enchantClip.repurchaseCost = EditorGUILayout.IntField("�籸�� ����", enchantClip.repurchaseCost, GUILayout.Width(300));

                EditorHelper.GetSpcae(1);
            }
            EditorGUILayout.EndVertical();
        }
    }
    private void CraftGUI()
    {
        if (craftClip == null || craftClip != itemData.allItemClips[realdSelection])
            SetSortTypeAndClassType();

        if (craftClip != null)
        {
            EditorGUILayout.BeginVertical("helpbox");
            {
                GUIStyle statsSettingGUi = new GUIStyle(EditorStyles.boldLabel);
                statsSettingGUi.fontSize = 14;
                EditorGUILayout.LabelField("Craft Settings", statsSettingGUi);
                EditorHelper.GetSpcae(3);

                EditorHelper.GetSpcae(1);


                EditorGUILayout.BeginHorizontal();
                {
                    craftClip.uiItemName = EditorGUILayout.TextField("���� ��� UI �̸� ", craftClip.uiItemName);
                }
                EditorGUILayout.EndHorizontal();

                EditorHelper.GetSpcae(1);

                EditorGUILayout.BeginHorizontal();
                {
                    craftClip.buyCost = EditorGUILayout.IntField("���� ���� ", craftClip.buyCost, GUILayout.Width(300));
                    GUILayout.FlexibleSpace();
                    craftClip.sellCost = EditorGUILayout.IntField("�Ǹ� ���� ", craftClip.sellCost, GUILayout.Width(300));
                }
                EditorGUILayout.EndHorizontal();

                craftClip.repurchaseCost = EditorGUILayout.IntField("�籸�� ����", craftClip.repurchaseCost, GUILayout.Width(300));

                EditorHelper.GetSpcae(1);
            }
            EditorGUILayout.EndVertical();
        }
    }
    private void ExtraGUI()
    {
        if (extraClip == null || extraClip != itemData.allItemClips[realdSelection])
            SetSortTypeAndClassType();

        if (extraClip != null)
        {
            EditorGUILayout.BeginVertical("helpbox");
            {
                GUIStyle statsSettingGUi = new GUIStyle(EditorStyles.boldLabel);
                statsSettingGUi.fontSize = 14;
                EditorGUILayout.LabelField("Craft Settings", statsSettingGUi);
                EditorHelper.GetSpcae(3);

                EditorHelper.GetSpcae(1);


                EditorGUILayout.BeginHorizontal();
                {
                    extraClip.uiItemName = EditorGUILayout.TextField("��� UI �̸� ", extraClip.uiItemName);
                }
                EditorGUILayout.EndHorizontal();

                EditorHelper.GetSpcae(1);

                EditorGUILayout.BeginHorizontal();
                {
                    extraClip.buyCost = EditorGUILayout.IntField("���� ���� ", extraClip.buyCost, GUILayout.Width(300));
                    GUILayout.FlexibleSpace();
                    extraClip.sellCost = EditorGUILayout.IntField("�Ǹ� ���� ", extraClip.sellCost, GUILayout.Width(300));
                }
                EditorGUILayout.EndHorizontal();
                extraClip.repurchaseCost = EditorGUILayout.IntField("�籸�� ����", extraClip.repurchaseCost, GUILayout.Width(300));

                EditorHelper.GetSpcae(1);
            }
            EditorGUILayout.EndVertical();
        }
    }
    private void QuestGUI()
    {
        if (questClip == null || questClip != itemData.allItemClips[realdSelection])
            SetSortTypeAndClassType();

        if (questClip != null)
        {
            EditorGUILayout.BeginVertical("helpbox");
            {
                GUIStyle statsSettingGUi = new GUIStyle(EditorStyles.boldLabel);
                statsSettingGUi.fontSize = 14;
                EditorGUILayout.LabelField("Quest Settings", statsSettingGUi);
                EditorHelper.GetSpcae(3);

                EditorHelper.GetSpcae(1);


                EditorGUILayout.BeginHorizontal();
                {
                    questClip.uiItemName = EditorGUILayout.TextField("����Ʈ ���� ���  UI �̸� ", questClip.uiItemName);
                }
                EditorGUILayout.EndHorizontal();

                EditorHelper.GetSpcae(1);
             
            }
            EditorGUILayout.EndVertical();
        }
    }

    #endregion
}



public class CategoryEditorWindow : EditorWindow
{
    private bool isOpenAllBtn = false;
    private bool isOpenEquipmentBtn = false;
    private bool isOpenConsumableBtn = false;
    private bool isOpenMaterialBtn = false;
    private bool isOpenQuestBtn = false;


    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical("helpbox", GUILayout.Width(100)); //ALL Data container
            {
                EditorGUILayout.BeginHorizontal( GUILayout.Height(50));
                {
                    EditorGUILayout.Separator();
                    GUILayout.FlexibleSpace();
                    if(GUILayout.Button("All", GUILayout.Width(120), GUILayout.Height(45)))
                    {
                        isOpenAllBtn = true;
                        isOpenEquipmentBtn = false;
                        isOpenConsumableBtn = false;
                        isOpenMaterialBtn = false;
                        isOpenQuestBtn = false;
                    }
                    GUILayout.FlexibleSpace(); 
                    EditorGUILayout.Separator();
                }
               

                EditorGUILayout.EndHorizontal();
                //���⼭ ����
                if (isOpenAllBtn)
                {
                    EditorHelper.GetSpcae(2);

                    EditorGUILayout.BeginHorizontal(GUILayout.Height(30));
                    {
                        EditorGUILayout.Separator();
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("All Data", GUILayout.Width(90), GUILayout.Height(20)))
                        {
                            ItemEditor.clickCategoryType = ItemEditorCategoryType.ALL;
                            ItemEditor.UpdateSort();
                            ItemEditor.selection = 0;
                            ItemEditor.realdSelection = 0;
                            this.Close();
                        }
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.Separator();
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();
                }
            }
            EditorGUILayout.EndVertical();


            EditorGUILayout.BeginVertical("helpbox", GUILayout.Width(100));  //Equipment Container
            {
                EditorGUILayout.BeginHorizontal( GUILayout.Height(50));
                {
                    EditorGUILayout.Separator();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Equipment", GUILayout.Width(120), GUILayout.Height(45)))
                    {
                        isOpenAllBtn = false;
                        isOpenEquipmentBtn = true;
                        isOpenConsumableBtn = false;
                        isOpenMaterialBtn = false;
                        isOpenQuestBtn = false;
                    }
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.Separator();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Separator();

                if(isOpenEquipmentBtn)
                {
                    EditorHelper.GetSpcae(2);

                    EditorGUILayout.BeginHorizontal(GUILayout.Height(30));
                    {
                        EditorGUILayout.Separator();

                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("All Equipment", GUILayout.Width(90), GUILayout.Height(20)))
                        {
                            ItemEditor.clickCategoryType = ItemEditorCategoryType.EQUIPMENT;
                            ItemEditor.UpdateSort();
                            ItemEditor.selection = 0;
                            ItemEditor.realdSelection = 0;
                            this.Close();
                        }
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.Separator();
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal( GUILayout.Height(30));
                    {
                        EditorGUILayout.Separator();

                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Weapon", GUILayout.Width(80), GUILayout.Height(20)))
                        {
                            ItemEditor.clickCategoryType = ItemEditorCategoryType.WEAPON;
                            ItemEditor.UpdateSort();
                            ItemEditor.selection = 0;
                            ItemEditor.realdSelection = 0;
                            this.Close();
                        }
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.Separator();
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal( GUILayout.Height(30));
                    {
                        EditorGUILayout.Separator();

                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Armor", GUILayout.Width(80), GUILayout.Height(20)))
                        {
                            ItemEditor.clickCategoryType = ItemEditorCategoryType.ARMOR;
                            ItemEditor.UpdateSort();
                            ItemEditor.selection = 0;
                            ItemEditor.realdSelection = 0;
                            this.Close();
                        }
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.Separator();
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal( GUILayout.Height(30));
                    {
                        EditorGUILayout.Separator();

                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Accessory", GUILayout.Width(80), GUILayout.Height(20)))
                        {
                            ItemEditor.clickCategoryType = ItemEditorCategoryType.ACCESSORY;
                            ItemEditor.UpdateSort();
                            ItemEditor.selection = 0;
                            ItemEditor.realdSelection = 0;
                            this.Close();
                        }
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.Separator();
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal( GUILayout.Height(30));
                    {
                        EditorGUILayout.Separator();

                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Title", GUILayout.Width(80), GUILayout.Height(20)))
                        {
                            ItemEditor.clickCategoryType = ItemEditorCategoryType.TITLE;
                            ItemEditor.UpdateSort();
                            ItemEditor.selection = 0;
                            ItemEditor.realdSelection = 0;
                            this.Close();
                        }
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.Separator();
                    }
                    EditorGUILayout.EndHorizontal();
                }
               
            }
            EditorGUILayout.EndVertical();


            EditorGUILayout.BeginVertical("helpbox", GUILayout.Width(100));  //Consumable Container
            {
                EditorGUILayout.BeginHorizontal( GUILayout.Height(50));
                {
                    EditorGUILayout.Separator();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Consumable", GUILayout.Width(120), GUILayout.Height(45)))
                    {
                        isOpenAllBtn = false;
                        isOpenEquipmentBtn = false;
                        isOpenConsumableBtn = true;
                        isOpenMaterialBtn = false;
                        isOpenQuestBtn = false;
                    }
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.Separator();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Separator();

                if (isOpenConsumableBtn)
                {
                    EditorHelper.GetSpcae(2);

                    EditorGUILayout.BeginHorizontal(GUILayout.Height(30));
                    {
                        EditorGUILayout.Separator();
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("All Consumable", GUILayout.Width(100), GUILayout.Height(20)))
                        {
                            ItemEditor.clickCategoryType = ItemEditorCategoryType.CONSUMABLE;
                            ItemEditor.UpdateSort();
                            ItemEditor.selection = 0;
                            ItemEditor.realdSelection = 0;
                            this.Close();
                        }
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.Separator();
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal( GUILayout.Height(30));
                    {
                        EditorGUILayout.Separator();

                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Posion", GUILayout.Width(80), GUILayout.Height(20)))
                        {
                            ItemEditor.clickCategoryType = ItemEditorCategoryType.POSION;
                            ItemEditor.UpdateSort();
                            ItemEditor.selection = 0;
                            ItemEditor.realdSelection = 0;
                            this.Close();
                        }
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.Separator();
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal(GUILayout.Height(30));
                    {
                        EditorGUILayout.Separator();

                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Enchant", GUILayout.Width(80), GUILayout.Height(20)))
                        {
                            ItemEditor.clickCategoryType = ItemEditorCategoryType.ENCHANT;
                            ItemEditor.UpdateSort();
                            ItemEditor.selection = 0;
                            ItemEditor.realdSelection = 0;
                            this.Close();
                        }
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.Separator();
                    }
                    EditorGUILayout.EndHorizontal();

                }

            }
            EditorGUILayout.EndVertical();


            EditorGUILayout.BeginVertical("helpbox", GUILayout.Width(100));  //Material Container
            {
                EditorGUILayout.BeginHorizontal( GUILayout.Height(50));
                {
                    EditorGUILayout.Separator();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Material", GUILayout.Width(120), GUILayout.Height(45)))
                    {
                        isOpenAllBtn = false;
                        isOpenEquipmentBtn = false;
                        isOpenConsumableBtn = false;
                        isOpenMaterialBtn = true;
                        isOpenQuestBtn = false;
                    }
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.Separator();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Separator();

                if (isOpenMaterialBtn)
                {
                    EditorHelper.GetSpcae(2);

                    EditorGUILayout.BeginHorizontal( GUILayout.Height(30));
                    {
                        EditorGUILayout.Separator();

                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("All Materials", GUILayout.Width(90), GUILayout.Height(20)))
                        {
                            ItemEditor.clickCategoryType = ItemEditorCategoryType.MATERIAL;
                            ItemEditor.UpdateSort();
                            ItemEditor.selection = 0;
                            ItemEditor.realdSelection = 0;
                            this.Close();
                        }
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.Separator();
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal( GUILayout.Height(30));
                    {
                        EditorGUILayout.Separator();

                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Craft", GUILayout.Width(80), GUILayout.Height(20)))
                        {
                            ItemEditor.clickCategoryType = ItemEditorCategoryType.CRAFT;
                            ItemEditor.UpdateSort();
                            ItemEditor.selection = 0;
                            ItemEditor.realdSelection = 0;
                            this.Close();
                        }
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.Separator();
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal( GUILayout.Height(30));
                    {
                        EditorGUILayout.Separator();

                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Extra", GUILayout.Width(80), GUILayout.Height(20)))
                        {
                            ItemEditor.clickCategoryType = ItemEditorCategoryType.EXTRA;
                            ItemEditor.UpdateSort();
                            ItemEditor.selection = 0;
                            ItemEditor.realdSelection = 0;
                            this.Close();
                        }
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.Separator();
                    }
                    EditorGUILayout.EndHorizontal();

                }

            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("helpbox", GUILayout.Width(100));  //Quest Container
            {
                EditorGUILayout.BeginHorizontal( GUILayout.Height(50));
                {
                    EditorGUILayout.Separator();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Quest", GUILayout.Width(120), GUILayout.Height(45)))
                    {
                        isOpenAllBtn = false;
                        isOpenEquipmentBtn = false;
                        isOpenConsumableBtn = false;
                        isOpenMaterialBtn = false;
                        isOpenQuestBtn = true;
                    }
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.Separator();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Separator();

                if (isOpenQuestBtn)
                {
                    EditorHelper.GetSpcae(2);

                    EditorGUILayout.BeginHorizontal(GUILayout.Height(30));
                    {
                        EditorGUILayout.Separator();

                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("All Quest", GUILayout.Width(90), GUILayout.Height(20)))
                        {
                            ItemEditor.clickCategoryType = ItemEditorCategoryType.QUEST;
                            ItemEditor.UpdateSort();
                            ItemEditor.selection = 0;
                            ItemEditor.realdSelection = 0;
                            this.Close();
                        }
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.Separator();
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();

        }
        EditorGUILayout.EndHorizontal();

    }

  
}