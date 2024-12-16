using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Igrač koga kamera prati
    public Vector3 offset; // Pomak kamere u odnosu na igrača
    public float smoothSpeed = 0.125f; // Brzina glatkog praćenja

    void LateUpdate()
    {
        // Ciljna pozicija kamere
        Vector3 targetPosition = player.position + offset;

        // Glatko pomeranje kamere
        Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        transform.position = smoothPosition;
    }
}
