// Copyright (C) 2020 Google LLC
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
using System.Collections.Generic;

using GoogleMobileAds;
using GoogleMobileAds.Common;

namespace GoogleMobileAds.Api
{
    /// <summary>
    /// Rewarded interstitial ads can serve without requiring the user to opt-in to viewing.
    /// At any point during the experience, the user can decide to skip the ad.
    /// </summary>
    public class RewardedInterstitialAd
    {
        /// <summary>
        /// Loads an rewarded interstitial ad.
        /// </summary>
        public static void Load(string adUnitId,
                                AdRequest request,
                                Action<RewardedInterstitialAd, LoadAdError> adLoadCallback)
        {
            var client = MobileAds.GetClientFactory().BuildRewardedInterstitialAdClient();
            client.CreateRewardedInterstitialAd();
            client.OnAdLoaded += (sender, args) =>
            {
                MobileAds.RaiseAdEvent(() =>
                {
                    if (adLoadCallback != null)
                    {
                        adLoadCallback(new RewardedInterstitialAd(client), null);
                    }
                });
            };

            client.OnAdFailedToLoad += (sender, args) =>
            {
                MobileAds.RaiseAdEvent(() =>
                {
                    if (adLoadCallback != null)
                    {
                        LoadAdError loadAdError = new LoadAdError(args.LoadAdErrorClient);
                        adLoadCallback(null, loadAdError);
                    }
                });
            };
            client.LoadAd(adUnitId, request);
        }

        /// <summary>
        /// Loads an rewarded interstitial ad.
        /// </summary>
        [Obsolete("Use RewardedInterstitialAd.Load().")]
        public static void LoadAd(string adUnitID,
            AdRequest request,
            Action<RewardedInterstitialAd, AdFailedToLoadEventArgs> adLoadCallback)
        {
            IRewardedInterstitialAdClient client = MobileAds.GetClientFactory().BuildRewardedInterstitialAdClient();
            client.CreateRewardedInterstitialAd();
            client.OnAdLoaded += (sender, args) =>
            {
                MobileAds.RaiseAdEvent(() =>
                {
                    if (adLoadCallback != null)
                    {
                        adLoadCallback(new RewardedInterstitialAd(client), null);
                    }
                });
            };

            client.OnAdFailedToLoad += (sender, args) =>
            {
                MobileAds.RaiseAdEvent(() =>
                {
                    if (adLoadCallback != null)
                    {
                        LoadAdError loadAdError = new LoadAdError(args.LoadAdErrorClient);
                        adLoadCallback(null, new AdFailedToLoadEventArgs()
                        {
                            LoadAdError = loadAdError
                        });
                    }
                });
            };

            client.LoadAd(adUnitID, request);
        }

        /// <summary>
        /// Raised when the ad is estimated to have earned money.
        /// </summary>
        public event Action<AdValue> OnAdPaid;

        /// <summary>
        /// Raised when an ad is clicked.
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
        /// On iOS, this event is only raised when an ad opens an overlay, not when opening a new
        /// application such as Safari or the App Store,
        /// </summary>
        public event Action OnAdFullScreenContentClosed;

        /// <summary>
        /// Raised when the ad failed to open full screen content.
        /// </summary>
        public event Action<AdError> OnAdFullScreenContentFailed;

        /// <summary>
        /// Raised when the ad is estimated to have earned money.
        /// </summary>
        [Obsolete("Use OnAdPaid.")]
        public event EventHandler<AdValueEventArgs> OnPaidEvent;

        /// <summary>
        /// Raised when the ad failed to open full screen content.
        /// </summary>
        [Obsolete("Use OnFullScreenAdFailed.")]
        public event EventHandler<AdErrorEventArgs> OnAdFailedToPresentFullScreenContent;

        /// <summary>
        /// Raised when an ad opened full screen content.
        /// </summary>
        [Obsolete("Use OnFullScreenAdOpened.")]
        public event EventHandler<EventArgs> OnAdDidPresentFullScreenContent;

        /// <summary>
        /// Raised when the ad closed full screen content.
        /// On iOS, this event is only raised when an ad opens an overlay, not when opening a new
        /// application such as Safari or the App Store,
        /// </summary>
        [Obsolete("Use OnFullScreenAdClosed.")]
        public event EventHandler<EventArgs> OnAdDidDismissFullScreenContent;

        /// <summary>
        /// Raised when an impression is recorded for an ad.
        /// </summary>
        [Obsolete("Use OnAdImpressionRecorded.")]
        public event EventHandler<EventArgs> OnAdDidRecordImpression;

        private IRewardedInterstitialAdClient _client;
        private bool _isLoaded;
        private Action<Reward> _userRewardEarnedCallback;

        private RewardedInterstitialAd(IRewardedInterstitialAdClient client)
        {
            _isLoaded = true;
            _client = client;

            _client.OnAdClicked += () =>
            {
                MobileAds.RaiseAdEvent(() =>
                {
                    if (OnAdClicked != null)
                    {
                        OnAdClicked();
                    }
                });
            };

            _client.OnAdDidDismissFullScreenContent += (sender, args) =>
            {
                MobileAds.RaiseAdEvent(() =>
                {
                    if (OnAdDidDismissFullScreenContent != null)
                    {
                        OnAdDidDismissFullScreenContent(this, args);
                    }
                    if (OnAdFullScreenContentClosed != null)
                    {
                        OnAdFullScreenContentClosed();
                    }
                });
            };

            _client.OnAdDidPresentFullScreenContent += (sender, args) =>
            {
                MobileAds.RaiseAdEvent(() =>
                {
                    if (OnAdDidPresentFullScreenContent != null)
                    {
                        OnAdDidPresentFullScreenContent(this, args);
                    }
                    if (OnAdFullScreenContentOpened != null)
                    {
                        OnAdFullScreenContentOpened();
                    }
                });
            };

            _client.OnAdDidRecordImpression += (sender, args) =>
            {
                MobileAds.RaiseAdEvent(() =>
                {
                    if (OnAdDidRecordImpression != null)
                    {
                        OnAdDidRecordImpression(this, args);
                    }
                    if (OnAdImpressionRecorded != null)
                    {
                        OnAdImpressionRecorded();
                    }
                });
            };
            _client.OnAdFailedToPresentFullScreenContent += (sender, error) =>
            {
                MobileAds.RaiseAdEvent(() =>
                {
                    if (OnAdFailedToPresentFullScreenContent != null)
                    {
                        OnAdFailedToPresentFullScreenContent(this,
                            new AdErrorEventArgs { AdError = new AdError(error.AdErrorClient) });
                    }
                    if (OnAdFullScreenContentFailed != null)
                    {
                        OnAdFullScreenContentFailed(new AdError(error.AdErrorClient));
                    }
                });
            };
            _client.OnPaidEvent += (sender, args) =>
            {
                MobileAds.RaiseAdEvent(() =>
                {
                    if (OnPaidEvent != null)
                    {
                        OnPaidEvent(this, args);
                    }
                    if (OnAdPaid != null)
                    {
                        OnAdPaid(args.AdValue);
                    }
                });
            };
            _client.OnUserEarnedReward += (sender, args) =>
            {
                MobileAds.RaiseAdEvent(() =>
                {
                    if (_userRewardEarnedCallback != null)
                    {
                        _userRewardEarnedCallback(args);
                        _userRewardEarnedCallback = null;
                    }
                });
            };
        }

        /// <summary>
        /// Determines if the ad can be showen.
        /// </summary>
        public bool IsLoaded()
        {
            return _client != null && _isLoaded;
        }


        /// <summary>
        /// Shows a rewarded interstitial ad.
        /// </summary>
        /// <param name="userRewardEarnedCallback">
        /// An action to be raised when the user earns a reward.
        /// </param>
        public void Show(Action<Reward> userEarnedRewardCallback)
        {
            if (_client != null && _isLoaded)
            {
                _userRewardEarnedCallback = userEarnedRewardCallback;
                _client.Show();
                _isLoaded = false;
            }
        }

        /// <summary>
        /// Sets the server side verification options
        /// </summary>
        public void SetServerSideVerificationOptions(ServerSideVerificationOptions options)
        {
            if (_client != null)
            {
                _client.SetServerSideVerificationOptions(options);
            }
        }

        /// <summary>
        /// The reward item for the loaded rewarded interstital ad.
        /// </summary>
        public Reward GetRewardItem()
        {
            return _client == null ? null : _client.GetRewardItem();
        }

        /// <summary>
        /// Destroys the ad.
        /// </summary>
        public void Destroy()
        {
            if (_client != null)
            {
                _client.DestroyRewardedInterstitialAd();
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
    }
}
