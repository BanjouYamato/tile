
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    int Score;
    [SerializeField]TextMeshProUGUI _scoreText;
    public int _Score => Score;
    [SerializeField] AudioClip _clip;
    int multiple;

    private void Start()
    {
        Score = 0;
        _scoreText.text = "0";
        Observer.Instance.AddToObser<int>(ObserverConstant.onScore,GainScore);
        SetMultiple(1);
    }
    private void OnDestroy()
    {
        Observer.Instance.RemoveObser<int>(ObserverConstant.onScore, GainScore);
    }

    void GainScore(int scorePlus)
    {
        Score += scorePlus*multiple;
        _scoreText.text = Score.ToString();
        SFXManager.Instance.PlaySfx(_clip);
    }
    public void SetMultiple(int _mul)
    {
        multiple = _mul;
    }
}
