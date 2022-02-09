using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class ads : MonoBehaviour
{
    public GameObject adsButtonControl;
    public GameObject hercules;
    private RewardedAd rewardedAd;
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        requestAds();
        
    }
    private void requestAds()
    {
        string adUnitId;
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(adUnitId);
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }
    public void adsButton()
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        adsButtonControl.SetActive(false);
        if (!hercules.GetComponent<Bandit>().falling)
        {
            hercules.GetComponent<Bandit>().health = 100;
            hercules.GetComponent<Bandit>().SetSize(hercules.GetComponent<Bandit>().health * 0.01f);
        }
        else if(hercules.GetComponent<Bandit>().falling)
        {

            hercules.transform.position = new Vector3(hercules.transform.position.x - 16, 0, hercules.transform.position.z);
            hercules.GetComponent<Bandit>().health = 100;
            hercules.GetComponent<Bandit>().SetSize(hercules.GetComponent<Bandit>().health * 0.01f);
        }
    }
}
