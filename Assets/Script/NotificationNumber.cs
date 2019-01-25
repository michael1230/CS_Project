using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationNumber : MonoBehaviour
{

    public TextMeshProUGUI notificationText;

    public float lifetime = 1f;//how many second it will be on the screen
    public float moveSpeed = 1f;//how fast it will move

    public float placement = 0.5f;//make the text appear on the left of the player

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

    public void SetNotification(int NotificationAmount=0, string effect="")//a method to make the text appear
    {
        if(effect=="")//no effect, regular color
        {
            notificationText.text = NotificationAmount.ToString();//its string
        }
        else if (effect == "Health")//green for Health effect
        {
            notificationText.text = NotificationAmount.ToString();//its string
            notificationText.color= new Color32(21, 212, 61, 255);
        }
        else if (effect == "Mana")//blue for Mana effect
        {
            notificationText.text = NotificationAmount.ToString();//its string
            notificationText.color = new Color32(21, 61, 212, 255);
        }
        else if (effect == "Special")//orange for Special effect
        {
            notificationText.text = NotificationAmount.ToString();//its string
            notificationText.color = new Color32(212, 115, 21, 255);
        }
        else if (effect == "Elixer")//violet  for Elixer effect
        {
            notificationText.text = effect;//its string
            notificationText.color = new Color32(212, 21, 128, 255);
        }
        transform.position += new Vector3(-placement,0F, 0f);//appear on different places each time

    }
}
