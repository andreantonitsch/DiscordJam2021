using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpUIHandler : EventListener
{

    public GameObject[] PrototypePowerUps;

    public List<PowerUp> ActivePowerUps;

    public RectTransform ActivePowerUpHolder;

    public EventHandler eh;

    public void Start()
    {
        eh = EventHandler.Instance;
    }
    public PowerUp GetPowerUp()
    {
        int i = Random.Range(0, 3);

        var chosen = Instantiate(PrototypePowerUps[i]);

        return chosen.GetComponent<PowerUp>();
    }

    public void AdjustList()
    {

    }

    public void SelectPowerUp(PowerUp selected_Power_up)
    {

    }

    public void ApplyPowerUp()
    {

    }




}
