using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageInfo
{
    public int attacker;
    public int victim;
    public int amount;
    public string cause;

    public DamageInfo(int attacker, int victim, int amount, string cause)
    {
        this.attacker = attacker;
        this.victim = victim;
        this.amount = amount;
        this.cause = cause;
    }
}

