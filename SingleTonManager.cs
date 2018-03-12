using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class SingleTonManager<T> : MonoBehaviour where T : SingleTonManager<T>
{
    private static T _instance;
    public static T instance
    {
        get
        {
            return _instance;
        }
        private set
        {

        }
    }

    protected virtual void Awake()
    {
        if(Equals(instance,null))
        {
            _instance = (T)this;
            Debug.Log("There active Instance : " + typeof(T));
        }
        else
        {
            Debug.Log("There does not active Instance " + typeof(T));
        }
    }
}

