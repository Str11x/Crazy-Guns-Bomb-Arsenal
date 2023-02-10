using Agava.YandexGames;
using System.Collections;
using TMPro;
using UnityEngine;

public class LeaderBoardDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerScore;
    [SerializeField] private CharacterWallet _wallet;
    [SerializeField] private TMP_Text[] _ranks;
    [SerializeField] private TMP_Text[] _leaderNames;
    [SerializeField] private TMP_Text[] _scoreList;
    [SerializeField] private string _leaderboardName = "LeaderBoard";
    [SerializeField] private TMP_Text _authorizationStatusText;
    [SerializeField] private TMP_Text _personalProfileDataPermissionStatusText;

    private int _money;
    private readonly string _anonimus = "Anonimus";

    private void Awake()
    {
        YandexGamesSdk.CallbackLogging = true;
        _money = (int)_wallet.Money;

        StartCoroutine(Start());
        gameObject.SetActive(false);
    }

    public void OpenLeaderboard()
    {
        //#if UNITY_WEBGL && !UNITY_EDITOR
        _money = (int)_wallet.Money;

        StartCoroutine(Start());

        Leaderboard.GetEntries(_leaderboardName, (result) =>
        {
            int leadersNumber = result.entries.Length >= _leaderNames.Length ? _leaderNames.Length : result.entries.Length;
            
            for (int i = 0; i < leadersNumber; i++)
            {
                string name = result.entries[i].player.publicName;

                if (string.IsNullOrEmpty(name))
                    name = _anonimus;

                _leaderNames[i].text = name;
                _scoreList[i].text = result.entries[i].formattedScore;
                _ranks[i].text = result.entries[i].rank.ToString();
            }
        },
        (error) =>
        {
            //_logInPanel.Show();
        });
        //#endif
    }

    public void SetLeaderboardScore()
    {
        //#if UNITY_WEBGL && !UNITY_EDITOR
        Leaderboard.GetPlayerEntry(_leaderboardName, OnSuccessCallback);
        //#endif
    }

    private IEnumerator Start()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        //yield break;
#endif

        // Always wait for it if invoking something immediately in the first scene.
        yield return YandexGamesSdk.Initialize();

        while (true)
        {
            _authorizationStatusText.color = PlayerAccount.IsAuthorized ? Color.green : Color.red;

            if (PlayerAccount.IsAuthorized)
                _personalProfileDataPermissionStatusText.color = PlayerAccount.HasPersonalProfileDataPermission ? Color.green : Color.red;
            else
                _personalProfileDataPermissionStatusText.color = Color.red;

            yield return new WaitForSecondsRealtime(0.25f);
        }
    }

    private void OnSuccessCallback(LeaderboardEntryResponse result)
    {
        if (result == null || _wallet.Money > result.score)
        {
            Leaderboard.SetScore(_leaderboardName, (int)_wallet.Money);
        }
    }
}