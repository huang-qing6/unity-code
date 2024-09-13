using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Menu : MonoBehaviour
{
    public GameObject newGameButton;

    private void OnEnable()
    {
        //��ʼ��ѡ��ť
        EventSystem.current.SetSelectedGameObject(newGameButton);
    }

    public void ExitGame()
    {
        Debug.Log("111");
        Application.Quit();
    }
}
