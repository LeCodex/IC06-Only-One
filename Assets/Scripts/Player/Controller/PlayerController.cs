using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerControllerState;
using ArenaEnvironment;
using WeaponSystem;

public class PlayerController : MonoBehaviour
{
    public float charge;
    public float speed;
    public bool lookingRight = true;
    public bool lookingUp = false;
    public Collider2D projectileCollider;
    public Hazard possessedHazard;
    public Sprite pickUpSpriteHint;

    public Rigidbody2D rb { private set; get; }
    public Weapon weapon { private set; get; }
    public PlayerScript player { private set; get; }
    public bool paused { private set; get; } = false;
    public List<Hazard> availableHazards { private set; get; } = new List<Hazard>();

    Animator animator;
    string currentAnimation = "";
    List<Weapon> availableWeapons = new List<Weapon>();
    ControllerStateBase controllerState;
    Dictionary<PlayerState, ControllerStateBase> controllerStates = new Dictionary<PlayerState, ControllerStateBase>()
	{
        { PlayerState.Alive, new ControllerStateAlive() },
        { PlayerState.Ghost, new ControllerStateGhost() },
        { PlayerState.Dead, new ControllerStateDead() },
        { PlayerState.Possession, new ControllerStatePossession() },
        { PlayerState.OutOfGame, new ControllerStateOOG() }
    };

    void Awake()
    {
        player = GetComponent<PlayerScript>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

	private void Start()
	{
        GameEventSystem.current.onStartRound += OnStartRound;
    }

	void Update()
    {
        controllerState.Update(this);
    }

    void FixedUpdate()
    {
        controllerState.FixedUpdate(this);
    }

	public void Pause()
	{
		paused = true;
	}

    public void Unpause()
	{
		paused = false;
	}

	public void ChangeState(PlayerState newState)
    {
        if (controllerState != null) controllerState.ExitState(this);
        controllerState = controllerStates[newState];
        controllerState.EnterState(this);
    }

    public void TakeWeapon(Weapon newWeapon)
	{
        if (weapon) DropWeapon();

        weapon = newWeapon;

        // Setup the weapon in the player's hands
        weapon.transform.SetParent(player.weaponAttachment);
        weapon.transform.localPosition = Vector2.zero;

        weapon.SetOwner(player);
        UnfindWeapon(newWeapon);
    }

    public void TakeClosestWeapon()
	{
        if (availableWeapons.Count == 0) return;

        Weapon closestWeapon = availableWeapons[0];
        foreach (Weapon wep in availableWeapons)
        {
            if (!wep)
            {
                availableWeapons.Remove(wep);
                break;
            }

            if (Vector2.Distance(transform.position, wep.transform.position) < Vector2.Distance(transform.position, closestWeapon.transform.position))
            {
                closestWeapon = wep;
            }
        }

        if (closestWeapon) TakeWeapon(closestWeapon);
    }

    public void DropWeapon()
	{
        // Unattach the weapon
        weapon.transform.SetParent(null);

        // Drop it at your feet
        weapon.transform.position = transform.position;

        // Tell the weapon that it's not owned anymore
        weapon.SetOwner(null);
	}

    public void FindHazard(Hazard newHazard)
    {
        Debug.Log("Found " + newHazard.gameObject.name);
        availableHazards.Add(newHazard);
    }

    public void UnfindHazard(Hazard hazard)
    {
        Debug.Log("Unfound " + hazard.gameObject.name);
        if (availableHazards.Contains(hazard)) availableHazards.Remove(hazard);
    }

    public void FindWeapon(Weapon wep)
	{
        availableWeapons.Add(wep);

        if (!weapon)
        {
            TakeWeapon(wep);
        }
        else
		{
            GetComponent<ButtonHint>().ShowHint("Take", pickUpSpriteHint);
		}
	}

    public void UnfindWeapon(Weapon wep)
    {
        if (availableWeapons.Contains(wep)) availableWeapons.Remove(wep);
    }

    public void PlayAnimation(string animation)
	{
        if (animation == currentAnimation) return;

        currentAnimation = animation;
        animator.Play(animation);
	}

    void OnStartRound()
	{
        availableHazards.Clear();
        availableWeapons.Clear();
	}

    public string GetAnimationStateDirection() { return GetAnimationFlipVertical() + GetAnimationFlipHorizontal(); }
    public string GetAnimationFlipHorizontal() { return lookingRight ? "R" : "L"; }
    public string GetAnimationFlipVertical() { return lookingUp ? "U" : "D"; }

	private void OnDestroy()
	{
        if (!GameEventSystem.current) return;

        GameEventSystem.current.onStartRound -= OnStartRound;
    }
}
