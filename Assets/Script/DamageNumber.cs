using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{

    public TextMeshProUGUI damageText;

    public float lifetime = 1f;//how many second it will be on the screen
    public float moveSpeed = 1f;//how fast it will move

    public float placementJitter = 0.5f;//make the text appear on different places 

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifetime);//destroy it after lifetime
        transform.position += new Vector3(0f, moveSpeed * Time.deltaTime, 0f);//move it up
    }

    public void SetDamage(int damageAmount)//make a text appear
    {
        damageText.text = damageAmount.ToString();//its string
        transform.position += new Vector3(Random.Range(-placementJitter, placementJitter), Random.Range(-placementJitter, placementJitter), 0f);//appear on different places each time
    }
}
