using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUI : MonoBehaviour
{
    


    public void CloseUIWindow(GameObject window)
    {
        window.gameObject.SetActive(false);
        //�׸��� esc�� ���� ��Ͽ� �ش� ui �����ϱ�.
    }
}
