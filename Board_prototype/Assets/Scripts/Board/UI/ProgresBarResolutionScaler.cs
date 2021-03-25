using UnityEngine;
using Zenject;

public class ProgresBarResolutionScaler : MonoBehaviour
{
    [Inject] private BoardProperties config;
    private RectTransform thisTransform;
    private void Awake()
    {
        thisTransform = GetComponent<RectTransform>();
    }
    private void Start()
    {
        Vector2 newPosition = new Vector2(thisTransform.position.x,
                                          thisTransform.position.y * config.scale);
        thisTransform.position = newPosition;
    }
}
