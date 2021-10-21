using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HealthChange
{
    int amount;
    string cause;

    public HealthChange(int amount, string cause)
    {
        this.amount = amount;
        this.cause = cause;
    }
}
