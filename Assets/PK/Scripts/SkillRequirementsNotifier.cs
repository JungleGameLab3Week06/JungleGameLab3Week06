using UnityEngine.UI;
using UnityEngine;

public class SkillRequirementsNotifier : MonoBehaviour
{
    public static SkillRequirementsNotifier Instance;
    private Image imageBG; // Using for BG Changes
    private Image imageSkillRequirements;

    [SerializeField] Sprite[] images;

    [Header("Color")]
    private Color colorCertain;
    private Color colorNotCertain;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;

        // Get Component
        imageBG = transform.GetChild(0).GetComponent<Image>(); // BG Layer
        imageSkillRequirements = transform.GetChild(1).GetComponent<Image>(); // Skill Icon

        // Set initial color
        ColorUtility.TryParseHtmlString("#00D0FF", out colorCertain);
        ColorUtility.TryParseHtmlString("#FFBD1B", out colorNotCertain);
    }

    // Status friend request is certain, not type
    void ShowFriendRequestStatus(bool isCertian)
    {
        if(isCertian) // If certain to decide
        {
            imageBG.color = colorCertain;
        }
        else // if not
        {
            imageBG.color = colorNotCertain;
        }
    }

    // Status of type
    void ShowFriendRequestType(Define.Elemental elemental)
    {
        switch(elemental)
        {
            case Define.Elemental.Flame:
                imageSkillRequirements.sprite = images[0];
                break;
            case Define.Elemental.Water:
                imageSkillRequirements.sprite = images[1];
                break;
            case Define.Elemental.Ground:
                imageSkillRequirements.sprite = images[2];
                break;
            case Define.Elemental.Lightning:
                imageSkillRequirements.sprite = images[3];
                break;
            case Define.Elemental.None:
                Debug.LogError("There are no command for this defined types");
                break;
            default:
                Debug.LogError("There are no command for nulls");
                break;
        }
    }
}
