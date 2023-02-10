using UnityEngine;

public class UIStarter : MonoBehaviour
{
    [SerializeField] private AbilityShop _abilityShop;
    [SerializeField] private GunShop _gunShop;
    [SerializeField] private LeaderBoardDisplay _leaderBoard;
    [SerializeField] private CharacterWallet _wallet;

    private void Start()
    {
        InitialLeaderBoard();
        InitialAbilityShop();
        InitialGunShop();
    }

    private void InitialGunShop()
    {
        _gunShop.gameObject.SetActive(true);
        _gunShop.gameObject.SetActive(false);
    }

    private void InitialLeaderBoard()
    {
        _leaderBoard.gameObject.SetActive(true);
        _leaderBoard.gameObject.SetActive(false);
    }

    private void InitialAbilityShop()
    {
        _abilityShop.gameObject.SetActive(true);
        _abilityShop.gameObject.SetActive(false);
        _abilityShop.InitialUI();
    }
}