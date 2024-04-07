using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Entities;

public class UIStatsPanel : MonoBehaviour
{
    public GameObject StatsPanel;
    public TextMeshProUGUI maxHelth;
    public TextMeshProUGUI lifeRegen;
    public TextMeshProUGUI inviciblyTime;
    public TextMeshProUGUI movementSpeed;
    public TextMeshProUGUI collectingDistance;
    public TextMeshProUGUI empty;
    private UIBridge _bridge;

    private void OnEnable()
    {
        _bridge = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<UIBridge>();
    }


    public void ShowStatsPanel()
    {
        maxHelth.text = _bridge.GetUiMaxHealth();
        //TODO liferegen
        inviciblyTime.text = _bridge.GetUiInviciblyTime();
        movementSpeed.text = _bridge.GetMovementSpeed();
        collectingDistance.text = _bridge.GetCollectingDistance();

        StatsPanel.SetActive(true);
    }

    public void HideStatsPanel()
    {
        StatsPanel.SetActive(false);
    }

}


public partial class UIBridge : SystemBase
{
    private UIStatsPanel _statsPanel;

    public void Paused(bool paused)
    {
        if(_statsPanel == null)
        {
            _statsPanel = GameObject.FindAnyObjectByType<UIStatsPanel>();
        }
        if (paused)
        {
            _statsPanel.ShowStatsPanel();
        } else
        {
            _statsPanel.HideStatsPanel();
        }
    }

    protected override void OnUpdate()
    {
        
    }

    public string GetUiMaxHealth()
    {
        int maxHealth = 0;
        foreach (RefRO<Player> player in SystemAPI.Query<RefRO<Player>>())
        {
            maxHealth = player.ValueRO.maxHealth;
            break;
        } 
        return maxHealth.ToString();
    }

    public string GetUiInviciblyTime()
    {
        float invicibly = 0;
        foreach (RefRO<Player> player in SystemAPI.Query<RefRO<Player>>())
        {
            invicibly = player.ValueRO.playerInvincibilityTime;
            break;
        }
        return invicibly.ToString();
    }

    public string GetCollectingDistance()
    {
        float pullDistance = 0;
        foreach (RefRO<PullCollectables> player in SystemAPI.Query<RefRO<PullCollectables>>())
        {
            pullDistance = player.ValueRO.distance;
            break;
        }
        return pullDistance.ToString();
    }

    public string GetMovementSpeed()
    {
        float maxSpeed = 0;
        foreach (RefRO<PlayerControl> player in SystemAPI.Query<RefRO<PlayerControl>>())
        {
            maxSpeed = player.ValueRO.maxSpeed;
            break;
        }
        return maxSpeed.ToString();
    }
}