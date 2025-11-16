using System.Collections;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Vector3 offset;
    public float smoothTime = 0.3f;
    public float rotateSpeed = 10f;

    private Vector3 velocity = Vector3.zero;
    public Transform player;

    void Start()
    {

        //courotine ile player'ın bulunmasını bekle
        StartCoroutine(WaitForPlayer());

        Debug.Log("Player found: " + player);
        if (player != null)
        {
            transform.position = player.position + offset;
            transform.rotation = player.rotation;
        }
    }
    //courotine ile player'ın bulunmasını bekle
    IEnumerator WaitForPlayer()
    {
        yield return new WaitForSeconds(1f);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player.position + offset, ref velocity, smoothTime);

        float rotationLerpSpeed = rotateSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, player.rotation, rotationLerpSpeed);
    }
}
