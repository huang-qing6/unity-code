using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDefination : MonoBehaviour
{
    public PresistentType presistentType;   

    public string ID;

    private void OnValidate()
    {
        if(presistentType == PresistentType.ReadWrite)
        {
            if (ID == string.Empty)
                ID = System.Guid.NewGuid().ToString();//Ψһ��ʶ��
        }
        else
        {
            ID = string.Empty;  
        }

    }
}
