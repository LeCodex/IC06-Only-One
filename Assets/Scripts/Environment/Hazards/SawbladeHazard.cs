using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArenaEnvironment
{
    // Moves along a path with user input
    public class SawbladeHazard : Hazard
    {
        public float defaultSpeed; 
        public float maxSpeed;
        public float acceleration;
        public float drag;
        public int damage;
        public Transform pathParent;
        public bool looped;

        Vector2 pathDirection;
        Rigidbody2D rb;
        int oldNode = 0;

        [SerializeField]
        float progress;
        float speed = 0f;
        Transform[] path;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            path = pathParent.GetComponentsInChildren<Transform>().Where(x => x.gameObject.transform.parent != transform.parent).ToArray();
            pathDirection = path[1].position - path[0].position;
            transform.position = (Vector2)path[0].position;
            progress = UnityEngine.Random.Range(0f, path.Length);
        }

		private void FixedUpdate()
		{
            progress += (ghost ? speed : defaultSpeed) / pathDirection.magnitude * Time.fixedDeltaTime;
            speed *= (1 - drag);

            if (looped)
            {
                progress = (progress + path.Length) % path.Length;
            }
            else
            {
                if (progress < 0f || progress >= path.Length)
                {
                    progress = Math.Min(Math.Max(0f, progress), path.Length - float.Epsilon);
                    defaultSpeed *= -1f;
                    speed *= -.8f; // Bounce
                }
            }

            int currentNode = (int)Math.Floor(progress);
            float localProgress = progress - currentNode;
            if (oldNode != currentNode)
            {
                pathDirection = path[(currentNode + 1) % path.Length].position 
                    - path[currentNode].position;
            }
            rb.MovePosition((Vector2)path[currentNode].position + localProgress * pathDirection);

            oldNode = currentNode;
		}

		public override void Tick()
        {
            MoveAlongPath(Vector2.right * Input.GetAxisRaw("Horizontal" + ghost.player.id) + Vector2.up * Input.GetAxisRaw("Vertical" + ghost.player.id));
        }

        void MoveAlongPath(Vector2 direction)
        {
            float force = Vector2.Dot(direction, pathDirection.normalized);
            speed = Math.Max(-maxSpeed, Math.Min(maxSpeed, speed + acceleration * force * Time.fixedDeltaTime));
        }

		public override void OnUnpossess()
		{
			base.OnUnpossess();
            speed = 0f;
		}

		private void OnCollisionEnter2D(Collision2D collision)
        {
            PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
            if (!player) return;
            if (player.playerState != PlayerState.Alive) return;

            player.Damage(new DamageInfo(ghost ? ghost.player.id : -1, player.id, damage, "Sawblade"));
            player.controller.Knockback(.5f, -collision.GetContact(0).normal * 40f);
        }
    }
}
