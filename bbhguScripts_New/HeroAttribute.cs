using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttribute : MonoBehaviour //singleTon으로 넘기면 안됀다. 추상 클래스로 넘기자. -> HeroAttribute로 변경
{
    //전체 관리용 속성 스크립트
    //히어로와 마물로 나누어서 만들 수 있음
    //우선 지금은 히어로와 마물을 포함한 속성으로 제작
    //수정 - 히어로 객체만의 속성 스크립트 만들기
    //수정 - 마물 객체만의 속성 스크립트 만들기
    //해야할 것 - 데미지를 Ontrigger 이 부분에서 넘겨줄 것인가 아니면 다른 방법으로 넘겨줄 것인가(Hit가 됬다는 것을 알려야 한다.
    //            데미지를 받았다는 것을 넘기고 난 후에 생명력 또는 마력을 깍는다.
    //          - curHP,MP 관리는 최상위 오브젝트 객체에서 관리할 것인가, 개별 객체로 관리할 것인가.
    //          - 우선 이젤리아 개인 객체의 속성 스크립트 만들어서 구현해 볼 것

    public struct Hero //Hero들을 Struct로 정의했다.
    {
        private double heroMaxHP;
        private double heroDamage;
        private float heroMoveForce;
        private float heroJumpForce;


        private float heroCurShield;
        private float heroMaxShield;

        private float heroRecShield;
        private float heroRecEnergy;

        private float heroCurEnergy;
        private float heroMaxEnergy;

        private float heroDefensivePower;

        private float heroHorizontalAcceleration;
        private float heroVerticalAcceleration;

        public Hero(Heros hero, out double maxHP, out double damage, out float moveForce, out float jumpForce, out float maxShield, out float maxEnergy, out float recShield, out float recEnergy, out float defensivePower, out float horizontalAcceleration, out float verticalAcceleration)
        {
            //초기화
            heroMaxHP = 0.0;
            heroDamage = 0.0;
            heroMoveForce = 0.0f;
            heroJumpForce = 0.0f;

            heroCurShield = 0.0f;
            heroMaxShield = 0.0f;
            heroRecShield = 0.0f;
            heroRecEnergy = 0.0f;
            heroCurEnergy = 0.0f;
            heroMaxEnergy = 0.0f;
            heroDefensivePower = 0.0f;
            heroHorizontalAcceleration = 0.0f;
            heroVerticalAcceleration = 0.0f;

            maxHP = 0.0;
            damage = 0.0;
            moveForce = 0.0f;
            jumpForce = 0.0f;
            maxShield = 0.0f;
            recShield = 0.0f;
            maxEnergy = 0.0f;
            recEnergy = 0.0f;
            defensivePower = 0.0f;
            horizontalAcceleration = 0.0f;
            verticalAcceleration = 0.0f;

            //영웅 탐색, 나중에 영웅이 많아지면 switch로 만들 것이다.
            if(Equals(hero, Heros.Easelia)) // 방어력, 시전속도 추가할 것
            {
                heroMaxHP = 200.0;
                heroMaxShield = 100.0f;
                heroMaxEnergy = 50.0f;
                heroRecShield = 2.5f;
                heroRecEnergy = 2.5f;
                heroDefensivePower = 5.0f;

                heroHorizontalAcceleration = 0.0f;
                heroVerticalAcceleration = 6.5f * 0.1f;

                heroDamage = 8.0;
                heroMoveForce = 1.4f * 0.1f;
                heroJumpForce = 3.3f;

                EaseliaAttribute(out maxHP, out damage, out moveForce, out jumpForce, out maxShield, out maxEnergy, out recShield, out recEnergy, out defensivePower, out horizontalAcceleration, out verticalAcceleration);
            }
        }

        private void EaseliaAttribute(out double maxHP, out double damage, out float moveForce, out float jumpForce, out float maxShield, out float maxEnergy, out float recShield, out float recEnergy, out float defensivePower, out float horizontalAcceleration, out float verticalAcceleration)
        {
            maxHP = heroMaxHP;
            damage = heroDamage;
            moveForce = heroMoveForce;
            jumpForce = heroJumpForce;

            maxShield = heroMaxShield;
            maxEnergy = heroMaxEnergy;

            recShield = heroRecShield;
            recEnergy = heroRecEnergy;

            defensivePower = heroDefensivePower;

            horizontalAcceleration = heroHorizontalAcceleration;
            verticalAcceleration = heroVerticalAcceleration;
        }
    }
    private CalculateAttribute cal;

    private HeroAttribute heroAttribute;
    private Hero hero;

    public delegate double Health(double curHP, double maxHP, double hitDamage); // HP 델리게이트
    public event Health Hero_HP;

    public Heros heros;

    public double curHP;
    public double maxHP;

    public float curShield;
    public float maxShield;

    public float recShield;
    public float recEnergy;

    public float curEnergy;
    public float maxEnergy;

    public float defensivePower;

    public float horizontalAcceleration;
    public float verticalAcceleration;

    public double damage; //gumAttribute로 넘길 예정

    public float moveForce;
    public float jumpForce;
    //public double armer; -> 추가 예정
    //public double castTime; -> 추가 예정

    //방어력
    //공격력
    //스킬 데미지
    //이동 속도
    //공격 속도
    //활성화 속도

    /* 각 객체에 변수들 전달 */
    public void Awake()
    {
      hero = new Hero(heros, out maxHP, out damage, out moveForce, out jumpForce, out maxShield, out maxEnergy, out recShield, out recEnergy, out defensivePower, out horizontalAcceleration, out verticalAcceleration);
      curHP = maxHP;
      curShield = maxShield;
      curEnergy = maxEnergy;

      heroAttribute = GetComponent<HeroAttribute>();
      cal = new CalculateAttribute(heroAttribute);
    }

    public void LoseHealth(double hitDamage) //추상클래스를 상속받는 클래스로 넘긴다.
    {
        //델리게이트 함수로 넘겨주기
        curHP = Hero_HP(curHP, maxHP, hitDamage); // 현재 공격한 객체의 CurHP와 maxHP를 넘겨주었다.

        if(curHP < 0.0f)
        {
            Debug.Log("die");
            //this.gameObject.SetActive(false);
        }
    }

    public enum Heros
    {
        Easelia,
        Oratus,
        Kain_Richter
    }
}

abstract class AbsCalculateAttribute
{
    //공격,HP 등의 모든 수치 계산을 하는 추상클래스
    //이곳에서 이제 앞으로 모두 속성들을 계산할 추상메소들을 추상화한다.
    //우선적으로 HP가 감소하는 것을 추상화할것
    public abstract double HealthDown(double curHP, double maxHP, double hitDamage);
}

class CalculateAttribute : AbsCalculateAttribute
{
    HeroAttribute hero;

    public CalculateAttribute(HeroAttribute hero)
    {
        this.hero = hero;

        hero.Hero_HP += HealthDown;
    }
    //이곳에서 이제 앞으로 모두 속성들을 계산할 것이다.
    //우선적으로 HP가 감소하는 것을 구현할 것
    public override double HealthDown(double curHP, double maxHP, double hitDamage) //Damage도 넘겨주기
    {
        curHP -= hitDamage;
        Debug.Log(curHP + "  " + maxHP);

        return curHP;
    }
}



/*
//HP, MP 회복 관련 추상 클래스
abstract class RescueMethod
{
    //회복/푸른 구슬/고유 능력/고유 기술/주문서 등을 통해 회복
    //각 아이템 및 고유 능력과 기술로 회복을 어떻게 해야할지 추상화할 메소드들의 모임
}*/


