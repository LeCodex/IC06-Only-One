using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptions : MonoBehaviour
{
    public static GameOptions current;

    public float volume = 1f;

    void Awake()
    {
        current = this;
    }
}
