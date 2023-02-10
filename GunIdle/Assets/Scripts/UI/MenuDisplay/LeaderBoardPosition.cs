using UnityEngine;
using TMPro;
using Agava.YandexGames;

public class LeaderBoardPosition : MonoBehaviour
{
    [SerializeField] private TMP_Text _numberPosition;
    [SerializeField] private TMP_Text _playerName;
    [SerializeField] private TMP_Text _score;

    private readonly string _anonymous = "Anonymous";

    public void Init(LeaderboardEntryResponse leaderboardEntryResponse)
    {
        _numberPosition.text = leaderboardEntryResponse.rank.ToString();

        string name = leaderboardEntryResponse.player.publicName;
        if (string.IsNullOrEmpty(name))
            name = _anonymous;

        _playerName.text = name;
        _score.text = leaderboardEntryResponse.formattedScore;
    }
}