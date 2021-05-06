using UnityEngine;
using Zenject;

public class AttackPositionScaler : MonoBehaviour
{
    [Inject] private BoardProperties config;

    private void Start()
    {
        transform.position = new Vector2(0, config.boardPositionFromResolution.y + (config.height + 2) * config.scale);
        transform.localScale = new Vector3(config.scale, config.scale, 1);
    }
}
