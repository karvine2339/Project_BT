using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log($"Particle collided with: {other.name}");

        if (other.TryGetComponent(out PlayerCharacter player))
        {
            player.OnDamaged(2f); 
        }
    }
}
