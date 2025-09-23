using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSizeFitter : MonoBehaviour
{
    public enum TextSizeFitterType
    {
        FIT_BOTH,
        FIT_HORIZONTAL,
        FIT_VERTICAL,
    }
    [SerializeField] private Vector2 paddingOffset;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private TextSizeFitterType fitType;
    void Awake()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();
        if (tmpText == null)
            tmpText = GetComponent<TMP_Text>();
    }

   


    [ContextMenu("����")]
    public void ExcuteFitToText()
    {
        tmpText.ForceMeshUpdate(); // �ؽ�Ʈ ������ ������

        Vector2 textSize = tmpText.GetRenderedValues(false); // ���� ��µ� �ؽ�Ʈ ũ��
        if (fitType == TextSizeFitterType.FIT_BOTH)
            rectTransform.sizeDelta = new Vector2(textSize.x + paddingOffset.x, textSize.y + paddingOffset.y);
        else if (fitType == TextSizeFitterType.FIT_HORIZONTAL)
            rectTransform.sizeDelta = new Vector2(textSize.x + paddingOffset.x, rectTransform.sizeDelta.y + paddingOffset.y);
        else if (fitType == TextSizeFitterType.FIT_VERTICAL)
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x + paddingOffset.x, textSize.y + paddingOffset.y);
    }
}
