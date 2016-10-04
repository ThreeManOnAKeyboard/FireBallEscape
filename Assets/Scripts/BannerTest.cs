using System;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public class BannerTest : MonoBehaviour
{
	public Text texta;

	// Use this for initialization
	void Start()
	{
		RequestBanner();
	}

	private void RequestBanner()
	{
#if UNITY_EDITOR
		string adUnitId = "unused";
#elif UNITY_ANDROID
	        string adUnitId = "ca-app-pub-5656091690425620/6467610798";
#elif UNITY_IPHONE
	        string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
#else
	        string adUnitId = "unexpected_platform";
#endif

		// Create a 320x50 banner at the top of the screen.
		BannerView bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the banner with the request.
		bannerView.LoadAd(request);

		bannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
	}

	public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		texta.text = args.Message;
	}
}
