using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour
{

    // player'ın pozisyonunu takip eden script
    public Transform player;
    public Vector3 offset;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    //y'yi player y'sinden 16

    //rotate the camera to the player
    public float rotateSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.position + offset;
        transform.rotation = player.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player.position + offset, ref velocity, smoothTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, player.rotation, rotateSpeed * Time.deltaTime);
    }
}
