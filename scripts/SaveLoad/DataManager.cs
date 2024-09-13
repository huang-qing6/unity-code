using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json;
using System.IO;


[DefaultExecutionOrder(-100)]//��֤������ִ�У���ԽСԽ��ִ��

public class DataManager : MonoBehaviour
{
    
    public static DataManager instance;

    [Header("�¼�����")]
    public VoidEventSO saveGameEvent;

    public VoidEventSO loadDataEvent;

    private List<ISaveable> saveableList = new List<ISaveable>();

    private Data saveData;

    private string jsonFolder;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else
        {
            Destroy(this.gameObject);
        }

        saveData = new Data();

        jsonFolder = Application.persistentDataPath + "/SAVE DATA/"; //��Ӧϵͳ�Զ�����·��

        ReadSaveData();
    }

    private void OnEnable()
    {
        saveGameEvent.OnEventRaised += Save;
        loadDataEvent.OnEventRaised += Load;
    }

    private void OnDisable()
    {
        saveGameEvent.OnEventRaised -= Save;
        loadDataEvent.OnEventRaised -= Load;
    }

    private void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            Load();
        }
    }
    public void RegisterSaveData(ISaveable saveable)
    {
        if(!saveableList.Contains(saveable)) 
        {
            saveableList.Add(saveable);
        }
    }


    public void UnregisterSaveData(ISaveable saveable)
    {
            saveableList.Remove(saveable);
    }

    public void Save()
    {
        foreach(var saveable in saveableList)
        {
            saveable.GetSaveData(saveData); 
        }

        var resultPath = jsonFolder + "data.sav";  //�������ݣ���׺����ν������ʶ��

        var jsonData = JsonConvert.SerializeObject(saveData);

        if (!File.Exists(resultPath))
        {
            Directory.CreateDirectory(jsonFolder);
        }

        File.WriteAllText(resultPath, jsonData);

        /**foreach(var item in saveData.characterPosDict)
        {
            Debug.Log(item.Key + "  " + item.Value);
        }**/
    }

    public void Load()
    {

        foreach (var saveable in saveableList)
        {
            saveable.LoadData(saveData);
        }
    }

    private void ReadSaveData()
    {
        var resultPath = jsonFolder + "data.sav";  //�������ݣ���׺����ν������ʶ��

        if (File.Exists(resultPath))
        {
            var stringData = File.ReadAllText(resultPath);

            var jsonData = JsonConvert.DeserializeObject<Data>(stringData); //�����л�

            saveData = jsonData;    
        }

    }
}
