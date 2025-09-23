using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : GenealState
{
    [SerializeField] private Vector3 resurrectionPosition = Vector3.zero;
    [SerializeField] private string deadAnimationName = string.Empty;

    public enum DeadType
    {
        NONE = -1,
        
    }

    protected override void Awake()
    {
        base.Awake();
        controller.AddState(this, ref controller.deadStateHash, this.hashCode);
    }



    public override void Enter(PlayerStateController stateController, int enumType = -1)
    {
        //enum���� �״� Ÿ���� �޾ƿͼ� ����������, �׳� ���������� �� �ִϸ��̼� ����.
        stateController.myAnimator.CrossFade(deadAnimationName, 0.2f);
        stateController.Conditions.DeadSettings();
        stateController.myAnimator.SetBool("IsDead", true);
        Debug.Log("����");

    }

    public override void UpdateAction(PlayerStateController stateController)
    {
        stateController.Conditions.DeadSettings();
        if(Input.GetKeyDown(KeyCode.V))
        {
            stateController.Resurrection(resurrectionPosition);
        }
    }

    public override void Exit(PlayerStateController stateController)
    {
    }

}
