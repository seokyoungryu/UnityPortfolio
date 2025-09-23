using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotContainer : MonoBehaviour
{
    [SerializeField] private QuickUI[] quickSlots;

    public QuickUI[] QuickSlots => quickSlots;

    /// <summary>
    /// ������ Item Count ������Ʈ
    /// </summary>
    public void UpdateItemCountInQuick(InventoryContainterUI inventoryContainterUI, Item item)
    {
        if (quickSlots == null || quickSlots.Length <= 0)
            return;

        for (int i = 0; i < quickSlots.Length; i++)
        {
            for (int x = 0; x < quickSlots[i].staticSlots.Length; x++)
            {
                InventorySlot slot = quickSlots[i].slotUIs[quickSlots[i].staticSlots[x]];

                if (!slot.item.HaveItem())
                    continue;
                if(slot.item.skillClip == null && slot.item.id == item.id)
                {
                    int count = inventoryContainterUI.GetHaveItemCount(item);
                    if (count <= 0) slot.UpdateSlot(new Item(), 0);
                    else slot.UpdateSlot(item, count);
                    Debug.Log(x+"��° ������ :" + gameObject.name);
                    return;
                }
            }
        }
    }

}
