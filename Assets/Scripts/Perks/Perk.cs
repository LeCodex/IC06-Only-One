﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PerkSystem
{
	public abstract class Perk : ScriptableObject
	{
		public Sprite sprite;

		protected PlayerScript player;

		public void Claim(PlayerScript claimer)
		{
			player = claimer;
			InitializeEvents();
		}

		public abstract void InitializeEvents(); // Use this function to initialize the GameEventSystem's Actions. This prevents the Perk from doing anything until claimed.
		public abstract void RemoveEvents(); // Use this function to remove the GameEventSystem's Actions.

		~Perk()
		{
			RemoveEvents();
		}
	}
}