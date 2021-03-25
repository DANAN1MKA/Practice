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

        character.transform.position = config.characterPosition;
    }
}
