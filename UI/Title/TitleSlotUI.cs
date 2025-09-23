using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TitleSlotUI : MonoBehaviour
{
    [SerializeField] private TitleSlotData data;

    [Header("[0] Have Data  [1] Empty Data")]
    [SerializeField] private Transform[] datas;

    [Header("[0] LvText  [1] PlayTimeText")]
    [SerializeField] private TMP_Text[] infos;

    [SerializeField] private Button slot_Btn;


    public int SlotIndex => data.SaveSlotIndex + 1;
    public TitleSlotData Data => data;


    public void SlotUpdate(int index)
    {
        for (int i = 0; i < datas.Length; i++)
            datas[i].gameObject.SetActive(false);

        if (data.CanLoadInfo())
        {
            datas[0].gameObject.SetActive(true);
            string[] dataInfo = data.LoadPlayerInfoForTitleSlot();

            infos[0].text = "�÷��̾� Level." + dataInfo[0];
            infos[1].text = "�÷��� Ÿ�� :" + dataInfo[1];

            if (index == 1)
                slot_Btn.interactable = true;
            Debug.Log("�ε�Ϸ� : Slot" + data.SaveSlotIndex);
        }
        else
        {
            datas[1].gameObject.SetActive(true);
            if (index == 1)
                slot_Btn.interactable = false;
            Debug.Log("�ε�Ұ� : Slot" + data.SaveSlotIndex);
        }
    }

}
