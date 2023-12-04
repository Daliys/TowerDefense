using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "AudioSourcesSO", menuName = "Tower Defense/AudioSourcesSO")]
    public class AudioSourcesSO : ScriptableObject
    {
       public AudioClip[] menuMusic;
       public AudioClip[] gameMusic;

       public AudioClip enemyReachedPortalSound;
       public AudioClip enemyDiedSound;
       
       public AudioClip towerAttackSound;
       public AudioClip towerRangeDamageSound;
    }
}