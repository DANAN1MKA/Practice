using UnityEngine;
using Zenject;

public class CharacterController : MonoBehaviour
{
    [Inject] private SignalBus signalBus;
    [Inject] private BoardProperties config;
    private GameObject character;

    public void Awake()
    {
        character = Instantiate(config.characterPrefab);
        MainCharacterController crutch = character.GetComponent<MainCharacterController>();

        crutch.setSignalBus(signalBus);

    }

    public void Start()
    {
        Vector2 newPosition = new Vector2(config.characterPosition.x * config.scale, 
                                          config.boardPositionFromResolution.y + (config.height + 0.8f) * config.scale);

        character.transform.position = newPosition;
        character.transform.localScale *= config.scale;
    }
}
