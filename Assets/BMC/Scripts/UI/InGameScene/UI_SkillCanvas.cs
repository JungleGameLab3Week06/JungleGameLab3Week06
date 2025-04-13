using System.Collections;
using TMPro;
using UnityEngine;

public class UI_SkillCanvas : MonoBehaviour
{
    Canvas _skillCanvas;
    TextMeshProUGUI _skillText;
    Coroutine _activateSkillCoroutine;
    float _disappearWeight = 1f;

    void Start()
    {
        _skillCanvas = GetComponent<Canvas>();
        _skillText = GetComponentInChildren<TextMeshProUGUI>();

        Manager.UI.activateSkillTextAction += ActivateSkillText;
    }

    public void ActivateSkillText(string skillDescription)
    {
        if(_activateSkillCoroutine != null)
            StopCoroutine(_activateSkillCoroutine);
        _activateSkillCoroutine = StartCoroutine(ActivateSkillTextCoroutine(skillDescription));
    }

    public IEnumerator ActivateSkillTextCoroutine(string skillDescription)
    {
        _skillText.text = skillDescription;
        _skillCanvas.enabled = true;

        // Fade out text
        float alphaValue = 1f;
        _skillText.color = new Color(_skillText.color.r, _skillText.color.g, _skillText.color.b, alphaValue);
        while (alphaValue > 0)
        {
            alphaValue -= Time.deltaTime * _disappearWeight;
            _skillText.color = new Color(_skillText.color.r, _skillText.color.g, _skillText.color.b, alphaValue);
            yield return null;
        }

        // Hide Text
        alphaValue = 0;
        _skillText.text = "";
        _skillText.color = new Color(_skillText.color.r, _skillText.color.g, _skillText.color.b, alphaValue);
    }
}