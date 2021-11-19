using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PerkSystem
{
	public class PerkCollectible : MonoBehaviour
	{
		public Perk perk;

		void Awake()
		{
			SpriteRenderer sprite = GetComponent<SpriteRenderer>();
			sprite.sprite = perk.sprite;
		}

		// Collectibles should only collide with alive players (same layer as walls/hazards?)
		private void OnTriggerEnter2D(Collider2D collider)
		{
			PlayerScript player = collider.GetComponent<PlayerScript>();

			player.GainPerk(perk);
			Destroy(gameObject);
		}
	}
}