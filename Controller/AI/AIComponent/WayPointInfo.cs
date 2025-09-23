using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WayPointInfo
{
    [SerializeField] private int wayPointIndex;
    [SerializeField] private float thisPointWaitTime = 0f;
    private Transform wayPointTr = null;  //�̰� private���� �ϱ�.


    public int WayIndex => wayPointIndex;
    public float ThisPointWaitTime => thisPointWaitTime;
    public Transform WayPointTr { get { return wayPointTr; } set { wayPointTr = value; } }
}
