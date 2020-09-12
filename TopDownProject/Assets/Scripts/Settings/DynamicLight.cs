using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(HDAdditionalLightData))]
public class DynamicLight : MonoBehaviour
{

    [SerializeField] private BoolParameter shadowsSetting;

    private new Light light;
    private HDAdditionalLightData lightData;

    private void Awake()
    {
        light = this.GetComponent<Light>();
        lightData = this.GetComponent<HDAdditionalLightData>();
    }

    private void Start() => UpdateSettings();

    private void OnEnable() => BaseParameter.OnSettingsChanged += UpdateSettings;

    private void OnDisable() => BaseParameter.OnSettingsChanged -= UpdateSettings;

    private void UpdateSettings()
    {
        lightData.EnableShadows(shadowsSetting.GetValue());
    }

}