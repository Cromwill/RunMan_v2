using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ExitPanel : MonoBehaviour
{
    [SerializeField] private Text _scoreValue;
    [SerializeField] private Text _distanceValue;

    [SerializeField] private Button _exit;
    [SerializeField] private Button _replay;
    [SerializeField] private Button _back;
    [SerializeField] private AddMoneyViewer _addMoneyViewer;
    [SerializeField] private GameObject[] _someObjectsForchangeActivation;

    private float _decrieseStep;

    private int _currentScore;
    private float _currentDistance;
    private int _currentMoney;

    public event Action<int> OnExit;

    private void Start()
    {
        _back.onClick.AddListener(ClosePanel);
        _exit.onClick.AddListener(Exit);
        _replay.onClick.AddListener(Replay);
        _addMoneyViewer.ConvertToMoneyStarting += delegate { StartCoroutine(DecrieseScore()); };

        var pool = FindObjectOfType<MapElementPool>();
        var fogPool = FindObjectOfType<FogPool>();

        pool.SetExitPanel(this);
        fogPool.SetExitPanel(this);
    }

    public void ShowPanel(bool isGameOver) => OpenPanel(isGameOver);

    public void ShowPanel(bool isGameOver, int score, float distance, int money)
    {
        _currentScore = score;
        _currentDistance = distance;
        _currentMoney = money;
        _decrieseStep = score/10;

        OpenPanel(isGameOver);
        ShowDatas(score, distance);
        if (isGameOver)
            _addMoneyViewer.ShowAddingMoney();
    }

    private void ShowDatas(float score, float distance)
    {
        _scoreValue.text = score.ToString("0.#");
        _distanceValue.text = distance.ToString("0.0#");
    }

    private void OpenPanel(bool isGameOver)
    {
        Time.timeScale = isGameOver ? 1 : 0;
        gameObject.SetActive(true);
        SetActiveForSeveralObject(isGameOver, _scoreValue.gameObject, _distanceValue.gameObject);
        SetActiveForSeveralObject(isGameOver, _someObjectsForchangeActivation);
        SetActiveForSeveralObject(!isGameOver, _back.gameObject);
    }

    private IEnumerator DecrieseScore()
    {

        while(_currentScore > 0)
        {
            _currentScore -= (int)_decrieseStep;
            if (_currentScore < 0)
                _currentScore = 0;

            ShowDatas(_currentScore, _currentDistance);

            yield return new WaitForSeconds(0.05f);
        }

        _addMoneyViewer.ShowAddingMoneyAnimation(_currentMoney);
        SaveDataStorage.AddScore(new Score(_currentMoney, 0));
    }


    private void Exit()
    {
        OnExit?.Invoke(0);
        Time.timeScale = 1;
    }

    private void Replay()
    {
        OnExit?.Invoke(1);
    }

    private void ClosePanel()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    private void SetActiveForSeveralObject(bool active, params GameObject[] objects)
    {
        foreach(GameObject obj in objects)
        {
            obj.SetActive(active);
        }
    }
}
