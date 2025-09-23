using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


[Serializable]
public class InventorySlot  
{
    public string objectName = string.Empty;
    public SlotAllowType[] allowSlotType = new SlotAllowType[0];
  
    [NonSerialized] public SlotAllowType currentItemType = SlotAllowType.NONE;
    [NonSerialized] public Action<InventorySlot> OnPreUpdate = null;
    [NonSerialized] public Action<InventorySlot> OnPostUpdate = null;
    [NonSerialized] public InventoryObject parent = null;
     public GameObject slotUI = null;

    public ImageCoolTimeUI coolTimeUI = null;

    public delegate void OnItemUse(Item item);
    private event OnItemUse onItemUse;
    public event OnItemUse OnItemUse_
    {
        add
        {
            if (onItemUse == null || !onItemUse.GetInvocationList().Contains(value))
                onItemUse += value;
        }
        remove
        {
            onItemUse -= value;
        }
    }
    public Item item = null;
    public int amount = 0;
    public bool isActive = false;

    public InventorySlot() => UpdateSlot(new Item(), 0);
    public InventorySlot(Item item, int amount) => UpdateSlot(item, amount);


    public void SetCoolTimeUI(ImageCoolTimeUI coolTimeUI)
    {
        if (coolTimeUI == null) return;

        this.coolTimeUI = coolTimeUI;
        this.coolTimeUI.Clear();
    
    }

    public void AddItem(Item item, int amount) => UpdateSlot(item, amount);
    public void AddAmount(int amount) => UpdateSlot(item, this.amount += amount);
    public void RemoveItem() => UpdateSlot(new Item(), 0);


    public void UpdateSlot(Item item, int amount)
    {
        OnPreUpdate?.Invoke(this);

        objectName = item.objectName;
        this.item = item;
        this.amount = amount;

        OnPostUpdate?.Invoke(this);

    }

    public bool CanPlaceInSlot(Item item)
    {
        if (item == null || allowSlotType.Length <= 0 || item.id < 0)
            return true;

        foreach (SlotAllowType type in allowSlotType)
        {
            if (item.itemType == type)
                return true;
        }

        return false;
    }

    public void ItemUse(PlayerStateController controller)
    {
        if (!item.HaveItem())
        {
            CommonUIManager.Instance.ExcuteGlobalSimpleNotifer("���Կ� ����� �ȵǾ� �ֽ��ϴ�.");
            return;
        }

        if (item.itemType == SlotAllowType.SKILL && item.skillClip != null)
            item.UseSkill(controller);
        else
        {
            if (controller.itemCheckController.CanAddItemToList(item))
            {
                Debug.Log("�ءءءء� ������ ���! ");
                controller.itemCheckController.AddCoolTimeList(item);
                item.UseItem(controller);   //������ ��ɸ� .
                onItemUse?.Invoke(item);    //invenContainer�� Rmove�� ��. -> remove��quick���� �κ��� �� ���� ����.
                UpdateSlot(item, amount);   //�ش� ���� ����.. �̺κ��ΰ�?
                if (amount <= 0) UpdateSlot(new Item(), 0);
            }
            else
                CommonUIManager.Instance.ExcuteGlobalSimpleNotifer("�������� ���� ��� ���Դϴ�.");

        }
    }

}
