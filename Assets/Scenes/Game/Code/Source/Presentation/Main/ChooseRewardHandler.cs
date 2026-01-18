using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class ChooseRewardHandler : MonoBehaviour
{
    public RewardSelectorGameObject rewardSelectorGameObject;

    [Inject]
    private readonly CalculateNextIndexUsecase calculateNextIndexUsecase;

    [Inject]
    private readonly RewardManager rewardManager;

    private Func<GameState> getGameStateFunc;
    private Action selectRewardCallBack;
    private int rewardIndex;
    private List<Reward> rewardsToChooseFrom;

    // Update is called once per frame
    void Update()
    {
        rewardSelectorGameObject.Appear(getGameStateFunc() is GameState.ChooseRewardState);
    }
    public void Init(
    Func<GameState> getGameStateFunc,
    Action selectRewardCallBack
    )
    {
        this.getGameStateFunc = getGameStateFunc;
        this.selectRewardCallBack = selectRewardCallBack;
        rewardSelectorGameObject.rewardSelectedAction = RewardSelectedAction;
        rewardSelectorGameObject.rewardHoverAction = RewardHoverAction;
    }

    public void SetUpRewardSelection()
    {
        Reset();
        rewardsToChooseFrom = rewardManager.Present();
        rewardSelectorGameObject.SetUp(rewardsToChooseFrom);
    }

    public void TargetNewReward(Key key)
    {
        rewardIndex = calculateNextIndexUsecase.Invoke(
            key == Key.RightArrow,
            rewardIndex,
            rewardsToChooseFrom.Count
        );
    }

    public Reward PickReward()
    {
        Reward reward = rewardsToChooseFrom[rewardIndex];
        rewardManager.Pick(reward);
        return reward;
    }

    public void UpdateUIState()
    {
        rewardSelectorGameObject.UpdateState(rewardsToChooseFrom[rewardIndex]);
        List<string> rewardsDisplay = rewardsToChooseFrom.Select(r =>
        {
            (int, int) pair = rewardManager.GetCurrentAndFutureState(r);
            return $"{r.Title}: {pair.Item1}=>{pair.Item2}";
        }).ToList();
        Debug.Log($"Picking reward: {rewardIndex}\n{string.Join(",", rewardsDisplay)}");
    }

    public void Reset()
    {
        rewardIndex = 0;
    }

    private void RewardSelectedAction(Reward reward)
    {
        TargetNewReward(reward);
        selectRewardCallBack();
        UpdateUIState();
    }

    private void RewardHoverAction(Reward reward)
    {
        int originalIndex = rewardIndex;
        TargetNewReward(reward);
        if (originalIndex != rewardIndex)
        {
            UpdateUIState();
        }
    }

    private void TargetNewReward(Reward reward)
    {
        int index = rewardsToChooseFrom.IndexOf(reward);
        if (index < 0)
        {
            return;
        }
        rewardIndex = index;
    }
}
