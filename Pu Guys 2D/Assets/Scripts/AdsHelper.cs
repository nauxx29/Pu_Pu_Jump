using System;

public class AdsHelper : MonoSingleton<AdsHelper>
{
    public Action successCallback;
    public Action failureCallback;

    public void OnTryShowRv(string placement, Action rvReward, Action failCallback = null)
    {
        successCallback = rvReward;
        failureCallback = failCallback;

        if (!IronSource.Agent.isRewardedVideoAvailable())
        {
            failCallback?.Invoke();
        }

        IronSource.Agent.showRewardedVideo(placement);
    }

    public void SuccessCallback()
    {
        successCallback?.Invoke();
    }

    public void FailureCallback()
    {
        failureCallback?.Invoke();
    }

}
