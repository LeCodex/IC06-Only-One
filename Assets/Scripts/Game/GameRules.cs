using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    public static GameRules current;

    public int PLAYER_MAX_HEALTH = 100;
    public int PLAYER_ALIVE_SPEED = 500;
    public int PLAYER_GHOST_SPEED = 700;

    public int GHOST_KILL_REGEN = 50;
    public int GHOST_ROUND_REGEN = 0;

    void Awake()
    {
        current = this;
    }
}
