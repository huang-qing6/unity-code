using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Character : MonoBehaviour,ISaveable
{
    [Header("监听")]
    public VoidEventSO newGameEvent;

    [Header("属性")]
    public float MaxHealth;
    public float CurrentHealth;

    public float maxPower;
    public float currentPower;
    public float powerRecoverSpeed;
    [Header("受伤无敌")]
    public float invulnerableDuration;
    [HideInInspector]public float invulnerableCounter;
    public bool invulnerable;

    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent<Character> OnHealthChange;  
    public UnityEvent OnDie;

    private void NewGame()
    {
        CurrentHealth = MaxHealth;
        currentPower = maxPower;
        OnHealthChange?.Invoke(this);
    }

    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;

        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;

        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    private void Update()
    {
        if(invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if(invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }

        if(currentPower < maxPower)
        {
            currentPower += Time.deltaTime * powerRecoverSpeed;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            if (CurrentHealth > 0)
            {
                CurrentHealth = 0;
                OnHealthChange?.Invoke(this);
                OnDie.Invoke();
            }

        }
    }

    public void TakeDamage(Attack attacker)
    {
        if (invulnerable) return;

        if (CurrentHealth - attacker.damage > 0)
        {
            CurrentHealth -= attacker.damage;
            TriggerInvulnerable();
            //执行受伤
            OnTakeDamage?.Invoke(attacker.transform);

        }
        else
        {
            CurrentHealth = 0;
            OnDie?.Invoke();
        }
        
        
        OnHealthChange?.Invoke(this);
    }


    private void TriggerInvulnerable()
    {
        if (!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }

    public void OnSlide(int cost)
    {
        currentPower -= cost;
        OnHealthChange?.Invoke(this);
    }

    public DataDefination GetDataID()
    {
        return  GetComponent<DataDefination>(); 
    }

    /// <summary>
    /// 哈希的方式储存玩家位置、血量以及能量
    /// </summary>
    /// <param name="data"></param>
    public void GetSaveData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            data.characterPosDict[GetDataID().ID]=new SerializeVector3(transform.position);
            data.floatSaveData[GetDataID().ID += "health"] = this.CurrentHealth;
            data.floatSaveData[GetDataID().ID += "power"] = this.CurrentHealth;
        }
        else
        {
            data.characterPosDict.Add(GetDataID().ID, new SerializeVector3(transform.position));
            data.floatSaveData.Add(GetDataID().ID += "health", this.CurrentHealth);
            data.floatSaveData.Add(GetDataID().ID += "power", this.CurrentHealth);
        }
    }

    public void LoadData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        { 
            this.CurrentHealth = data.floatSaveData[GetDataID().ID += "health"];
            this.currentPower = data.floatSaveData[GetDataID().ID += "power"];
            transform.position = data.characterPosDict[GetDataID().ID].ToVector3();

            //通知更新
            OnHealthChange?.Invoke(this);
        }
    }
}
