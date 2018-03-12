using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class EffectManager : SingleTonManager<EffectManager> //처음 모든 Effect들의 Object들을 초기화시킨다(SetActive(False)
{
    public GameObject prefab;

    private void Start()
    {
        AbstEffectFactory abstEffectFactory = new ManagementableEffect();
        abstEffectFactory.InitAllEffect();
        ManagementableEffect.instance.Init();
    }

    private void Update()
    {
        
    }
}

//1.ActingEffect 먼저 초기화 시킨다.
//2.ActedEffect를 초기화 시킨다.
public abstract class AbstEffectFactory
{ 
    //초기화 할 템플릿 메소드
    public void InitAllEffect()
    {
        //1. Acting
        ActingEffect();
        //2. Acted
        ActedEffect();
    }

    abstract protected void ActingEffect();
    abstract protected void ActedEffect();
}

public interface IActingEffectKinds
{
    void AttackEffect(bool isProperty, GameObject prefab, Vector3 position, Vector3 scale);
    void WalkEffect(bool isProperty, GameObject prefab, Vector3 position, Vector3 scale);
    void TeleportEffect(bool isProperty, GameObject prefab, Vector3 position, Vector3 scale);
    void JumpEffect(bool isProperty, GameObject prefab, Vector3 position, Vector3 scale);
}

public interface IActedEffectKinds
{
    void HittedEffect(bool isProperty);
}

public class ManagementableEffect : AbstEffectFactory , IActingEffectKinds, IActedEffectKinds
{
    public int poolSize = 3;
    
    private static ManagementableEffect _instance;
    public static ManagementableEffect instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("There active Instance : " + typeof(ManagementableEffect));
                _instance = new ManagementableEffect();

                if (_instance == null)
                {
                    Debug.Log("There does not active Instance " + typeof(ManagementableEffect));
                }
            }
            return _instance;
        }
    }

    /* 메소드 */
    public void Init()
    {
        try
        {
            Debug.Log("Effect Object Init Finish Complete");
        }
        catch(System.Exception e)
        {
            Debug.Log("Initation has error" + e);
        }
    }

    protected override void ActingEffect() //모든 ActingEffect 오브젝트들을 생성하여 SetActive(False) 시킨다.
    {
        EffectObjectPool.instance.CreateActingEffectPool(EffectManager.instance.prefab, poolSize);
    }

    protected override void ActedEffect() //모든 ActedEffect 오브젝트들을 생성하여 SetActive(False) 시킨다.
    {
        EffectObjectPool.instance.CreateActedEffectPool();
    }

    /* 이하 메소드들은 오브젝트 풀링을 구현할 메소드들 집합소. */
    public void AttackEffect(bool isProperty, GameObject prefab, Vector3 position, Vector3 scale)
    {
        if (isProperty == EffectScript.instance.isPropertyFlame)
        {
            EffectObjectPool.instance.AttackEffectPooling(isProperty,prefab,position,scale,ManagementableEffect.instance.poolSize);
        }
        else if (isProperty == EffectScript.instance.isPropertyIce)
        {

        }
    }

    public void WalkEffect(bool isProperty, GameObject prefab, Vector3 position, Vector3 scale)
    {
        if (isProperty == EffectScript.instance.isPropertyFlame)
        {

        }
        else if (isProperty == EffectScript.instance.isPropertyIce)
        {

        }
    }

    public void TeleportEffect(bool isProperty, GameObject prefab, Vector3 position, Vector3 scale)
    {
        if (isProperty == EffectScript.instance.isPropertyFlame)
        {

        }
        else if (isProperty == EffectScript.instance.isPropertyIce)
        {

        }
    }

    public void JumpEffect(bool isProperty, GameObject prefab, Vector3 position, Vector3 scale)
    {
        if (isProperty == EffectScript.instance.isPropertyFlame)
        {

        }
        else if (isProperty == EffectScript.instance.isPropertyIce)
        {

        }
    }

    // ActedEffectInterface
    public void HittedEffect(bool isProperty)
    {
        if (isProperty == EffectScript.instance.isPropertyFlame)
        {

        }
        else if (isProperty == EffectScript.instance.isPropertyIce)
        {

        }
    }
}