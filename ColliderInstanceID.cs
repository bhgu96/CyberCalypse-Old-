using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderInstanceID : SingleTonManager<ColliderInstanceID>
{
    public List<HeroAttribute> collides;

    private Dictionary<int, HeroAttribute> colliderInstance;
    private int[] poolID;
    private int getCollideID;

    private new void Awake()
    {
        base.Awake();

        colliderInstance = new Dictionary<int, HeroAttribute>();
        poolID = new int[collides.Count];

        GetInstance();
    }

    private void GetInstance()
    {
        for (int i = 0; i < collides.Count; i++)
        {
            poolID[i] = collides[i].gameObject.GetInstanceID();
            colliderInstance.Add(poolID[i], collides[i]);
        }
    }

    public double CheckCollider(Collider2D col)
    {
        getCollideID = col.transform.parent.gameObject.GetInstanceID();

        for (int i = 0; i <= collides.Count; i++)
        {
            if (colliderInstance.ContainsKey(getCollideID))
            {
                return colliderInstance[col.transform.parent.gameObject.GetInstanceID()].damage;
            }
        }

        return 0;
    }

}
