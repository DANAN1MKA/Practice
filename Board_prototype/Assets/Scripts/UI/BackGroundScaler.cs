using UnityEngine;
using Zenject;

public class BackGroundScaler : MonoBehaviour
{
    [Inject] BoardProperties config;
    GameObject back;

    public void Awake()
    {
        back = gameObject;
    }

    public void Start()
    {
        back.transform.position = new Vector2(0, (float)(config.boardPosition.y + config.height * config.scale) + 3.4f);
    }
}
