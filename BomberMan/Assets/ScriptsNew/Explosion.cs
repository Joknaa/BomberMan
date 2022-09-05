using System;
using UnityEngine;

namespace BomberMan {
    public class Explosion : MonoBehaviour {
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player")) other.gameObject.GetComponent<Player>().OnDeath();
            if (other.gameObject.CompareTag("Enemy")) other.gameObject.GetComponent<Enemy>().OnDeath();
        }
    }
}