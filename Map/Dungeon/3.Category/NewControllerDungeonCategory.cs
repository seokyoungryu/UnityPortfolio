using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewControllerDungeonCategory : BaseDungeonCateogry
{
    //������ ScriptableObject�� �޴µ�, ������ �ٸ� ��Ʈ�ѷ��� �ִ� �������� ����������.
    //���÷� ���̽� �ڵ��� ���� , �κ� ���� ��. 
    //�̰͵� So�ε� ������ SO�� �޴� ������.  ��ü ������Ʈ ������ ������������ ��û ���̳����µ� ������ SO�� �����ϸ� ���� ���ð���.

    public override PlayerStateController InitControllerSetting(BaseDungeonTitle title)
    {
        //Init�� -> ���ο� ������Ʈ ����. // �ٵ� �� ������Ʈ�� ���پ�ĳ��.?
        //�� ��� �� ��Ʈ�ѷ��� playerStatecontroller�̸�, ���´� state��
        return null; // ������ ���⿡ ����.
    }
}


//Ÿ��Ʋ
// -> ���� PlayerStatecontroller��, ���� ���ӿ� ����� ExcuteController ����������.
// �������� ���� ��Ʈ�ѷ� ����? ������ �ʿ�� ������.