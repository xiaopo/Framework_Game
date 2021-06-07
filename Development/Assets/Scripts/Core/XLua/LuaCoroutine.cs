using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;
using XLua;

public class LuaCoroutine : MonoBehaviour
{
    void Start()
    {
        gameObject.hideFlags = HideFlags.HideInHierarchy;
    }


    public void YieldAndCallback(object to_yield, Action callback)
    {
        StartCoroutine(CoBody(to_yield, callback));
    }

    private IEnumerator CoBody(object to_yield, Action callback)
    {
        if (to_yield is IEnumerator)
            yield return StartCoroutine((IEnumerator)to_yield);
        else
            yield return to_yield;
        callback();
    }

    private void Update()
    {

    }
}