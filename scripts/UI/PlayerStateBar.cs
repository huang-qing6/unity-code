using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateBar : MonoBehaviour
{
    private Character currentCharacter;

    public Image healthImage;
    public Image helatdownImage;
    public Image powerImage;

    private bool isRecovering;
    private void Update()
    {
        if(healthImage.fillAmount < helatdownImage.fillAmount)
        {
            helatdownImage.fillAmount -= Time.deltaTime * (float)0.25;
        }

        if(isRecovering )
        {
            float percentage = currentCharacter.currentPower / currentCharacter.maxPower;
            powerImage.fillAmount = percentage; 

            if(percentage >= 1) {
            isRecovering = false;
            return; }
        }
    }
    /// <summary>
    /// µ÷ÕûÑªÁ¿
    /// </summary>
    /// <param name="percentage"></param>
    public void OnHealthChange(float percentage)
    {
        healthImage.fillAmount = percentage;    
    }

    public void OnPowerChange(Character character)
    {
        isRecovering = true;    
        currentCharacter = character;   
    }

}
