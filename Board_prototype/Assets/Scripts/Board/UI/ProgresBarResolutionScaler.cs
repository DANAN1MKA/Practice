using UnityEngine;
using Zenject;

public class ProgresBarResolutionScaler : MonoBehaviour
{
    [Inject] private BoardProperties config;
    private RectTransform thisTransform;
    private Camera mainCamera;

    private void Awake()
    {
        thisTransform = GetComponent<RectTransform>();
        mainCamera = GetComponentInParent<Camera>();
    }
    private void Start()
    {
        Vector2 newPosition = new Vector2(0, config.boardPositionFromResolution.y + config.height * config.scale);
        thisTransform.position = RectTransformUtility.WorldToScreenPoint(mainCamera, newPosition);
    }
}
