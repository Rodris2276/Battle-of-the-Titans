using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DamageTester : MonoBehaviour
{
    public AttributesManager playerAtm;
    public AttributesManager player1Atm;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F11))
        {
            playerAtm.DealDamage(player1Atm.gameObject);
        }

        if(Input.GetKeyDown(KeyCode.F12))
        {
            player1Atm.DealDamage(playerAtm.gameObject);
        }
    }
}
