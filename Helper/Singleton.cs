using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject newGameobject = new GameObject(typeof(T).Name, typeof(T));
                    instance = newGameobject.GetComponent<T>();
                    Debug.Log(typeof(T) + "instance ���� : " + instance);

                }
            }
            return instance;
        }
    }



    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // �ߺ��� �ν��Ͻ� ����
            return;
        }
        instance = this as T;
        DontDestroyOnLoad(this.gameObject);
    }

}
