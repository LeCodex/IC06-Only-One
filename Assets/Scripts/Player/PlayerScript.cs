using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int health = 100;
    public int id;
    public bool ready;
    public Transform weaponAttachment;

    public PlayerState playerState;

    public PlayerController controller { private set; get; }

    List<Perk> perks = new List<Perk>();
    List<HealthChange> healthChanges = new List<HealthChange>(); // Storing what changed your health for intermission puproses

    void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    void Start()
    {
        ChangeState(playerState);

        GameEventSystem.current.onDamage += OnDamage;
        GameEventSystem.current.onEndRound += OnEndRound;
    }

    void Update()
    {
        // Maybe move the controller update into here
    }

    public void Damage(DamageInfo info)
    {
        if (info.amount == 0) return;

        ChangeState(PlayerState.Dead);

        health -= info.amount;
        healthChanges.Add(new HealthChange(info.amount, info.cause));
        GameEventSystem.current.OnDamage(info);

        if (health <= 0)
        {
            health = 0;
            GameEventSystem.current.OnKill(info);
        }
    }

    public void Heal(int amount, string cause)
    {
        if (amount == 0) return;
        
        health = Math.Min(health + amount, GameRules.current.PLAYER_MAX_HEALTH);
        healthChanges.Add(new HealthChange(amount, cause));
    }

    public void GainPerk(Perk perk)
    {
        perk.Claim(this);
        perks.Add(perk);
    }

    public void ChangeState(PlayerState newState)
    {
        playerState = newState;
        controller.ChangeState(newState);

        GameEventSystem.current.OnChangeState(id, newState);
    }

    void OnDamage(DamageInfo info)
    {
        if (playerState == PlayerState.Ghost && info.attacker == id)
        {
            Heal(GameRules.current.GHOST_KILL_REGEN, "Kill");
        }
    }

    void OnEndRound()
    {
        if (playerState == PlayerState.Possession) ChangeState(PlayerState.Ghost);

        if (playerState == PlayerState.Dead)
        {
            ChangeState(health == 0 ? PlayerState.Ghost : PlayerState.Alive);
        } 
        else if (playerState == PlayerState.Ghost)
        {
            Heal(GameRules.current.GHOST_ROUND_REGEN, "Ghost Regeneration");
            if (health == GameRules.current.PLAYER_MAX_HEALTH) ChangeState(PlayerState.Alive);
        }
    }

    void OnDestroy()
    {
        if (!GameEventSystem.current) return;

        GameEventSystem.current.onDamage -= OnDamage;
        GameEventSystem.current.onEndRound -= OnEndRound;
    }
}
