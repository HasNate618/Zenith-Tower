using UnityEngine;

public class Enhancements : MonoBehaviour
{
    [SerializeField] CharacterShooting shooting;
    [SerializeField] CharacterMovement movement;
    [SerializeField] PlayerHealth health;

    private float _bowDamageLevel, _bowRangeLevel, _reloadSpeedLevel, _maxHealthLevel, _absorptionLevel, _characterSpeedLevel, _visionLevel;

    private float defaultBowDamage = 20;
    public float bowDamageLevel
    {
        get
        {
            return _bowDamageLevel;
        }
        set
        {
            GetComponent<CharacterShooting>().arrowDamage = defaultBowDamage + value * 4;
            _bowRangeLevel = value;
        }
    }    
    
    private float defaultBowRange = 6;
    public float bowRangeLevel
    {
        get
        {
            return _bowRangeLevel;
        }
        set
        {
            GetComponent<CharacterShooting>().bowRange = defaultBowRange + value;
            _bowRangeLevel = value;
        }
    }
    
    private float defaultMaxHealth = 75;
    public float maxHealthLevel
    {
        get
        {
            return _maxHealthLevel;
        }
        set
        {
            GetComponent<PlayerHealth>().EnhanceHealth(defaultMaxHealth + value * 15);
            _maxHealthLevel = value;
        }
    }
    
    public float absorptionLevel
    {
        get
        {
            return _absorptionLevel;
        }
        set
        {
            GetComponent<PlayerHealth>().healAmount = value * 0.01f;
            _absorptionLevel = value;
        }
    }
    
    private float defaultReloadSpeed = 3;
    public float reloadSpeedLevel
    {
        get
        {
            return _reloadSpeedLevel;
        }
        set
        {
            GetComponent<CharacterShooting>().reloadSpeed = defaultReloadSpeed - value * 0.25f;
            _reloadSpeedLevel = value;
        }
    }

    private float defaultCharacterSpeed = 4;
    public float characterSpeedLevel
    {
        get
        {
            return _characterSpeedLevel;
        }
        set
        {
            GetComponent<CharacterMovement>().speed = defaultCharacterSpeed + value * 0.24f;
            _characterSpeedLevel = value;
        }
    }

    [SerializeField] Transform cameraSize;
    private float defaultCameraSize = 1;
    public float visionLevel
    {
        get
        {
            return _visionLevel;
        }
        set
        {
            float newSize = defaultCameraSize + value * 0.06f;
            cameraSize.localScale = new Vector3(newSize, newSize, newSize);
            _visionLevel = value;
        }
    }
}
