using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private Slider slider;
    private float targetValue;
    private float fillSpeed = 1f;

    private void Awake() => slider = GetComponent<Slider>();

    public void Update()
    {
        if (slider.value < targetValue)
            slider.value += fillSpeed * Time.deltaTime;
    }

    public void IncrementValue(float val)
    {
        if (slider == null) return;

       // targetValue = slider.value + val;
        targetValue = val;
    }

    public void SetValue(float val) => slider.value = val;
}
