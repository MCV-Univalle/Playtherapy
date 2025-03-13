using UnityEngine;
using UnityEngine.UI;

public class NPCSDropdownController : MonoBehaviour
{
    public Dropdown speedDropdown;
    public static float enemySpeed = 3f; // Valor predeterminado

    private void Start()
    {
        speedDropdown.onValueChanged.AddListener(delegate { UpdateSpeed(speedDropdown.value); });
    }

    private void UpdateSpeed(int index)
    {
        switch (index)
        {
            case 0: // Paso lento
                enemySpeed = 2f;
                break;
            case 1: // Paso medio
                enemySpeed = 3.5f;
                break;
            case 2: // Paso rápido
                enemySpeed = 5f;
                break;
        }
    }
}