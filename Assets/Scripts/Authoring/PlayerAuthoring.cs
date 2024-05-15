using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public int maxHealth;
    public float playerInvincibilityTime;
    public class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Player
            {
                expirience = 0,
                maxHealth = authoring.maxHealth,
                currentHealth = authoring.maxHealth,
                playerInvincibilityTime = authoring.playerInvincibilityTime,
                timer = 0f,
                showPopup = false,
            });
        }
    }
}

public struct Player : IComponentData
{
    public int expirience;
    public int maxHealth;
    public int currentHealth;
    public float playerInvincibilityTime;
    public float timer;
    public bool showPopup;
    public int lastRecievedHit;
}
