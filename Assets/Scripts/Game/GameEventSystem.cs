using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class GameEventSystem : MonoBehaviour
{
    static public GameEventSystem current;

    private void Awake()
    {
        current = this;
    }

    public Action onStartRound;
    public void OnStartRound()
    {
        onStartRound?.Invoke();
    }

    public Action<DamageInfo> onDamage;
    public void OnDamage(DamageInfo damageInfo)
    {
        onDamage?.Invoke(damageInfo);
    }

    public Action<DamageInfo> onKill;
    public void OnKill(DamageInfo damageInfo)
    {
        onKill?.Invoke(damageInfo);
    }

    public Action<int, PlayerState> onChangeState;
    public void OnChangeState(int id, PlayerState state)
    {
        onChangeState?.Invoke(id, state);
    }

    public Action onEndRound;
    public void OnEndRound()
    {
        onEndRound?.Invoke();
    }
}

