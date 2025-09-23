using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="AI/FSM/State Database")]
public class AIStateDatabase : ScriptableObject
{
    public State startState = null;

    public List<State> enemyStates = null;  //�� state�� ���� ����°� �ƴ϶�  Action, decisions�� ��ũ���ͺ�� ��������, �ش� ��ӹ��� 
                                            // OrcStateDataBase�� ��ũ���ͺ��� �����ϰ� �� orc�����ͺ���Ʈ�� state�� ����ϴ½�����.
                                            // State�� ������ �ְ� �����Ҽ��������� ���⿡ ����ؼ��˾ƺ��� �Ͱ� ��. 


    //start�� null�� �ƴϸ�. �ش� state�� ����� ��ȯ state�� enemyStates�� �߰��ϱ�??
    private void OnValidate()
    {
        SettingEnemyStatesList();
    }

    [ContextMenu("Register EnemyStates")]
    private void SettingEnemyStatesList()
    {
        if (startState == null) return;

        enemyStates.Clear();
        RegisterNewState(startState);

        Queue<State> statesToProcess = new Queue<State>(enemyStates);

        while (statesToProcess.Count > 0)
        {
            State currentState = statesToProcess.Dequeue();
            RegisterNewState(currentState);
        }
    }

    private void RegisterNewState(State state)
    {
        if (state == null) return;

        for (int i = 0; i < state.transitions.Length; i++)
        {
            if (state.transitions[i].trueState != null && !enemyStates.Contains(state.transitions[i].trueState))
                enemyStates.Add(state.transitions[i].trueState);

            if (state.transitions[i].falseState != null && !enemyStates.Contains(state.transitions[i].falseState))
                enemyStates.Add(state.transitions[i].falseState);
        }
    }
}

