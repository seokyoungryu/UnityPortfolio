using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIRootOrderByEventTrigger : MonoBehaviour
{
    [SerializeField] private GameObject orderByWindow = null;
    [SerializeField] private List<UIRootOrderByInfo> infos = new List<UIRootOrderByInfo>();


    protected virtual void Start()
    {
        if (orderByWindow == null) orderByWindow = this.gameObject;
        for (int i = 0; i < infos.Count; i++)
            AddEventTriggerForOrderByWindow(infos[i]);

        SetParentWindowsForOrderBy();
        GameManager.Instance.canUseCamera = true;
        CommonUIManager.Instance.ClearActiveUIs();
    }

    public void FindUIAndExcuteOrderBy(UIRoot uiRoot)
    {
        UIRootOrderByInfo info = GetInfo(uiRoot.UIID);
        ExcuteOrderBy(info);
    }

    public void AddEventTriggerForOrderByWindow(UIRootOrderByInfo info)
    {
        if (info == null)
            return;
        EventTrigger[] triggers = info.rootTr.gameObject.GetComponentsInChildren<EventTrigger>();
        for (int i = 0; i < triggers.Length; i++)
        {
            if (triggers[i] == null) continue;
            UIHelper.AddEventTrigger(triggers[i].gameObject, EventTriggerType.PointerClick, delegate { OnPointerClickOrderByWindow(info); });
        }
        info.uiRoot.StartResetActive();
    }

    private void OnPointerClickOrderByWindow(UIRootOrderByInfo info)
    {
        ExcuteOrderBy(info);
        info.uiRoot.OnItemInformationPointerExit(null);
    }

    private void SetParentWindowsForOrderBy()
    {
        for (int i = 0; i < infos.Count; i++)
            infos[i].orderByTarget.transform.SetParent(orderByWindow.transform);

        /// ��. �ϴ� �ش� Ŭ���� -> ���ӿ�����Ʈ�� �� ������ �̵�
        /// �׸��� commonManager�� activeUI ��Ͽ��� �ش� uiroot�� ������ count�� �̵�.`

    }

    public UIRootOrderByInfo GetInfo(int uiID)
    {
        foreach (UIRootOrderByInfo info in infos)
            if (info.uiRoot.UIID == uiID)
                return info;
        return null;
    }

    public void ExcuteOrderBy(UIRootOrderByInfo info)
    {
        if (info == null) return;
        if(!info.forceOrderBy && CommonUIManager.Instance.isItemInfomationOpen) return;

        info.orderByTarget.transform.SetAsLastSibling();
        CommonUIManager.Instance.OrderLast(info.uiRoot);
    }

}



[System.Serializable]
public class UIRootOrderByInfo
{
    public bool forceOrderBy = false;
    public Transform rootTr = null;
    public GameObject orderByTarget = null;
    public UIRoot uiRoot = null;
}

