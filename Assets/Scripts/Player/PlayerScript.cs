using PerkSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public int score;
    public int health;
    public int id;
    public bool ready;
    public Transform weaponAttachment;
    public Slider ammoDisplay;
    public AudioSource hurtSound;
    public AudioSource resurrectSound;
    public SpriteRenderer render;
    public float resistance = 0f;

    public Transform playerHud;
    public Transform intermissionHud;

    public PlayerState playerState;

    public PlayerController controller { private set; get; }
    public List<HealthChange> healthChanges { private set; get; } = new List<HealthChange>(); // Storing what changed your health for intermission puproses
    public List<Perk> perks { private set; get; } = new List<Perk>();

    Transform HUD;
    float invulnerabilityTime;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        controller = GetComponent<PlayerController>();
        id = transform.GetSiblingIndex() + 1;
    }

    void Start()
    {
        health = GameRules.current.PLAYER_MAX_HEALTH;
        HUD = GameManager.current.playerHuds.GetChild(id - 1);

        controller.ChangeState(PlayerState.OutOfGame);

        GameEventSystem.current.onDamage += OnDamage;
        GameEventSystem.current.onEndRound += OnEndRound;
    }

    void Update()
    {
        // Maybe move the controller update into here

        // Update the HUD
        HUD.GetComponentInChildren<Slider>().value = (float)health / GameRules.current.PLAYER_MAX_HEALTH;

        // Update invulnerability status
        if (invulnerabilityTime > 0f)
		{
            invulnerabilityTime -= Time.deltaTime;
		}

        if (invulnerabilityTime < 0f)
		{
            ClearInvulnerability();
        }

        // Update perks
        foreach (Perk perk in perks) perk.Tick();
    }

    public void Damage(DamageInfo info)
    {
        if (info.amount == 0) return;

        info.amount = (int)Math.Round(info.amount * (1f - resistance));

        hurtSound.Play();

        if (invulnerabilityTime > 0f) return;

        health -= info.amount;
        healthChanges.Add(new HealthChange(info.amount, info.cause));
        GameEventSystem.current.OnDamage(info);

        if (health <= 0)
        {
            health = 0;
            ChangeState(PlayerState.Dead);
            GameEventSystem.current.OnKill(info);
            GameManager.current.SlowDownTime(.5f, 1f);
        }
        else
		{
            MakeInvulnerable(1f);
        }
    }

    public void Heal(int amount, string cause)
    {
        if (amount == 0) return;

        int newHealth = Math.Min(health + amount, GameRules.current.PLAYER_MAX_HEALTH);
        health = newHealth;
        healthChanges.Add(new HealthChange(newHealth - health, cause));

        if (playerState != PlayerState.Alive && health == GameRules.current.PLAYER_MAX_HEALTH)
        {
            ChangeState(PlayerState.Alive);
            MakeInvulnerable(2f);
        }
    }

    public void GainPerk(Perk perk)
    {
        perk.Claim(this);
        perks.Add(perk);

        if (perks.Count > GameRules.current.PLAYER_MAX_PERKS)
        {
            Perk p = perks[0];
            p.RemoveEvents();
            perks.Remove(p);
        }

        GetComponent<ButtonHint>().ShowHint(perk.title, perk.sprite);
    }

    public void ChangeState(PlayerState newState)
    {
        playerState = newState;
        controller.ChangeState(newState);

        GameEventSystem.current.OnChangeState(id, newState);
    }

    public void MakeInvulnerable(float time)
	{
        invulnerabilityTime += time;
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
    }

    public void ClearInvulnerability()
	{
        invulnerabilityTime = 0;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    void OnDamage(DamageInfo info)
    {
        if (playerState == PlayerState.Ghost && info.attacker == id)
        {
            Heal(GameRules.current.GHOST_KILL_REGEN_PERCENT * info.amount + GameRules.current.GHOST_KILL_REGEN_FLAT, "Kill");
        }
    }

    void OnEndRound()
    {
        // if (playerState == PlayerState.Possession) ChangeState(PlayerState.Ghost);

        /* if (playerState == PlayerState.Dead)
        {
            ChangeState(health == 0 ? PlayerState.Ghost : PlayerState.Alive);
        } 
        else if (playerState == PlayerState.Ghost)
        {
            Heal(GameRules.current.GHOST_ROUND_REGEN, "Ghost Regeneration");
            if (health == GameRules.current.PLAYER_MAX_HEALTH) ChangeState(PlayerState.Alive);
        } */
    }

    void OnDestroy()
    {
        if (!GameEventSystem.current) return;

        GameEventSystem.current.onDamage -= OnDamage;
        GameEventSystem.current.onEndRound -= OnEndRound;
    }
}
