using Firebase.Extensions;
using UnityEngine;
using Firebase;
using Firebase.Crashlytics;
using Firebase.Analytics;

public class AdsManager : MonoSingleton<AdsManager>
{
    public bool IsRvInit { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {

        void IronSourceInit()
        {
            IronSource.Agent.init("1e06a87e5", IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL);
            IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
            IronSource.Agent.shouldTrackNetworkState(true);
        }

        void AddAdsEvents()
        {
            //Add AdInfo Rewarded Video Events
            IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
            IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
            IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
            IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
            IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
            IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
            IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;
            IronSourceEvents.onImpressionDataReadyEvent += ImpressionDataReadyEvent;

        }

        FirebaseManager.Instance.FirebaseInit();
        IronSourceInit();
        AddAdsEvents();

    }

    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
        IronSource.Agent.validateIntegration();
    }
    
    private void SdkInitializationCompletedEvent() 
    {
        Debug.Log("##### IronSource Init OK #####");
    }


    /************* RewardedVideo AdInfo Delegates *************/
    // Indicates that there¡¦s an available ad.
    // The adInfo object includes information about the ad that was loaded successfully
    // This replaces the RewardedVideoAvailabilityChangedEvent(true) event
    protected void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
        if (UiManager.Instance != null)
        {
            UiManager.Instance.SetReviveBtn(true);
        }
        else
        {
            IsRvInit = true;
        }
    }
    // Indicates that no ads are available to be displayed
    // This replaces the RewardedVideoAvailabilityChangedEvent(false) event
    protected void RewardedVideoOnAdUnavailable()
    {
        UiManager.Instance.SetReviveBtn(false);
    }
    // The Rewarded Video ad view has opened. Your activity will loose focus.
    protected void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
        UiManager.Instance.OnRvToggleMusic(true);
    }
    // The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
    protected void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        UiManager.Instance.OnRvToggleMusic(false);
    }
    // The user completed to watch the video, and should be rewarded.
    // The placement parameter will include the reward data.
    // When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
    protected void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
#if DEBUG
        Debug.Log($"Ads success, revenue : {adInfo.revenue}, {adInfo.adNetwork}, {adInfo.segmentName}");
#endif
        AdsHelper.Instance.SuccessCallback();
    }
    // The rewarded video ad was failed to show.
    protected void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
    {
        AdsHelper.Instance.FailureCallback();
    }
    // Invoked when the video ad was clicked.
    // This callback is not supported by all networks, and we recommend using it only if
    // it¡¦s supported by all networks you included in your build.
    protected void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {

    }

    
    private void ImpressionDataReadyEvent(IronSourceImpressionData impressionData) 
    {
        ImpressionSuccessEvent(impressionData);
    }

    #region Firebase
    private void ImpressionSuccessEvent(IronSourceImpressionData infoData)
    {
        if (infoData != null)
        {
            Parameter[] AdParameters = 
            {
                new Parameter("ad_platform", "ironSource"),
                new Parameter("ad_source", infoData.adNetwork),
                new Parameter("ad_unit_name", infoData.adUnit),
                new Parameter("ad_format", infoData.instanceName),
                new Parameter("currency","USD"),
                new Parameter("value", infoData.revenue.Value)
            }; 
            FirebaseAnalytics.LogEvent("Ad_Impression", AdParameters);
        }
    }
    #endregion
}
