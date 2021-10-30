using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class DailyRewardController
{ 
    private DailyRewardView _dailyRewardView;
    private List<ContainerSlotRewardView> _slots;
  
    private bool _rewardReceived;

    private const float _numOfRewardsInRow = 4f;
    public DailyRewardController(DailyRewardView generateLevelView)
    {
        _dailyRewardView = generateLevelView;
        _dailyRewardView.ProgressBar.maxValue = _numOfRewardsInRow;
        _dailyRewardView.ProgressBar.value = 1;
    }
  
     public void RefreshView()
    {
       InitSlots();
      
       _dailyRewardView.StartCoroutine(UpdateCoroutine());
      
       RefreshUi();
       SubscribeButtons();
    }

   private void InitSlots()
   {
       _slots = new List<ContainerSlotRewardView>();

       for (var i = 0; i < _dailyRewardView.Rewards.Count; i++)
       {
           var reward = _dailyRewardView.Rewards[i];
           var instanceSlot = Object.Instantiate(_dailyRewardView.ContainerSlotRewardView,
               _dailyRewardView.MountRootSlotsReward, false);
           
           instanceSlot.SetData(reward,i+1,false);
           _slots.Add(instanceSlot);
           
       }
   }

   private IEnumerator UpdateCoroutine()
   {
       while (true)
       {
           Update();
           yield return new WaitForSeconds(1);
       }
   }

   private void Update()
   {
       _rewardReceived = false;

       if (_dailyRewardView.TimeGetReward.HasValue)
       {
           var timeSpan = DateTime.UtcNow - _dailyRewardView.TimeGetReward.Value;

           if (timeSpan.Seconds > _dailyRewardView.TimeDeadline)
           {
               _dailyRewardView.TimeGetReward = null;
               _dailyRewardView.CurrentActiveSlot = 0;
               _dailyRewardView.ProgressBar.value = 0;
           }
           else if (timeSpan.Seconds < _dailyRewardView.TimeCooldown)
           {
               _rewardReceived = true;
           }
       }

       RefreshUi();
   }

   private void RefreshUi()
   {
       _dailyRewardView.GetRewardButton.interactable = !_rewardReceived;

       if (!_rewardReceived)
       {
           _dailyRewardView.TimerNewReward.text = "Reward already received";
       }
       else
       {
           if (_dailyRewardView.TimeGetReward != null)
           {
               var nextClaimTime = _dailyRewardView.TimeGetReward.Value.AddSeconds(_dailyRewardView.TimeCooldown);
               var currentClaimCooldown = nextClaimTime - DateTime.UtcNow;
               var timeGetReward = $"{currentClaimCooldown.Days:D2} days {currentClaimCooldown.Hours:D2}:{currentClaimCooldown.Minutes:D2}:{currentClaimCooldown.Seconds:D2}";
      
               _dailyRewardView.TimerNewReward.text = $"Time to get the next reward: {timeGetReward}";

               _dailyRewardView.ProgressBar.value = (_dailyRewardView.CurrentActiveSlot% _numOfRewardsInRow)+(currentClaimCooldown.Seconds / 86400);
           }
       }

       for (var i = 0; i < _slots.Count; i++)
           _slots[i].SetData(_dailyRewardView.Rewards[i],i + 1, i <= _dailyRewardView.CurrentActiveSlot);
   }

   private void SubscribeButtons()
   {
       _dailyRewardView.GetRewardButton.onClick.AddListener(ClaimReward);
       _dailyRewardView.ResetButton.onClick.AddListener(ResetReward);
   }

   private void ClaimReward()
   {
       if (_rewardReceived)
           return;

       var reward = _dailyRewardView.Rewards[_dailyRewardView.CurrentActiveSlot];

       switch (reward.RewardType)
       {
           case RewardType.Wood:
               CurrencyView.Instance.AddWood(reward.Count);
               break;
           case RewardType.Diamond:
               CurrencyView.Instance.AddDiamond(reward.Count);
               break;
           default:
               throw new ArgumentOutOfRangeException();
       }

       _dailyRewardView.TimeGetReward = DateTime.UtcNow;
       _dailyRewardView.CurrentActiveSlot = (_dailyRewardView.CurrentActiveSlot + 1) % _dailyRewardView.Rewards.Count;
       
       

       Update();
   }

   private void ResetReward()
   {
       _dailyRewardView.TimeGetReward = null;
       _dailyRewardView.CurrentActiveSlot = 0;
       _dailyRewardView.ProgressBar.value = 0;
   }
}
