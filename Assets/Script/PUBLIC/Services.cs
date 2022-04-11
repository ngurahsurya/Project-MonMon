using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Services : MonoBehaviour
{
    public static Services I;
    Dictionary<System.Type, object> services = new Dictionary<System.Type, object>();

    Coroutine calledUpdateRoutine;

    void Awake()
    {
        if (I == null)
            I = this;
    }

    public void Add<T>(object serv, bool checkExists = false)
    {
        // List<Transform> transformList = new List<Transform>();
        if (checkExists && services.ContainsKey(typeof(T)))
            return;
        services[typeof(T)] = serv;
        Transform p = Get<Transform>();
    }

    public T Get<T>()
    {

        if (services.ContainsKey(typeof(T)))
            return (T)services[typeof(T)];
        return default(T);
    }

    public void CallLateUpdate(System.Action a)
    {
        if (calledUpdateRoutine != null)
            StopCoroutine(calledUpdateRoutine);
        calledUpdateRoutine = StartCoroutine(CalledUpdate(a));
    }

    IEnumerator CalledUpdate(System.Action a)
    {
        yield return new WaitForEndOfFrame();
        a.Invoke();
        calledUpdateRoutine = null;
    }
}