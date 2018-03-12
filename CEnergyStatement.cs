using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnergyStatement : SingleTonManager<CEnergyStatement> // 임시용
{
    public GameObject energyBar;
    public GameObject target;
    public SpriteRenderer sprite;
    public static float energy;

    public ArcanePhsics arcane;

    SpriteRenderer spritePivot;

    public GameObject parentTarget;

    private new void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        energyBar.SetActive(false);
        InputManager.instance.PlayerVMove += VMoveEnergyState;
    }

    public void VMoveEnergyState(float inputValue)
    {
        energy = this.sprite.size.x;

        if(parentTarget.transform.localScale.x < 0.0f)
        {
            this.transform.localPosition = new Vector3(+2, 3, 0);
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(parentTarget.transform.localScale.x > 0.0f)
        {
            this.transform.localPosition = new Vector3(-2, 3, 0);
            this.transform.localScale = new Vector3(+1, 1, 1);
        }


        if (this.sprite.size.x > 4f)
        {
            this.sprite.size = new Vector2(4, this.sprite.size.y);
            energyBar.SetActive(false);
            return;
        }
        else if(this.sprite.size.x < 0)
        {
            this.sprite.size = new Vector2(0, this.sprite.size.y);
            return;
        }


        if(!arcane.isGrounded &&(inputValue == 1 || inputValue == -1))
        {
            energyBar.SetActive(true);
            this.sprite.size = new Vector2(this.sprite.size.x - 0.1f, this.sprite.size.y);
        }
        else if (inputValue == 0)
        {
            this.sprite.size = new Vector2(this.sprite.size.x + 0.05f, this.sprite.size.y);
        }
        else if(arcane.isGrounded && (inputValue == 1 || inputValue == -1))
        {
            energyBar.SetActive(false);
        }
    }
}
