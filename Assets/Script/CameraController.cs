using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    public Transform target;//the target which is the player
    public Tilemap theMap;//the Tilemap object
    private Vector3 bottomLeftLimit;//the camera upper limit
    private Vector3 topRightLimit;//the camera bottom limit
    private float halfHeight;//a float for halfHeight
    private float halfWidth;//a float for halfWidth
    public int musicToPlay;//the music to play in this scene
    private bool musicStarted;//to know of the music is already started
    public Material TransitionMaterial;//for fade manger
    // Use this for initialization
    void OnRenderImage(RenderTexture src, RenderTexture dst)//for fade manger
    {
        if (TransitionMaterial != null)
            Graphics.Blit(src, dst, TransitionMaterial);
    }
    void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;//find the player
        halfHeight = Camera.main.orthographicSize;//get the halfHeight
        halfWidth = halfHeight * Camera.main.aspect;//get the halfWidth
        theMap.CompressBounds();//get the bounds from the tilemap
        bottomLeftLimit = theMap.localBounds.min + new Vector3(halfWidth, halfHeight, 0f);//set the bottomLeftLimit
        topRightLimit = theMap.localBounds.max + new Vector3(-halfWidth, -halfHeight, 0f);//set the topRightLimit
        PlayerController.instance.SetBounds(theMap.localBounds.min, theMap.localBounds.max);//set the bounds for the camera
    }
    // LateUpdate is called once per frame after Update
    void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);//the player is at the center of the camera
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);
        //keep the camera inside the bounds
        if (!musicStarted)//if no music then
        {
            musicStarted = true;//only once
            AudioManager.instance.PlayBGM(musicToPlay);//play music
        }

    }
}
