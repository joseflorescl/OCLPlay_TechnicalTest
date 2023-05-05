using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject readyText;
    [SerializeField] private GameObject goText;
    [SerializeField] private GameObject grabButton;
    [SerializeField] private Image fadeImage;


    [Space(10)]
    [Header("Settings")]
    [SerializeField] private float timeToFadeBackground = 0.5f;

    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += GameStartHandler;
        GameManager.Instance.OnGameEnd += GameEndHandler;
    }    

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= GameStartHandler;
        GameManager.Instance.OnGameEnd -= GameEndHandler;
    }

    private void Awake()
    {
        // Make sure to have UI elements disabled
        readyText.SetActive(false);
        goText.SetActive(false);
        grabButton.SetActive(false);
    }

    private void GameStartHandler(int level)
    {
        StartCoroutine(ShowUIRoutine());
    }

    private void GameEndHandler()
    {
        FadeGraphic(fadeImage, 0f, 1f, timeToFadeBackground);
    }

    IEnumerator ShowUIRoutine()
    {
        FadeGraphic(fadeImage, 1f, 0f, timeToFadeBackground);
        
        yield return new WaitForSeconds(timeToFadeBackground);
        
        float length = ActivateTextAnimation(readyText);

        yield return new WaitForSeconds(length);

        length = ActivateTextAnimation(goText);

        yield return new WaitForSeconds(length);

        grabButton.SetActive(true);

        GameManager.Instance.UIShown();        
    }

    float ActivateTextAnimation(GameObject text)
    {
        text.SetActive(true);
        Animator anim = readyText.GetComponent<Animator>();
        return anim.GetCurrentAnimatorStateInfo(0).length;
    }

    void FadeGraphic(Graphic graphic, float fromAlpha, float toAlpha, float duration)
    {
        graphic.canvasRenderer.SetAlpha(fromAlpha);
        graphic.CrossFadeAlpha(toAlpha, duration, true);
    }
}
