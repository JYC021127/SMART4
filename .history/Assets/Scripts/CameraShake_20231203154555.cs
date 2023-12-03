using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform cameraTransform; // Reference to your camera transform
    public float shakeDuration = 0.3f; // Duration of the shake
    public float shakeMagnitude = 0.5f; // Intensity of the shake

    private Vector3 originalPos; // Original position of the camera

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = GetComponent<Transform>();
        }
    }

    public void Shake()
    {
        StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        float elapsedTime = 0.0f;
        originalPos = cameraTransform.localPosition;
        while (elapsedTime < shakeDuration)
        {
            Vector3 newPos = originalPos + Random.insideUnitSphere * shakeMagnitude;

            cameraTransform.localPosition = newPos;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        cameraTransform.localPosition = originalPos;
    }
}