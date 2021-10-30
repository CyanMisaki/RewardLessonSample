using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardView : MonoBehaviour
{
    private const string CurrentActiveSlotKey = "CurrentActiveSlotKey";
    private const string TimeGetRewardKey = "TimeGetRewardKey";

    #region Fields
    [Header("Time settings")]
    [SerializeField] private float _timeCooldown = 86400;
    [SerializeField] private float _timeDeadline = 172800;
    
    [Space]
    [Header("Reward list")]
    [SerializeField] private List<Reward> _rewards;
    
    [Space]
    [Header("UI settings")]
    [SerializeField] private TMP_Text _timerNewReward;
    [SerializeField] private Transform _mountRootSlotsReward;
    [SerializeField] private ContainerSlotRewardView _containerSlotRewardView;
    [SerializeField] private Button _getRewardButton;
    [SerializeField] private Button _resetButton;
    [SerializeField] private Slider _progressBar;
    #endregion

    public Slider ProgressBar => _progressBar;
    public ContainerSlotRewardView ContainerSlotRewardView => _containerSlotRewardView;
    public Button GetRewardButton => _getRewardButton;
    public Button ResetButton => _resetButton;
    public Transform MountRootSlotsReward => _mountRootSlotsReward;
    public TMP_Text TimerNewReward => _timerNewReward;
    public List<Reward> Rewards => _rewards;
    public float TimeDeadline => _timeDeadline;
    public float TimeCooldown => _timeCooldown;

    public int CurrentActiveSlot
    {
        get => PlayerPrefs.GetInt(CurrentActiveSlotKey, 0);
        set => PlayerPrefs.SetInt(CurrentActiveSlotKey, value);
    }

    public DateTime? TimeGetReward
    {
        get
        {
            var data = PlayerPrefs.GetString(TimeGetRewardKey, null);

            if (!string.IsNullOrEmpty(data))
                return DateTime.Parse(data);

            return null;
        }
        set
        {
            if (value != null)
                PlayerPrefs.SetString(TimeGetRewardKey, value.ToString());
            else
                PlayerPrefs.DeleteKey(TimeGetRewardKey);
        }
    }

    private void OnDestroy()
    {
        _getRewardButton.onClick.RemoveAllListeners();
        _resetButton.onClick.RemoveAllListeners();
    }
}
