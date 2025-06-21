using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    [SerializeField] GameObject _gov;
    [SerializeField] TextMeshProUGUI _score;
    [SerializeField] ScoreManager ScoreManager;
    [SerializeField] AudioClip _clip;
    [SerializeField] Button _startButton;
    [SerializeField] RectTransform canvas;
        
    private void Start()
    {   
        GameState.OnGameOver += SetGOV;
        GameState.OnReady += GetReady;
        GameState.OnStart += GetStart;
        var newHeight = canvas.sizeDelta.y * 0.25f;
        var rect = _startButton.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);
    }
    private void OnDestroy()
    {
        GameState.OnGameOver -= SetGOV;
        GameState.OnReady -= GetReady;
        GameState.OnStart -= GetStart;
    }
    private void OnDisable()
    {
        transform.DOKill(); 
    }
    void SetGOV()
    {
        StartCoroutine(TriggerGov());
    }
    IEnumerator TriggerGov()
    {
        yield return new WaitForSeconds(3f);
        _score.text = ScoreManager._Score.ToString();
        CanvasGroup _cv = _gov.GetComponent<CanvasGroup>();
        
        _cv.alpha = 0f;
        _cv.DOFade(1f, 2f).SetEase(Ease.InOutBounce)
            .OnComplete(() =>
            {
                SFXManager.Instance.PlaySfx(_clip);
                _cv.interactable = true;
                _cv.blocksRaycasts = true;
                _gov.SetActive(true);
            }).SetUpdate(true);
    }
    void GetReady()
    {
        _startButton.gameObject.SetActive(true);
    }
    void GetStart()
    {
        _startButton.gameObject.SetActive(false);
    }
    public void StartGame()
    {
        GameState.Instance.SelectState(State.play);
        
    }
    public void RestartGame()
    {
        GameState.Instance.SelectState(State.ready);
        _gov.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
