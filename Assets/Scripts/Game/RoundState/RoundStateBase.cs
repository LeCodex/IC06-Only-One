using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameRoundState
{
	public abstract class RoundStateBase
	{
		public abstract void EnterState();
		public abstract void Update();
		public abstract void ExitState();
	}
}