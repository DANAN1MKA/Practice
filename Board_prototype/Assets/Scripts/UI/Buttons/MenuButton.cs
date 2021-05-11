using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    public void ExitMethod()
    {
        menu.SetActive(true);
    }
}
