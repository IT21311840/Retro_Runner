using Cinemachine;
using UnityEngine;

public class CameraShaker : MonoBehaviourSingleton<CameraShaker>
{
    [SerializeField] private CinemachineVirtualCamera vCam;
    private CinemachineBasicMultiChannelPerlin basicMultiChannelPerlin;
    private float shakeTime;
    
    protected override void Awake()
    {
        base.Awake();
            
        if (vCam == null)
            vCam = GetComponent<CinemachineVirtualCamera>()
                ?? FindObjectOfType<CinemachineVirtualCamera>();
        
        if (vCam == null)
        {
            Debug.LogWarning("CinemachineVirtualCamera not found in CameraShaker.");
        }
        else
        {
            basicMultiChannelPerlin = vCam
                .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    void Update()
    {
        if (shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;

            if (shakeTime <= 0 && basicMultiChannelPerlin != null)
                basicMultiChannelPerlin.m_AmplitudeGain = 0f;
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        if (basicMultiChannelPerlin == null)
        {
            Debug.LogWarning("Cannot shake camera—Perlin component missing.");
            return;
        }

        basicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTime = time;
    }
}
