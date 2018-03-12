using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : SingleTonManager<EffectScript> //EffectManager에 생성된 Effect들을 조건에 맞게 Active 시킨다, Active된 Effect들을 발동시킨다.
{
    public bool isPropertyFlame;
    public bool isPropertyIce;

	void Update () //불/물속성에 따라서 이펙트가 달라진다.
    {
		
	}
}

