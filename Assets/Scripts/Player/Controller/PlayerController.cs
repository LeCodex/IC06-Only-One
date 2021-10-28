using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerControllerState;
using ArenaEnvironment;

public class PlayerController : MonoBehaviour
{
    public float charge;
    public float speed;

    public bool lookingRight = true;
    public bool lookingUp = false;

    public Rigidbody2D rb { private set; get; }
    public Weapon weapon { private set; get; }
    public PlayerScript player { private set; get; }
    public Hazard possessedHazard { private set; get; }
    public bool paused { private set; get; } = false;

    Animator animator;
    string currentAnimation = "";
    List<Hazard> availableHazards = new List<Hazard>();
    Hazard closestHazard;
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

    void Update()
    {
        controllerState.Update(this);

        if (availableHazards.Count == 0)
        {
            closestHazard = null;
        } 
        else
        {
            closestHazard = availableHazards[0];
            foreach (Hazard hazard in availableHazards)
            {
                if (Vector2.Distance(transform.position, hazard.transform.position) < Vector2.Distance(transform.position, closestHazard.transform.position))
                {
                    closestHazard = hazard;
                }
            }
        }
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
    }

    public void DropWeapon()
	{
        // Unattach the weapon
        weapon.transform.parent = null;

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

    public void PossessClosest()
    {
        if (!closestHazard) return;

        possessedHazard = closestHazard;
        ChangeState(PlayerState.Possession);
    }

    public void Unpossess()
    {
        transform.position = possessedHazard.transform.position;
        
        ChangeState(PlayerState.Ghost);
    }

    public void PlayAnimation(string animation)
	{
        if (animation == currentAnimation) return;

        currentAnimation = animation;
        animator.Play(animation);
	}

    public string GetAnimationStateDirection() { return GetAnimationFlipVertical() + GetAnimationFlipHorizontal(); }
    public string GetAnimationFlipHorizontal() { return lookingRight ? "R" : "L"; }
    public string GetAnimationFlipVertical() { return lookingUp ? "U" : "D"; }
}
