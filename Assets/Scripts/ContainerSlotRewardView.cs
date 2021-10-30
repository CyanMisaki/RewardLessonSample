using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContainerSlotRewardView : MonoBehaviour
{
    [SerializeField] private Image _slotBackground;
    [SerializeField] private Image _iconCurrency;
    [SerializeField] private TMP_Text _textDay;
    [SerializeField] private TMP_Text _rewardCount;

    public void SetData(Reward reward, int dayNum, bool isSelected)
    {
        _iconCurrency.sprite = reward.Icon;
        _textDay.text = $"Day {dayNum}";
        _rewardCount.text = reward.Count.ToString();
        _slotBackground.gameObject.SetActive(isSelected);
    }
}
