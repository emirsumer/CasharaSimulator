using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioSource gameMusic;

    [Header("Building SFX")]
    [SerializeField] private AudioSource buildingPlaceSfx;

    [Header("Shelf / Product SFX")]
    [SerializeField] private AudioSource shelfPlaceSfx;    
    [SerializeField] private AudioSource productPickupSfx;  
    [SerializeField] private AudioSource productDropSfx; 

    [Header("AI SFX")]
    [SerializeField] private AudioSource aiTakeSfx; 
    [SerializeField] private AudioSource aiPaySfx;

    [Header("Footstep SFX")]
    [SerializeField] private AudioSource rightStepSfx;
    [SerializeField] private AudioSource leftStepSfx;

    [Header("UI SFX")]
    [SerializeField] private AudioSource clickSfx;              
    [SerializeField] private AudioSource orderConfirmSfx;       
    [SerializeField] private AudioSource insufficientFundsSfx;
    [SerializeField] private AudioSource maxOrderExceededSfx; 
    [SerializeField] private AudioSource moneySfx; 

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayGameMusic()
    {
        if (gameMusic != null && !gameMusic.isPlaying)
        {
            gameMusic.Play();
        }
    }
    public void PauseGameMusic()
    {
        if (gameMusic != null && gameMusic.isPlaying)
        {
            gameMusic.Pause();
        }
    }

    public void ResumeGameMusic()
    {
        if (gameMusic != null)
        {
            gameMusic.UnPause();
        }
    }
    public void PlayBuildingPlace()
    {
        if (buildingPlaceSfx != null)
        {
            buildingPlaceSfx.PlayOneShot(buildingPlaceSfx.clip);
        }
    }

    public void PlayShelfPlace()
    {
        if (shelfPlaceSfx != null)
        {
            shelfPlaceSfx.PlayOneShot(shelfPlaceSfx.clip);
        }
    }

    public void PlayProductPickup()
    {
        if (productPickupSfx != null)
        {
            productPickupSfx.PlayOneShot(productPickupSfx.clip);
        }
    }

    public void PlayProductDrop()
    {
        if (productDropSfx != null)
        {
            productDropSfx.PlayOneShot(productDropSfx.clip);
        }
    }

    public void PlayAiTake()
    {
        if (aiTakeSfx != null)
        {
            aiTakeSfx.PlayOneShot(aiTakeSfx.clip);
        }
    }

    public void PlayAiPay()
    {
        if (aiPaySfx != null)
        {
            aiPaySfx.PlayOneShot(aiPaySfx.clip);
        }
    }

    public void PlayRightStepSfx()
    {
        if (rightStepSfx != null)
        {
            rightStepSfx.Play();
        }
    }

    public void PlayLeftStepSfx()
    {
        if (leftStepSfx != null)
        {
            leftStepSfx.Play();
        }
    }

    public void PlayClickSfx()
    {
        if (clickSfx != null)
        {
            clickSfx.PlayOneShot(clickSfx.clip);
        }
    }

    public void PlayOrderConfirmSfx()
    {
        if (orderConfirmSfx != null)
        {
            orderConfirmSfx.PlayOneShot(orderConfirmSfx.clip);
        }
    }

    public void PlayInsufficientFundsSfx()
    {
        if (insufficientFundsSfx != null)
        {
            insufficientFundsSfx.PlayOneShot(insufficientFundsSfx.clip);
        }
    }

    public void PlayMaxOrderExceededSfx()
    {
        if (maxOrderExceededSfx != null)
        {
            maxOrderExceededSfx.PlayOneShot(maxOrderExceededSfx.clip);
        }
    }
    public void PlayMoneySfx()
    {
        if (moneySfx != null)
        {
            moneySfx.PlayOneShot(moneySfx.clip);
        }
    }
}
