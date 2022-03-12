using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Destroy spawn point");
        if (collision.CompareTag("SpawnPoint"))
            Destroy(collision.gameObject);
        /*if (collision.CompareTag("RoomCenter"))
            Destroy(collision.transform.parent.gameObject);*/
    }
}
