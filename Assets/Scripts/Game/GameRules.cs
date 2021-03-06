using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    public static GameRules current;

    public int GAME_MAX_SCORE = 10;

    public int PLAYER_MAX_HEALTH = 100;
    public int PLAYER_ALIVE_SPEED = 5;
    public int PLAYER_GHOST_SPEED = 7;
    public int PLAYER_MAX_PERKS = 3;

    public int GHOST_KILL_REGEN_PERCENT = 0;
    public int GHOST_KILL_REGEN_FLAT = 50;

    void Awake()
    {
        current = this;
    }
}
