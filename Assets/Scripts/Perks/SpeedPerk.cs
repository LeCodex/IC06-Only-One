﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Speed", menuName = "Perks/Speed", order = 1)]
class SpeedPerk : Perk
{
    public string title = "Speed";
    public float speedGain = 0.2f;

    public override void InitializeEvents()
    {
        GameEventSystem.current.onChangeState += OnChangeState;
        OnChangeState(player.id, player.playerState);
    }

    void OnChangeState(int id, PlayerState state)
    {
        if (id == player.id && state == PlayerState.Alive)
        {
            player.controller.speed += GameRules.current.PLAYER_ALIVE_SPEED * speedGain;
        }
    }

    public override void RemoveEvents()
    {
        if (player.playerState == PlayerState.Alive) player.controller.speed -= GameRules.current.PLAYER_ALIVE_SPEED * speedGain;
        GameEventSystem.current.onChangeState -= OnChangeState;
    }
}

