using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;

    void Start() 
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        virtualCamera.LookAt = GameManager.instance.player.transform;
        virtualCamera.Follow = GameManager.instance.player.transform;
    }
}
