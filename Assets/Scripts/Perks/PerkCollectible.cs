using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PerkSystem
{
	public class PerkCollectible : MonoBehaviour
	{
		public Perk perk;

		AudioSource pickupSound;
		SpriteRenderer sprite;
		Collider2D coll;

		void Awake()
		{
			pickupSound = GetComponentInChildren<AudioSource>();
			coll = GetComponentInChildren<Collider2D>();
			sprite = GetComponent<SpriteRenderer>();
			sprite.sprite = perk.sprite;
		}

		// Collectibles should only collide with alive players (same layer as walls/hazards?)
		private void OnTriggerEnter2D(Collider2D collider)
		{
			PlayerScript player = collider.GetComponent<PlayerScript>();

			pickupSound.Play();

			player.GainPerk(perk);
			sprite.enabled = false;
			coll.enabled = false;
			StartCoroutine(DestroyIn(2f));
		}

		IEnumerator DestroyIn(float time)
		{
			yield return new WaitForSeconds(time);

			Destroy(gameObject);
		}
	}
}