using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeController : MonoBehaviour
{
    public Slider slider;
    public string parameter;
    
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private float multiplier;

    public void SliderChange(float value) => mixer.SetFloat(parameter, Mathf.Log10(value) * multiplier);
    

    public void LoadSlider(float _value)
    {
        if(_value >= 0.001f)
            slider.value = _value;
    }
}
