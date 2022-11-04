using System;
using UnityEngine;
using GoogleMobileAds.Api;

namespace GoogleMobileAds.Sample
{
    /// <summary>
    /// Demonstrates how to use the Google Mobile Ads BannerView.
    /// </summary>
    /// <remarks>
    /// Before loading ads, have your app initialize the Mobile Ads SDK
    /// by calling MobileAds.Initialize();
    /// This needs to be done only once, ideally at app launch.
    /// <remarks>
    [AddComponentMenu("GoogleMobileAds/Samples/BannerViewController")]
    public class BannerViewController : MonoBehaviour
    {
        // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
        private string _adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
        private string _adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        private string _adUnitId = "unused";
#endif
        private BannerView _bannerView;

        // # Creating a banner view
        //
        // The first step in using a banner view is to create an instance
        // of a banner view in a C# script attached to a GameObject.
        // Below we have demonstrated three ways to load a banner view.

        /// <summary>
        /// Creates a 320x50 banner at top of the screen.
        /// </summary>
        public void CreateBannerView()
        {
            Debug.Log("Creating banner view");

            // If we already have a banner, destroy the old one.
            if(_bannerView != null)
            {
                DestroyAd();
            }

            // Create a 320x50 banner at top of the screen
            _bannerView = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Top);

            // listen to events the banner may raise.
            ListenToAdEvents();
        }

        /// <summary>
        /// Creates a 320x50 banner view at the top left of the screen.
        /// </summary>
        public void CreateCustomPositionBannerView()
        {
            Debug.Log("Creating custom banner view");

            // If we already have a banner, destroy the old one.
            if(_bannerView != null)
            {
                DestroyAd();
            }

            // The top-left corner of the `BannerView` will be positioned at the x and y values
            // passed to the constructor, where the origin is the top-left of the screen.

            // Create a 320x50 banner ad at coordinate (0,50) on screen.
            _bannerView = new BannerView(_adUnitId, AdSize.Banner, 0, 50);

            // listen to events the banner may raise.
            ListenToAdEvents();
        }

        /// <summary>
        /// Creates a 250x250 banner view at the bottom of the screen.
        /// </summary>
        public void CreateCustomSizeBannerView()
        {
            Debug.Log("Creating custom banner view");

            // If we already have a banner, destroy the old one.
            if(_bannerView != null)
            {
                DestroyAd();
            }

            // Use the AdSize argument to set a custom size for the ad.
            AdSize adSize = new AdSize(250, 250);
            _bannerView = new BannerView(_adUnitId, adSize, AdPosition.Bottom);

            // listen to events the banner may raise.
            ListenToAdEvents();
        }

        // # Loading a banner ad
        //
        // The second step in using the banner view is to create an AdRequest
        // and pass it to the LoadAd method.

        /// <summary>
        /// Creates the banner view and loads a banner ad.
        /// </summary>
        public void LoadAd()
        {
            // create an instance of a banner view first.
            if(_bannerView == null)
            {
                CreateBannerView();
            }
            // create our request used to load the ad.
            var adRequest = new AdRequest.Builder()
                .AddKeyword("unity-admob-sample")
                .Build();

            // send the request to load the ad.
            Debug.Log("Loading banner ad.");
            _bannerView.LoadAd(adRequest);
        }

        /// <summary>
        /// Shows the ad.
        /// </summary>
        public void ShowAd()
        {
            if (_bannerView != null)
            {
                Debug.Log("Showing banner view.");
                _bannerView.Show();
            }
        }

        /// <summary>
        /// Hides the ad.
        /// </summary>
        public void HideAd()
        {
            if (_bannerView != null)
            {
                Debug.Log("Hiding banner view.");
                _bannerView.Hide();
            }
        }

        /// <summary>
        /// Destroys the ad.
        /// </summary>
        public void DestroyAd()
        {
            // When you are finished with a BannerView,  make sure to call
            // the Destroy() method before dropping your reference to it:

            if (_bannerView != null)
            {
                Debug.Log("Destroying banner ad.");
                _bannerView.Destroy();
                _bannerView = null;
            }
        }

        // # Listen to banner ad events.
        //
        // To further customize the behavior of your ad, you can hook into a number of
        // events in the ad's lifecycle: loading, opening, closing, and so on. Listen for
        // these events by registering a delegate, as shown below.

        /// <summary>
        /// listen to events the banner may raise.
        /// </summary>
        private void ListenToAdEvents()
        {
            // Raised when  an ad is loaded into the banner view.
            _bannerView.OnBannerAdLoaded += OnBannerAdLoaded;
            // Raised when an ad fails to load into the banner view.
            _bannerView.OnBannerAdLoadFailed += OnBannerAdLoadFailed;
            // Raised when the ad is estimated to have earned money.
            _bannerView.OnAdPaid += OnAdPaid;
            // Raised when an impression is recorded for an ad.
            _bannerView.OnAdImpressionRecorded += OnAdImpressionRecorded;
            // Raised when a click is recorded for an ad.
            _bannerView.OnAdClicked += OnAdClickRecorded;
            // Raised when the ad closed full screen content.
            _bannerView.OnAdFullScreenContentClosed += OnAdFullScreenContentClosed;
            // Raised when an ad opened full screen content.
            _bannerView.OnAdFullScreenContentOpened += OnAdFullScreenContentOpened;
        }

        /// <summary>
        /// Raised when  an ad is loaded into the banner view.
        /// </summary>
        private void OnBannerAdLoaded()
        {
            Debug.Log("Banner view loaded an ad with response : " + _bannerView.GetResponseInfo());
        }

        /// <summary>
        /// Raised when an ad fails to load into the banner view.
        /// </summary>
        private void OnBannerAdLoadFailed(LoadAdError error)
        {
            // The OnBannerAdLoadFailed event contains special event arguments.
            // It passes an instance of LoadAdError with a Message describing the error:
            Debug.LogError("Banner view failed to load an ad with error : " + error);
        }

        /// <summary>
        /// Raised when the ad is estimated to have earned money.
        /// </summary>
        private void OnAdPaid(AdValue adValue)
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                              adValue.Value,
                              adValue.CurrencyCode));
        }

        /// <summary>
        /// Raised when an impression is recorded for an ad.
        /// </summary>
        private void OnAdImpressionRecorded()
        {
            Debug.Log("Banner view recorded an impression.");
        }

        /// <summary>
        /// Raised when a click is recorded for an ad.
        /// </summary>
        private void OnAdClickRecorded()
        {
            Debug.Log("Banner view was clicked.");
        }

        /// <summary>
        /// Raised when the ad closed full screen content.
        /// </summary>
        private void OnAdFullScreenContentOpened()
        {
            Debug.Log("Banner view full screen content opened.");
        }

        /// <summary>
        /// Raised when an ad opened full screen content.
        /// </summary>
        private void OnAdFullScreenContentClosed()
        {
            Debug.Log("Banner view full screen content closed.");
        }
    }
}
