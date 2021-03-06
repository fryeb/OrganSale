using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName="config", menuName="Config", order=1)]
public class Config : ScriptableObject
{
    public float MovementSpeed = 16;

    [Range(0, 1)]
    public float GameVolume = 1;
    [Range(0, 1)]
    public float TitleVolume = 1;

    public float SaleDistance = 64;
    public int BrainPrice = 100;
    public int HeartPrice = 100;
    public int LungPrice = 100;
    public int LeftKidneyPrice = 100;
    public int RightKidneyPrice = 100;
    public int SpleenPrice = 100;
    public int WinPrice = 100;

    public double BleedDelay = 1;
    public double BleedSoundThreshold = 10;
    public double MaxBlood = 100;
    public double BrainBleed = 1;
    public double HeartBleed = 1;
    public double LungBleed = 1;
    public double LeftKidneyBleed = 1;
    public double RightKidneyBleed = 1;
    public double SpleenBleed = 1;

    public Sprite BrainSprite;
    public Sprite NoBrainSprite;
    public Sprite HeartSprite;
    public Sprite NoHeartSprite;
    public Sprite LungSprite;
    public Sprite NoLungSprite;
    public Sprite LeftKidneySprite;
    public Sprite NoLeftKidneySprite;
    public Sprite RightKidneySprite;
    public Sprite NoRightKidneySprite;
    public Sprite SpleenSprite;
    public Sprite NoSpleenSprite;

    public VideoClip introVideo;
    public VideoClip titleVideo;
    public VideoClip deathVideo;
    public VideoClip deathTitleVideo;
    public VideoClip winVideo;
    public VideoClip winTitleVideo;

    public AudioClip saleSFX;
    public AudioClip brainSFX;
    public AudioClip titleSong;
    public AudioClip gameplaySong;

    public AudioClip playerGameStart;
    public AudioClip playerFootsteps;
    public AudioClip playerBleedingOut;

    void OnValidate()
    {
        Debug.AssertFormat(BrainBleed > HeartBleed, $"BrainBleed({BrainBleed}) must be greater than HeartBleed({HeartBleed})");
        Debug.AssertFormat(HeartBleed > LungBleed, $"HeartBleed({HeartBleed}) must be greater than LungBleed({LungBleed})");
        Debug.AssertFormat(LungBleed > LeftKidneyBleed, $"LungBleed({LungBleed}) must be greater than LeftKidneyBleed({LeftKidneyBleed})");
        Debug.AssertFormat(LeftKidneyBleed > RightKidneyBleed, $"LeftKidneyBleed({LeftKidneyBleed}) must be greater than RightKidneyBleed({RightKidneyBleed})");
        Debug.AssertFormat(RightKidneyBleed > SpleenBleed, $"RightKidneyBleed({RightKidneyBleed}) must be greater than SpleenSprite({SpleenBleed})");
        Debug.AssertFormat(SpleenBleed > 0, $"SpleenBleed({SpleenBleed}) must be greater than 0");
    }
}
