using TMPro;
using UnityEngine;
using Zenject;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;

    [Inject]
    private void Construct(GameManager gameManager)
    {
        gameManager.onStateChanged.AddListener(OnStateChanged);
        OnStateChanged(GameState.None);
    }

    private void OnStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.None:
                textComponent.text = "Пока отдыхаем...";
                break;
            case GameState.Build:
                textComponent.text = "Пора вкалывать...";
                break;
        }
    }
}
