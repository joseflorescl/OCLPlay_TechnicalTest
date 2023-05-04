using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject readyText;
    [SerializeField] private GameObject goText;
    [SerializeField] private GameObject grabButton;


    [Space(10)]
    [Header("Settings")]
    [SerializeField] private float initialDelay = 0.1f;

    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += GameStartHandler;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= GameStartHandler;
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


    IEnumerator ShowUIRoutine()
    {
        yield return new WaitForSeconds(initialDelay);
        
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
}
