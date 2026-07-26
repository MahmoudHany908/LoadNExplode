using System;
using UnityEngine;

namespace _LoadNExplode._Scripts.Player
{
    public class SpriteHandler : MonoBehaviour
    {
        private Animator anim;
        private SpriteRenderer sprite;

        private void Awake() {
            anim = GetComponent<Animator>();
            sprite = GetComponent<SpriteRenderer>();
        }
        
        public void SwitchToWalk() {
            anim.SafeCrossFade("WalkAnimation", 0.3f);
        }

        public void SwitchToIdle() {
            anim.SafeCrossFade("IdleAnimation", 0.2f);
        }
        
        public void FlipSprite(bool flip) {
            sprite.flipX = flip;
        }
    }
}