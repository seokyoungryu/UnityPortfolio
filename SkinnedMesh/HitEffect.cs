using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public List<SkinnedMeshRenderer> skinnedRenderers;
    public List<MeshRenderer> meshRenderers;

    private SkinnedMeshRenderer[] tempRenderer;
    private MeshRenderer[] tempMeshRenderer;

    public Color damagedColor;
    public Color defenseColor;
    public Color standingColor;
    public Color evasionColor;

    public float startIntensity = 3f; // ������ ���� (0 ~ 1 ����)
    public float endIntensity = 1f; // ������ ���� (0 ~ 1 ����)
    public float duration = 2f; // ��ü ��ȭ�� �ɸ� �ð� (��)
    public float elapsedTime = 0f; // ��� �ð�

    public bool isSkinnedNormalMaterial = false;


    public float TestIntensity = 0f;
    private void Start()
    {
        tempRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        tempMeshRenderer = GetComponentsInChildren<MeshRenderer>();

        foreach (SkinnedMeshRenderer skinned in tempRenderer)
        {
            if (skinned.gameObject.activeInHierarchy)
                skinnedRenderers.Add(skinned);
        }
        foreach (MeshRenderer skinned in tempMeshRenderer)
        {
            if (skinned.gameObject.activeInHierarchy)
                meshRenderers.Add(skinned);
        }
    }

    public void ExcuteDamagedColor()
    {
        StopAllCoroutines();
        StartCoroutine(ColorChange(damagedColor));
    }

    public void ExcuteDefenseColor()
    {
        StopAllCoroutines();
        StartCoroutine(ColorChange(defenseColor));
    }

    public void ExcuteStandinfColor()
    {
        StopAllCoroutines();
        StartCoroutine(ColorChange(standingColor));
    }

    public void ExcuteEvasionColor()
    {
        StopAllCoroutines();
        StartCoroutine(ColorChange(evasionColor));
    }

    IEnumerator ColorChange(Color color)
    {
        elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration); // ��� �ð��� [0, 1] ������ ����ȭ

            float lerp = Mathf.Lerp(startIntensity, endIntensity, t); // ���� ������ ���� ���� ���� ���
            Color TestColor = color * lerp;

            for (int i = 0; i < skinnedRenderers.Count; i++)
            {
                if (!isSkinnedNormalMaterial)
                    skinnedRenderers[i].material.SetColor("_AllColor", TestColor);
                else
                    skinnedRenderers[i].material.color = TestColor;
            }
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                meshRenderers[i].material.SetColor("_AllColor", TestColor);
                meshRenderers[i].material.color = TestColor;
            }

            yield return null;
        }

        for (int i = 0; i < skinnedRenderers.Count; i++)
        {
            if (!isSkinnedNormalMaterial)
                skinnedRenderers[i].material.SetColor("_AllColor", Color.white);
            else
                skinnedRenderers[i].material.color = Color.white;
        }

        for (int i = 0; i < meshRenderers.Count; i++)
        {
            meshRenderers[i].material.SetColor("_AllColor", Color.white);
            meshRenderers[i].material.color = Color.white;
        }
    }

    public void ResetEffect()
    {
        for (int i = 0; i < skinnedRenderers.Count; i++)
        {
            if (!isSkinnedNormalMaterial)
                skinnedRenderers[i].material.SetColor("_AllColor", Color.white);
            else
            {
                skinnedRenderers[i].material.color = Color.white;
            }
        }
        for (int i = 0; i < meshRenderers.Count; i++)
        {
            meshRenderers[i].material.SetColor("_AllColor", Color.white);
            meshRenderers[i].material.color = Color.white;
        }
    }

}


