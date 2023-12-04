using DefaultNamespace;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private AudioSource audioSourceSounds;
    [SerializeField] private AudioSourcesSO audioSources;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
        
        audioSourceMusic.loop = true;
    }

    public void PlayMenuMusic()
    {
        audioSourceMusic.clip = audioSources.menuMusic[Random.Range(0, audioSources.menuMusic.Length)];
        audioSourceMusic.Play();
    }

    public void PlayGameMusic()
    {
        audioSourceMusic.clip = audioSources.gameMusic[Random.Range(0, audioSources.gameMusic.Length)];
        audioSourceMusic.Play();
    }
    
    public void PlayEnemyReachedPortalSound()
    {
        audioSourceSounds.clip = audioSources.enemyReachedPortalSound;
        audioSourceSounds.Play();
    }

    public void PlayEnemyDiedSound()
    {
        audioSourceSounds.clip = audioSources.enemyDiedSound;
        audioSourceSounds.Play();
    }
    
    public void PlayTowerAttackSound()
    {
        audioSourceSounds.clip = audioSources.towerAttackSound;
        audioSourceSounds.Play();
    }
    
    public void PlayTowerRangeDamageSound()
    {
        audioSourceSounds.clip = audioSources.towerRangeDamageSound;
        audioSourceSounds.Play();
    }

}
