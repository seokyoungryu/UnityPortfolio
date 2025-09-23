using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// ����ɷ� ScriptableObject - �ش� ����ɷ� ���� �缳�� ���.
/// </summary>
public class PotentialOptionClip   :BaseEditorClip
{
    public PotentialSelectType potentialType;   //  NONE, ALL, COMMON, WEAPON, ARMOR,
    public string potentialNameKorean = string.Empty;
    public string potentialName = string.Empty;   //�ش� ����ɷ� �̸�
    public float potentialValue = 0;              //����ɷ� �ɷ�ġ
    public string description = string.Empty;
    public string lastWord = string.Empty;
    public bool isFloatValue = false;
    public PotentialFunctionObject potentialFunctionObject;

    public PotentialRankValue[] rankValue = new PotentialRankValue[]
        {
           new PotentialRankValue(),
           new PotentialRankValue(),
           new PotentialRankValue(),
           new PotentialRankValue()
        };   //�� ��޺� �ּ�, �ִ� �ɷ�ġ ����.


    public PotentialOptionClip() { }
    public PotentialOptionClip(PotentialOptionClip clip)
    {
        id = clip.id;
        potentialType = clip.potentialType;
        potentialNameKorean = clip.potentialNameKorean;
        potentialName = clip.potentialName;
        potentialValue = clip.potentialValue;
        lastWord = clip.lastWord;
        description = clip.description;
        isFloatValue = clip.isFloatValue;
        potentialFunctionObject = clip.potentialFunctionObject;
    }
    public PotentialOptionClip(string name, int ID, int enumType = 0, PotentialOptionClip clip = null)
    {
        this.id = ID;
        this.potentialName = name;

        switch (enumType)
        {
            case 0:
                this.potentialType = PotentialSelectType.COMMON;
                break;
            case 1:
                this.potentialType = PotentialSelectType.WEAPON;
                break;
            case 2:
                this.potentialType = PotentialSelectType.ARMOR;
                break;
            case 3:
                this.potentialType = PotentialSelectType.ACCESSORY;
                break;
            case 4:
                this.potentialType = PotentialSelectType.Title;
                break;
            default:
                this.potentialType = PotentialSelectType.COMMON;
                Debug.Log("enumType�� �ε����� �ٸ�");
                break;
        }

        if(clip != null)
        {
            this.potentialName = clip.potentialName;
            this.description = clip.description;
            this.isFloatValue = clip.isFloatValue;
            this.potentialFunctionObject = clip.potentialFunctionObject;

            List<PotentialRankValue> values = new List<PotentialRankValue>();
            for (int i = 0; i < clip.rankValue.Length; i++)
                values.Add(clip.rankValue[i].Clone());

            this.rankValue = values.ToArray();

        }
    }
   

    public void SetID(int id)
    {
        this.id = id;
    }

    public void SetRankValues()
    {
        for (int i = 0; i < 4; i++)
        {
            rankValue[i].SetSplitValue();
        }
    }

    /// <summary>
    ///  ��� �� �ּ� �ִ� �� ����
    /// </summary>
    public void SetRandomPotentialValue(ItemPotentialRankType rankType)
    {
        SetRankValues();  //�̰� �����Ϳ��� ȣ���ϱ�.
        switch (rankType)
        {
            case ItemPotentialRankType.NONE:  
                potentialValue = 0;
                break;
            case ItemPotentialRankType.NORMAL:
                potentialValue = rankValue[ConstDefine.PotentialRankNormal].GetRandomPotentialValue();
                break;
            case ItemPotentialRankType.RARE:
                potentialValue = rankValue[ConstDefine.PotentialRankRare].GetRandomPotentialValue();
                break;
            case ItemPotentialRankType.UNIQUE:
                potentialValue = rankValue[ConstDefine.PotentialRankUnique].GetRandomPotentialValue();
                break;
            case ItemPotentialRankType.LEGENDARY:
                potentialValue = rankValue[ConstDefine.PotentialRankLegendary].GetRandomPotentialValue();
                break;

        }

        //Debug.Log("------------------------------------------------");
        //Debug.Log("-" + rankType + " : " + potentialValue +"-");
        //Debug.Log("------------------------------------------------");

    }


}


/// <summary>
/// ��� ����ɷ� ScriptableObject�� �����͸� ����
/// </summary>
public class PotentialDataContainer : ScriptableObject
{
    [Header("ALL Potential")]
    public PotentialOptionClip[] allOptions = new PotentialOptionClip[0];
    [Header("Common Potential")]
    public PotentialOptionClip[] commonOptions = new PotentialOptionClip[0];
    [Header("Weapon Potential")]
    public PotentialOptionClip[] weaponOptions = new PotentialOptionClip[0];
    [Header("Armor Potential")]
    public PotentialOptionClip[] armorOptions = new PotentialOptionClip[0];
    [Header("Accessory Potential")]
    public PotentialOptionClip[] accessoryOptions = new PotentialOptionClip[0];
    [Header("Title Potential")]
    public PotentialOptionClip[] titleOptions = new PotentialOptionClip[0];

}

/// <summary>
/// ����ɷ��� Ȯ���� ��� Ŭ����
/// </summary>
[System.Serializable]
public class PotentialRankValue
{
    public ItemPotentialRankType rank;   // NONE,  NORMAL, RARE, UNIQUE, LEGENDARY , �� ���� min ~ max �� ���� class
    public float minValue = 0;
    public float maxValue = 0;
    public int totalPercentage = 0;
    public bool isSplitValue = true;

    public int splitCount = 0;
    public int[] percentage = new int[5] { 0, 0, 0, 0, 0 }; //[0] : 30% ,[1] : 50%, [2] : 20%

    public float[] splitMinValue = new float[5] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
    public float[] splitMaxValue = new float[5] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };


    public PotentialRankValue Clone()
    {
        PotentialRankValue ret = new PotentialRankValue();
        ret.minValue = minValue;
        ret.maxValue = maxValue;
        ret.totalPercentage = totalPercentage;
        ret.isSplitValue = isSplitValue;
        ret.splitCount = splitCount;
        ret.percentage = percentage; 
        ret.splitMinValue = splitMinValue; 
        ret.splitMaxValue = splitMaxValue;

        return ret;
    }


    //���� �� ���� 3��� �ϴ� ���� isPlitValue = true�� ��� �Լ� ����. false �� �׳� �������� .
    public void SetSplitValue()
    {
        if(isSplitValue)
        {
            float step = (maxValue - minValue) / splitCount;
           
            for (int i = 0; i < splitCount; i++)
            {
                splitMinValue[i] = minValue + (i * step);
                splitMaxValue[i] = minValue + (i + 1) * step;
            }
        }

    }


    public float GetRandomPotentialValue()
    {
        int[] finalRange = new int[splitCount];

        for (int i = 0; i < splitCount; i++)  //percent�� ���� ����
        {
            finalRange[i] = (i == 0) ? percentage[i] : finalRange[i - 1] + percentage[i];
        }
       
        if(isSplitValue)   
        {
            int randomPercent = MathHelper.GetRandom(0, 100);
            for (int i = 0; i < splitCount; i++)
            {
                if( i == 0)
                {
                    if (randomPercent >= 0 && randomPercent <= finalRange[i])
                    {
                        //Debug.Log($"�ۼ�Ʈ�� : {randomPercent} , ������ {0} ���� ũ�� {finalRange[i]} ���� ���� / Splitcount{splitCount}");
                        return MathHelper.GetRandom(splitMinValue[i], splitMaxValue[i]);
                    }
                }
                else
                {
                    if (randomPercent > finalRange[i - 1] && randomPercent <= finalRange[i])
                    {
                        //Debug.Log($"�ۼ�Ʈ�� : {randomPercent} , ������ {finalRange[i - 1]} ���� ũ�� {finalRange[i]} ���� ���� / Splitcount{splitCount}");
                        return MathHelper.GetRandom(splitMinValue[i], splitMaxValue[i]);
                    }
                }
            }
            //Debug.Log("===================");
            //Debug.Log("==���͸� ���⿡�� �� : 0  �ۼ�Ʈ : "+randomPercent +"== ");
            //Debug.Log("===================");

            return 0;
        }
        else
        {
            return MathHelper.GetRandom(minValue, maxValue);
        }

    }



}



/// <summary>
/// ���� �������� ������ �ִ� ����ɷ�. ScriptableObject�� �������� �ʰ� �ش� So �ɷ¸� ����.
/// </summary>
[System.Serializable]
public class OwnPotential
{
    public int id = 0;
    public PotentialOptionClip clipData;
    public PotentialSelectType type;
    public string potentialName = string.Empty;   //�ش� ����ɷ� �̸�
    public float potentialValue = 0;              //����ɷ� �ɷ�ġ
    public string description = string.Empty;
    public string lastWord = string.Empty;


    public OwnPotential(PotentialOptionClip clip)
    {
        id = clip.id;
        clipData = clip;
        type = clip.potentialType;
        potentialName = clip.potentialNameKorean;
        description = clip.description;
        lastWord = clip.lastWord;
        if (clip.isFloatValue)
            potentialValue = clip.potentialValue;
        else
            potentialValue = Mathf.RoundToInt(clip.potentialValue);

    }

    public void Apply(bool isEquip , PlayerStatus playerStatus)
    {
        if (clipData.potentialFunctionObject == null) return;

        if (isEquip)
            clipData.potentialFunctionObject.Apply(potentialValue, playerStatus);
        else
            clipData.potentialFunctionObject.Remove(potentialValue, playerStatus);
    }
}


