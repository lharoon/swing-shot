using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NameEntryScreenManager : MonoBehaviour
{
    public GameObject canvas;
    public TMP_InputField inputField;
    public TextMeshProUGUI welcomeText, placeholderName, playerName,
        instructionalText, loadingText;
    public Image foreground;
    public GameObject whiteLine;
    public Shaper2D disc, discFg;

    private bool isEnterKeyPressed;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("playerName"))
            SceneManager.LoadScene(1);
    }

    private void Start()
    {
        StartCoroutine(SetActiveAfterT(foreground.gameObject, false, 0.5f));
    }

    private void Update()
    {
        if (isEnterKeyPressed) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            isEnterKeyPressed = true;

            SavePlayerName();

            #region Hide GUI
            welcomeText.DOFade(0, 0.5f);
            placeholderName.DOFade(0, 0.5f);
            playerName.DOFade(0, 0.5f);
            instructionalText.DOKill();
            instructionalText.DOFade(0, 0.5f);
            #endregion

            whiteLine.SetActive(true);
        }
    }

    private void SavePlayerName()
    {
        if (string.IsNullOrEmpty(inputField.text))
            PlayerPrefs.SetString("playerName", placeholderName.text);
        else
            PlayerPrefs.SetString("playerName", inputField.text);
        print(PlayerPrefs.GetString("playerName"));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("black-line"))
        {
            #region Show GUI
            welcomeText.gameObject.SetActive(true);
            instructionalText.gameObject.SetActive(true);
            inputField.gameObject.SetActive(true);
            #endregion

            // Give focus to input field
            EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
            inputField.OnPointerClick(new PointerEventData(EventSystem.current));

            // Set placeholder text for name field
            placeholderName.text = System.Environment.UserName;

            // Show/hide instructional text
            instructionalText.DOFade(0, 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.OutQuad);
        }
        else if (collision.CompareTag("white-line"))
        {
            loadingText.DOFade(1, 0.5f);
            loadingText.gameObject.SetActive(true);
            //disc.gameObject.SetActive(true);
            //discFg.gameObject.SetActive(true);
            //DOTween.To(() => discFg.arcDegrees, x => discFg.arcDegrees = x, 0, 1)
            //    .SetEase(Ease.OutQuart);
            StartCoroutine("LoadScene");
        }
    }

    private IEnumerator SetActiveAfterT(GameObject go, bool isEnabled, float t)
    {
        yield return new WaitForSeconds(t);
        go.SetActive(isEnabled);
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }
}
