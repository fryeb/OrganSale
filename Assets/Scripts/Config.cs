using UnityEngine;

[CreateAssetMenu(fileName="config", menuName="Config", order=1)]
public class Config : ScriptableObject
{
    public float MovementSpeed = 16;

    public int BrainPrice = 100;
    public int HeartPrice = 100;
    public int LungPrice = 100;
    public int LeftKidneyPrice = 100;
    public int RightKidneyPrice = 100;
    public int SpleenPrice = 100;

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

}
