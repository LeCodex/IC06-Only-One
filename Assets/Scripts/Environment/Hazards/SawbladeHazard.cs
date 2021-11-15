using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArenaEnvironment
{
    // Moves along a path with user input
    public class SawbladeHazard : Hazard
    {
        public float speed;
        public Transform[] path;
        public bool looped;

        [SerializeField]
        [ReadOnlySerialize]
        int currentNode;
        Vector2 pathDirection;
        Rigidbody2D rb;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            currentNode = 0;
            pathDirection = path[1].position - path[0].position;
            transform.position = (Vector2)path[0].position + 0.5f * pathDirection;
        }

        void FixedUpdate()
        {
            rb.velocity = Vector2.Dot(pathDirection.normalized, rb.velocity) * pathDirection.normalized;
            transform.position = (Vector2)path[currentNode].position + GetProgress() * pathDirection;

            if (GetProgress() > 1f)
            {
                if (currentNode < path.Length - 2 || looped)
                {
                    ChangeNode(1);
                }
                else
                {
                    rb.MovePosition(path[path.Length - 1].position);
                    rb.velocity = Vector2.zero;
                }
            }

            if (GetProgress() < 0f)
            {
                if (currentNode > 0 || looped)
                {
                    ChangeNode(-1);
                }
                else
                {
                    rb.MovePosition(path[0].position);
                    rb.velocity = Vector2.zero;
                }
            }
        }
        
        public override void Tick()
        {
            MoveAlongPath(Vector2.right * Input.GetAxisRaw("Horizontal" + ghost.player.id) + Vector2.up * Input.GetAxisRaw("Vertical" + ghost.player.id));
        }

        void MoveAlongPath(Vector2 direction)
        {
            float force = Vector2.Dot(direction, pathDirection.normalized);
            rb.AddForce(pathDirection * force * Time.fixedDeltaTime, ForceMode2D.Force);
        }

        void ChangeNode(int delta)
        {
            float oldProgress = GetProgress();
            Vector2 oldDirection = pathDirection;

            currentNode = (currentNode + delta + path.Length) % path.Length;
            pathDirection = path[(currentNode + 1) % path.Length].position - path[currentNode].position;

            // Delta should only be 1 or -1
            float newProgress = Math.Sign(delta) * oldProgress - delta;
            rb.MovePosition((Vector2)path[currentNode].position + pathDirection * newProgress);
            rb.velocity = Math.Abs(Vector2.Dot(pathDirection.normalized, rb.velocity)) * pathDirection.normalized;
        }

        float GetProgress()
        {
            Vector2 diff = (transform.position - path[currentNode].position);
            return Vector2.Dot(pathDirection.normalized, diff) / pathDirection.magnitude;
        }

        public override void OnUnpossess()
        {
            rb.velocity /= 2;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
            if (!player) return;
            if (player.playerState != PlayerState.Alive) return;

            player.Damage(new DamageInfo(ghost ? ghost.player.id : -1, player.id, 50, "Sawblade"));
        }
    }
}
