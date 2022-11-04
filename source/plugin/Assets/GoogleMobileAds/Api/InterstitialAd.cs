// Copyright (C) 2015 Google, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using GoogleMobileAds;
using GoogleMobileAds.Common;

namespace GoogleMobileAds.Api
{
    /// <summary>
    /// A full page ad experience at natural transition points such as a page change, an app launch.
    /// Interstitials use a close button that removes the ad from the user's experience.
    /// </summary>
    public class InterstitialAd
    {
        /// <summary>
        /// Loads an interstitial ad.
        /// </summary>
        public static void Load(string adUnitId,
                                  AdRequest request,
                                  Action<InterstitialAd, LoadAdError> callback)
        {
            var loader = MobileAds.GetClientFactory().BuildInterstitialClient();
            loader.CreateInterstitialAd();
            loader.OnAdLoaded += (sender, args) =>
            {
                if (callback != null)
                {
                    callback(new InterstitialAd(loader), null);
                }
            };
            loader.OnAdFailedToLoad += (sender, error) =>
            {
                if (callback != null)
                {
                    callback(null, new LoadAdError(error.LoadAdErrorClient));
                }
            };
            loader.LoadAd(adUnitId, request);
        }

        /// <summary>
        /// Raised when the ad is estimated to have earned money.
        /// </summary>
        public event Action<AdValue> OnAdPaid;

        /// <summary>
        /// Raised when a click is recorded for an ad.
        /// </summary>
        public event Action OnAdClicked;

        /// <summary>
        /// Raised when an impression is recorded for an ad.
        /// </summary>
        public event Action OnAdImpressionRecorded;

        /// <summary>
        /// Raised when an ad opened full screen content.
        /// </summary>
        public event Action OnAdFullScreenContentOpened;

        /// <summary>
        /// Raised when the ad closed full screen content.
        /// On iOS this does not include ads which open (safari) web browser links.
        /// </summary>
        public event Action OnAdFullScreenContentClosed;

        /// <summary>
        /// Raised when the ad failed to open full screen content.
        /// </summary>
        public event Action<AdError> OnAdFullScreenContentFailed;

        [Obsolete("Use InterstitialAd.Load().")]
        public event EventHandler<EventArgs> OnAdLoaded;
        [Obsolete("Use InterstitialAd.Load().")]
        public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;
        [Obsolete("Use OnFullScreenAdOpened.")]
        public event EventHandler<EventArgs> OnAdOpening;
        [Obsolete("Use OnFullScreenAdClosed.")]
        public event EventHandler<EventArgs> OnAdClosed;
        [Obsolete("Use OnAdImpressionRecorded.")]
        public event EventHandler<AdErrorEventArgs> OnAdFailedToShow;
        [Obsolete("Use OnAdImpressionRecorded.")]
        public event EventHandler<EventArgs> OnAdDidRecordImpression;
        [Obsolete("Use OnAdPaid.")]
        public event EventHandler<AdValueEventArgs> OnPaidEvent;

        private IInterstitialClient _client;
        private string _adUnitId;
        private bool _isLoaded;

        // Creates an InterstitialAd.
        [Obsolete("Use InterstitialAd.Load().")]
        public InterstitialAd(string adUnitId)
        {
            _adUnitId = adUnitId;
        }

        private InterstitialAd(IInterstitialClient client)
        {
            _client = client;
            _isLoaded = true;
            Init();
        }

        // Loads an InterstitialAd.
        [Obsolete("Use InterstitialAd.Load().")]
        public void LoadAd(AdRequest request)
        {
            _client = MobileAds.GetClientFactory().BuildInterstitialClient();
            _client.CreateInterstitialAd();
            _client.OnAdLoaded += (sender, args) =>
            {
                _isLoaded = true;
                Init();
                if (OnAdLoaded != null)
                {
                    OnAdLoaded(this, EventArgs.Empty);
                }
            };
            _client.OnAdFailedToLoad += (sender, error) =>
            {
                if (OnAdFailedToLoad != null)
                {
                    OnAdFailedToLoad(this, new AdFailedToLoadEventArgs
                    {
                        LoadAdError = new LoadAdError(error.LoadAdErrorClient)
                    });
                }
            };
            _client.LoadAd(_adUnitId, request);
        }

        /// <summary>
        /// Determines is the ad can be showen.
        /// </summary>
        public bool IsLoaded()
        {
            return _client != null && _isLoaded;
        }

        /// <summary>
        /// Shows the ad.
        /// </summary>
        public void Show()
        {
            if (_client != null && _isLoaded)
            {
                _client.Show();
                _isLoaded = false;
            }
        }

        /// <summary>
        /// Destroys the ad.
        /// </summary>
        public void Destroy()
        {
            if (_client != null)
            {
                _client.DestroyInterstitial();
                _isLoaded = false;
            }
        }

        /// <summary>
        /// Returns the ad request response info.
        /// </summary>
        public ResponseInfo GetResponseInfo()
        {
            return _client != null ? new ResponseInfo(_client.GetResponseInfoClient()) : null;
        }

        private void Init()
        {
            _client.OnAdClicked += () =>
            {
                if (OnAdClicked != null)
                {
                    OnAdClicked();
                }
            };

            _client.OnAdDidDismissFullScreenContent += (sender, args) =>
            {
                if (OnAdClosed != null)
                {
                    OnAdClosed(this, args);
                }
                if (OnAdFullScreenContentClosed != null)
                {
                    OnAdFullScreenContentClosed();
                }
            };

            _client.OnAdDidPresentFullScreenContent += (sender, args) =>
            {
                if (OnAdOpening != null)
                {
                    OnAdOpening(this, args);
                }
                if (OnAdFullScreenContentOpened != null)
                {
                    OnAdFullScreenContentOpened();
                }
            };

            _client.OnAdDidRecordImpression += (sender, args) =>
            {
                if (OnAdDidRecordImpression != null)
                {
                    OnAdDidRecordImpression(this, args);
                }
                if (OnAdImpressionRecorded != null)
                {
                    OnAdImpressionRecorded();
                }
            };
            _client.OnAdFailedToPresentFullScreenContent += (sender, error) =>
            {
                if (OnAdFailedToShow != null)
                {
                    OnAdFailedToShow(this,
                        new AdErrorEventArgs{ AdError = new AdError(error.AdErrorClient) });
                }
                if (OnAdFullScreenContentFailed != null)
                {
                    OnAdFullScreenContentFailed(new AdError(error.AdErrorClient));
                }
            };
            _client.OnPaidEvent += (sender, args) =>
            {
                if (OnPaidEvent != null)
                {
                    OnPaidEvent(this, args);
                }
                if (OnAdPaid != null)
                {
                    OnAdPaid(args.AdValue);
                }
            };
        }
    }
}
