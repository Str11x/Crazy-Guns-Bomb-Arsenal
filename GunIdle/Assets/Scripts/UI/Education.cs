using UnityEngine;
using UnityEngine.UI;

public class Education : MonoBehaviour
{
    [SerializeField] private Canvas _buyUpgradeScreen;
    [SerializeField] private Canvas _buyAbilityScreen;
    [SerializeField] private Canvas _overheatingScreen;
    [SerializeField] private Canvas _buyWeaponScreen;
    [SerializeField] private Canvas _shooterEnemiesScreen;
    [SerializeField] private Canvas _bombAdvicelScreen;
    [SerializeField] private Canvas _citizenScreen;
    [SerializeField] private Level _level;

    [SerializeField] private Button _ability;
    [SerializeField] private Button _weapon;

    private bool _isOverheatShowed;
    private const int BUY_UPGRADE_LEVEL = 0;
    private const int BUY_ABILITY_LEVEL = 1;
    private const int BUY_WEAPON_LEVEL = 2;
    private const int CITIZEN_LEVEL = 5;
    private const int SHOOTER_ENEMIES_LEVEL = 7;
    private const int BOMB_ADVICE_LEVEL = 8;
    private const int END_EDUCATION = 9;

    private void OnEnable()
    {
        if (PlayerPrefs.GetInt(KeySave.LEVEL_NUMBER) == BUY_UPGRADE_LEVEL)
            _level.EnemyDie += ShowOverheatEducation;
    }

    private void OnDisable()
    {
        if (PlayerPrefs.GetInt(KeySave.LEVEL_NUMBER) == BUY_UPGRADE_LEVEL)
            _level.EnemyDie -= ShowOverheatEducation; 
    }

    private void Start()
    {
        int currentNumber = PlayerPrefs.GetInt(KeySave.LEVEL_NUMBER);
        int educationStep = PlayerPrefs.GetInt(KeySave.EDUCATION_STEP);

        if(currentNumber != END_EDUCATION)
        {
            switch (currentNumber)
            {
                case BUY_UPGRADE_LEVEL:
                    ShowUpgrade(educationStep);
                        break;
                case BUY_ABILITY_LEVEL:
                    ShowAbilities(educationStep);
                    break;
                case BUY_WEAPON_LEVEL:
                    ShowWeaponShop(educationStep);
                    break;
                case CITIZEN_LEVEL:
                    ShowCitizenScreen(educationStep);
                    break;
                case SHOOTER_ENEMIES_LEVEL:
                    ShowShooterEnemies(educationStep);
                    break;
                case BOMB_ADVICE_LEVEL:
                    ShowBombAdvice(educationStep);
                    break;
            }
        }  
    }

    public void HideOverheatEducation()
    {
        Time.timeScale = 1;
        _isOverheatShowed = true;
        _overheatingScreen.gameObject.SetActive(false);
    }

    private void ShowOverheatEducation()
    {
        if(_isOverheatShowed != true)
        {
            _overheatingScreen.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void ShowUpgrade(int educationStep)
    {
        if(educationStep == BUY_UPGRADE_LEVEL)
        {
            _ability.gameObject.SetActive(false);
            _buyUpgradeScreen.gameObject.SetActive(true);
            PlayerPrefs.SetInt(KeySave.EDUCATION_STEP, BUY_ABILITY_LEVEL);
        }      
    }

    private void ShowAbilities(int educationStep)
    {
        if(educationStep == BUY_ABILITY_LEVEL)
        {
            _buyAbilityScreen.gameObject.SetActive(true);
            PlayerPrefs.SetInt(KeySave.EDUCATION_STEP, BUY_WEAPON_LEVEL);
        }     
    }

    private void ShowWeaponShop(int educationStep)
    {
        if(educationStep == BUY_WEAPON_LEVEL)
        {
            _buyWeaponScreen.gameObject.SetActive(true);
            PlayerPrefs.SetInt(KeySave.EDUCATION_STEP, CITIZEN_LEVEL);
        }       
    }

    private void ShowShooterEnemies(int educationStep)
    {
        if(educationStep == SHOOTER_ENEMIES_LEVEL)
        {
            _shooterEnemiesScreen.gameObject.SetActive(true);
            PlayerPrefs.SetInt(KeySave.EDUCATION_STEP, BOMB_ADVICE_LEVEL);
        }       
    }

    private void ShowBombAdvice(int educationStep)
    {
        if(educationStep == BOMB_ADVICE_LEVEL)
        {
            _bombAdvicelScreen.gameObject.SetActive(true);
            PlayerPrefs.SetInt(KeySave.EDUCATION_STEP, END_EDUCATION);
        }
    }

    private void ShowCitizenScreen(int educationStep)
    {
        if (educationStep == CITIZEN_LEVEL)
        {
            _citizenScreen.gameObject.SetActive(true);
            PlayerPrefs.SetInt(KeySave.EDUCATION_STEP, SHOOTER_ENEMIES_LEVEL);
        }
    }
}