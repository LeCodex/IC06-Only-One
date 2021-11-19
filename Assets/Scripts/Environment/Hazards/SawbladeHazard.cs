using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArenaEnvironment
{
    // Moves along a path with user input
    public class SawbladeHazard : Hazard
    {
        public float maxSpeed;
        public float acceleration;
        public float drag;
        public Transform[] path;
        public bool looped;

        Vector2 pathDirection;
        Rigidbody2D rb;
        int oldNode = 0;

        [SerializeField]
        [ReadOnlySerialize]
        float progress = 0f;
        float speed = 0f;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            pathDirection = path[1].position - path[0].position;
            transform.position = (Vector2)path[0].position;
        }
        
        public override void Tick()
        {
            MoveAlongPath(Vector2.right * Input.GetAxisRaw("Horizontal" + ghost.player.id) + Vector2.up * Input.GetAxisRaw("Vertical" + ghost.player.id));

            progress += speed / pathDirection.magnitude * Time.fixedDeltaTime;
            speed *= (1 - drag);

            if (looped)
            {
                progress = (progress + path.Length) % path.Length;
            }
            else
            {
                progress = Math.Min(Math.Max(0f, progress), path.Length);
            }

            int currentNode = (int)Math.Floor(progress);
            float localProgress = progress - currentNode;
            if (oldNode != currentNode)
            {
                pathDirection = path[(currentNode + 1) % path.Length].position - path[currentNode].position;
            }
            rb.MovePosition((Vector2)path[currentNode].position + localProgress * pathDirection);

            oldNode = currentNode;
        }

        void MoveAlongPath(Vector2 direction)
        {
            float force = Vector2.Dot(direction, pathDirection.normalized);
            speed = Math.Min(maxSpeed, speed + acceleration * force * Time.fixedDeltaTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
            if (!player) return;
            if (player.playerState != PlayerState.Alive) return;

            player.Damage(new DamageInfo(ghost ? ghost.player.id : -1, player.id, 50, "Sawblade"));
            player.controller.Knockback(.5f, collision.GetContact(0).normal * 10f);
        }
    }
}
