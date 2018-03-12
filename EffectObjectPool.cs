using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//오브젝트 풀링(초기화, pop,push) 하는 스크립트
public class EffectObjectPool : SingleTonManager<EffectObjectPool>
{
    public Dictionary<int, Queue<ObjectInstance>> objectPoolDictionary = new Dictionary<int, Queue<ObjectInstance>>();
    public int objectFireCount;

    float destroyTime;

    public void CreateActingEffectPool(GameObject prefab, int poolSize)
    {
        int poolKey = prefab.GetInstanceID();

        GameObject poolHolder = new GameObject(prefab.name + " pool"); //FlameEffect 부모로 EffectObjectPool로 생성한다.
        poolHolder.transform.parent = transform;

        if (!objectPoolDictionary.ContainsKey(poolKey))
        {
            objectPoolDictionary.Add(poolKey, new Queue<ObjectInstance>());

            for (int i = 0 ; i < poolSize; i++)
            {
                ObjectInstance newObject = new ObjectInstance(Instantiate(prefab) as GameObject);
                objectPoolDictionary[poolKey].Enqueue(newObject);
                newObject.SetParent(poolHolder.transform,poolSize);
            }
        }

        try
        {
            Debug.Log("ActingEffect Initialize");
        }
        catch(System.Exception e)
        {
            Debug.Log(e);
        }
    }

    public void CreateActedEffectPool()
    {
        try
        {
            Debug.Log("ActedEffect Initialize");
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    public void AttackEffectPooling(bool isProperty, GameObject prefab, Vector3 position, Vector3 scale, int objectPoolSize)
    {
        objectFireCount++;

        if (isProperty == EffectScript.instance.isPropertyFlame)
        {
            int poolKey = prefab.GetInstanceID();

            if (objectPoolDictionary.ContainsKey(poolKey))
            {
                if (objectFireCount == objectPoolSize)
                {
                    objectFireCount = 0;
                    EffectScript.instance.isPropertyFlame = false;
                }

                Debug.Log("Effect Acting");
                ObjectInstance objectToReuse = objectPoolDictionary[poolKey].Dequeue();
                objectPoolDictionary[poolKey].Enqueue(objectToReuse);

                objectToReuse.ReuseObject(position, scale); 
            }
        }
        else if (isProperty == EffectScript.instance.isPropertyIce)
        {

        }
    }

    public void WalkEffectPooling(bool isProperty, GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (isProperty == EffectScript.instance.isPropertyFlame)
        {

        }
        else if (isProperty == EffectScript.instance.isPropertyIce)
        {

        }
    }

    public void TeleportEffectPooling(bool isProperty, GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (isProperty == EffectScript.instance.isPropertyFlame)
        {

        }
        else if (isProperty == EffectScript.instance.isPropertyIce)
        {

        }
    }

    public void JumpEffectPooling(bool isProperty, GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (isProperty == EffectScript.instance.isPropertyFlame)
        {

        }
        else if (isProperty == EffectScript.instance.isPropertyIce)
        {

        }
    }

    // ActedEffectInterface
    public void HittedEffectPooling(bool isProperty)
    {
        if (isProperty == EffectScript.instance.isPropertyFlame)
        {

        }
        else if (isProperty == EffectScript.instance.isPropertyIce)
        {

        }
    }

    /* test 재사용 */
    public virtual void OnObjectReuse()
    {

    }

    /* test setActive(False)*/
    public void Destroy(GameObject effectobject)
    {
        effectobject.SetActive(false);
    }

    public void StartDestroy(GameObject effectobject)
    {
        StartCoroutine(DestroyEffectObject(effectobject));
    }

    IEnumerator DestroyEffectObject(GameObject effectobject)
    {
        while (destroyTime < 0.5f)
        {
            yield return null;
            destroyTime += Time.deltaTime;
        }

        if (destroyTime > 0.5f)
        {
            destroyTime = 0.0f;
            EffectObjectPool.instance.Destroy(effectobject);
            yield break;
        }
    }
}

[System.Serializable]
public class ObjectInstance
{
    public int objectPoolSize;
    public int objectFireCount;

    GameObject gameObject;
    Transform transform;

    bool hasPoolObject;
    EffectObjectPool effectobjectpool;

    public ObjectInstance(GameObject gameobjectInstance)
    {
        gameObject = gameobjectInstance;
        transform = gameObject.transform;
        gameObject.SetActive(false);

        if(gameObject.GetComponent<EffectObjectPool>())
        {
            hasPoolObject = true;
            effectobjectpool = gameObject.GetComponent<EffectObjectPool>();
        }
    }

    public void ReuseObject(Vector3 position, Vector3 scale)
    {
        objectFireCount++;

        if (hasPoolObject)
        {
            EffectObjectPool.instance.OnObjectReuse();
        }

        if (objectFireCount > objectPoolSize)
        {
            objectFireCount = 0;
            EffectScript.instance.isPropertyFlame = false;
        }

        gameObject.SetActive(true);
        gameObject.transform.position = position;
        gameObject.transform.localScale = scale;
        EffectObjectPool.instance.StartDestroy(this.gameObject);
    }

    public void SetParent(Transform parent, int poolSize)
    {
        objectPoolSize = poolSize;
        transform.parent = parent;
    }
}
