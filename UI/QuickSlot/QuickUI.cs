using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuickUI : StaticContainertUI
{
    //�� ������ �ܼ��� ����ϴ� �����. 
    //�׷��� �����Կ��� �巡���ؼ� �����ٰ� ������ �������� �ƴ�.
    //���⼭ �ؾ��� �Ŵ� ���� �κ��� ����ִ� ������, �ش� ��������.
    [SerializeField] private PlayerStateController controller = null; // �̺κ� ���߿� keyborad �Ҷ� �ȱ�ų� �ϱ�?


    protected override void Awake()
    {
        base.Awake();
        uIType = ContainerType.QUICK;
        if (controller == null) controller = GameManager.Instance.Player;
       // inventoryObject.SaveLoadData.LoadInventoryData(inventoryObject);
   }

    private void Update()
    {
        InputKey();
        CheckSlotCoolTimeImg();
    }


    private void InputKey()
    {
        if (!GameManager.Instance.canUseCamera) return;


        if (Input.GetKeyDown(KeyCode.Alpha1))
            slotUIs[staticSlots[0]].ItemUse(controller);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            slotUIs[staticSlots[1]].ItemUse(controller);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            slotUIs[staticSlots[2]].ItemUse(controller);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            slotUIs[staticSlots[3]].ItemUse(controller);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            slotUIs[staticSlots[4]].ItemUse(controller);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            slotUIs[staticSlots[5]].ItemUse(controller);
        else if (Input.GetKeyDown(KeyCode.Alpha7))
            slotUIs[staticSlots[6]].ItemUse(controller);
        else if (Input.GetKeyDown(KeyCode.Alpha8))
            slotUIs[staticSlots[7]].ItemUse(controller);
        else if (Input.GetKeyDown(KeyCode.Alpha9))
            slotUIs[staticSlots[8]].ItemUse(controller);
        else if (Input.GetKeyDown(KeyCode.Alpha0))
            slotUIs[staticSlots[9]].ItemUse(controller);

    }


    private void CheckSlotCoolTimeImg()
    {
        if (controller == null) controller = GameManager.Instance.Player;

        foreach (InventorySlot slot in inventoryObject.slots)
        {
            if (slot.coolTimeUI == null || !slot.item.HaveItem())
            {
                slot.coolTimeUI.Clear();
                continue;
            }

            if (slot.item.itemType == SlotAllowType.SKILL)
            {
                if(controller.skillController.GetThisSkillIsCoolTime(slot.item.id))
                {
                    SkillData data = controller.skillController.GetCoolTimeSkill(slot.item.id);
                    slot.coolTimeUI.CoolTime(data.coolTime, data.skillClip.skillCoolTime);
                }
                else if(!controller.skillController.ExistInCoolTimeList(slot.item.id))
                {
                    slot.coolTimeUI.Clear();
                }
            }
            else
            {
                if(controller.itemCheckController.ItemIsCoolTime(slot.item.id))
                {
                    ItemCheckInfo info = controller.itemCheckController.GetCoolTimeItemInfo(slot.item.id);
                    slot.coolTimeUI.CoolTime(info.currentTimer, info.itemCoolTime);
                }
                else
                {
                    slot.coolTimeUI.Clear();
                }
            }

        }

    }

    protected override void CreateSlotUIs()
    {
        base.CreateSlotUIs();
    }

   

    protected override void SlotFunction(GameObject slot)
    {
        if (uIType != ContainerType.QUICK) return;

        UIHelper.AddEventTrigger(slot.transform.GetChild(1).gameObject, EventTriggerType.BeginDrag, delegate { OnStartDrag(slot); });
        UIHelper.AddEventTrigger(slot.transform.GetChild(1).gameObject, EventTriggerType.Drag, delegate { OnDraging(slot); });
        UIHelper.AddEventTrigger(slot.transform.GetChild(1).gameObject, EventTriggerType.EndDrag, delegate { OnEndDrag(slot, inventoryObject); });
    }
    

    protected override void OnEndDrag(GameObject go, InventoryObject inventory)
    {
        base.OnEndDrag(go, inventory);
        if (MouseUIData.tempDraggingImage != null)
            Destroy(MouseUIData.tempDraggingImage);

        if(MouseUIData.enterUIRoot == null)
            slotUIs[go].RemoveItem();
    }



}
