using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private LeaderBoardDisplay _leaderBoardDisplay;
    [SerializeField] private Button _closeButton;
    [SerializeField] private CharacterWallet _wallet;
#if VK_GAMES
    [SerializeField] private Wallet _wallet;
#endif

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
        _closeButton.onClick.AddListener(OnCloseClock);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
        _closeButton.onClick.RemoveListener(OnCloseClock);
    }

    private void OnCloseClock() => Hide();

    private void OnClick()
    {
//#if YANDEX_GAMES
        if (_leaderBoardDisplay.gameObject.activeSelf)
            Hide();
        else
            Show();
//#endif
//#if VK_GAMES
//       Agava.VKGames.Leaderboard.ShowLeaderboard(Convert.ToInt32(_wallet.Money));
//#endif
    }

    private void Show()
    {
        _leaderBoardDisplay.gameObject.SetActive(true);
        _leaderBoardDisplay.SetLeaderboardScore();
        _leaderBoardDisplay.OpenLeaderboard();
    }

    private void Hide() => _leaderBoardDisplay.gameObject.SetActive(false);
}