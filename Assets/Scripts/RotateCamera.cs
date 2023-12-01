using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateCamera : MonoBehaviour
{
    public Transform centerPoint; // The center point around which the camera will rotate
    public Slider rotationSlider; // Reference to the UI slider controlling the rotation

    private float previousSliderValue = 0f; // The previous value of the slider

    void Update()
    {
        // Check if the slider's value has changed
        if (rotationSlider.value != previousSliderValue)
        {
            float delta = rotationSlider.value - previousSliderValue;
            RotateCameraSlider(delta);
        }

        // Store the current slider value for comparison in the next frame
        previousSliderValue = rotationSlider.value;
    }

    private void RotateCameraSlider(float delta)
    {
        // Calculate rotation based on the change in slider value
        float rotationAmount = delta * 10000f; 

        // Apply rotation to the camera around the center point
        transform.RotateAround(centerPoint.position, Vector3.down, rotationAmount * Time.deltaTime);
        transform.LookAt(centerPoint); // Look at the center point
    }
}
