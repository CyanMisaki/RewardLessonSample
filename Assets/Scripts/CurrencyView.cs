using System;
using TMPro;
using UnityEngine;

public class CurrencyView : MonoBehaviour
{
    [SerializeField] private TMP_Text _currentWoodCount;
    [SerializeField] private TMP_Text _currentDiamondCount;
    
    public static CurrencyView Instance { get; private set; }
    
    private const string WoodKey = "WoodKey";
    private const string DiamondKey = "DiamondKey";
  
    private int Wood
    {
        get => PlayerPrefs.GetInt(WoodKey, 0);
        set => PlayerPrefs.SetInt(WoodKey, value);
    }
    private int Diamond
    {
        get => PlayerPrefs.GetInt(DiamondKey, 0);
        set => PlayerPrefs.SetInt(DiamondKey, value);
    }
    
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        RefreshRewardsValueText();
    }
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    
    public void AddWood(int value)
    {
        Wood += value;

        RefreshRewardsValueText();
    }
    public void AddDiamond(int value)
    {
        Diamond += value;

        RefreshRewardsValueText();
    }
    private void RefreshRewardsValueText()
    {
        if (_currentDiamondCount!=null)
            _currentDiamondCount.text = Diamond.ToString();
        if (_currentWoodCount!=null)
            _currentWoodCount.text = Wood.ToString();
    }
    
}
