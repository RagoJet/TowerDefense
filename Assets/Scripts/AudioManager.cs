using UnityEngine;

public class AudioManager : MonoBehaviour{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] audioClips;

    public static AudioManager Instance{ get; private set; }

    private void Awake(){
        _audioSource = GetComponent<AudioSource>();
        Instance = this;
    }

    public void PlayShotSound(int levelWeapon){
        switch (levelWeapon){
            case <5:
                _audioSource.PlayOneShot(audioClips[0]);
                break;
            case <9:
                _audioSource.PlayOneShot(audioClips[1]);
                break;
            case <13:
                _audioSource.PlayOneShot(audioClips[2]);
                break;
            case <17:
                _audioSource.PlayOneShot(audioClips[3]);
                break;
            case <21:
                _audioSource.PlayOneShot(audioClips[4]);
                break;
            case <25:
                _audioSource.PlayOneShot(audioClips[5]);
                break;
        }
    }
}