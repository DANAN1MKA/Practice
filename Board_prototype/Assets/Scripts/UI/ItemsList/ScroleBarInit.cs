using UnityEngine;
using UnityEngine.UI;

public class ScroleBarInit : MonoBehaviour
{
    private Scrollbar scrollbar;

    private void Awake()
    {
        scrollbar = GetComponent<Scrollbar>();
    }
    private void Start()
    {
        scrollbar.value = 1f;
    }

    public void onValueChaged()
    {
        if (scrollbar.value > 1 || scrollbar.value < 0)
        {
            scrollbar.value = scrollbar.value > 1 ? 1 : scrollbar.value < 0 ? 0 : scrollbar.value;
        }
    }

}
