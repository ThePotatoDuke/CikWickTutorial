using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;
    private float _shakeTimer;
    private float _shakeTimerTotal;
    private float _startingIntensity;

    void Awake()
    {
        Instance = this;

        _cinemachineBasicMultiChannelPerlin = GetComponent<CinemachineBasicMultiChannelPerlin>();

        if (_cinemachineBasicMultiChannelPerlin == null)
        {
            Debug.LogError(
                "CinemachineBasicMultiChannelPerlin not found! Make sure this script is on the virtual camera with a Noise component."
            );
        }
    }

    private IEnumerator CameraShakeCoroutine(float intensity, float time, float delay)
    {
        yield return new WaitForSeconds(delay);
        _cinemachineBasicMultiChannelPerlin.AmplitudeGain = intensity;
        _shakeTimer = time;
        _shakeTimerTotal = time;
        _startingIntensity = intensity;
    }

    public void ShakeCamera(float intensity, float time, float delay = 0f)
    {
        StartCoroutine(CameraShakeCoroutine(intensity, time, delay));
    }

    private void Update()
    {
        if (_cinemachineBasicMultiChannelPerlin == null)
            return;

        if (_shakeTimer > 0f)
        {
            _shakeTimer -= Time.deltaTime;
            float lerpAmount = 1 - (_shakeTimer / _shakeTimerTotal);
            _cinemachineBasicMultiChannelPerlin.AmplitudeGain = Mathf.Lerp(
                _startingIntensity,
                0f,
                lerpAmount
            );
        }
        else if (_cinemachineBasicMultiChannelPerlin.AmplitudeGain != 0f)
        {
            _cinemachineBasicMultiChannelPerlin.AmplitudeGain = 0f;
        }
    }
}
