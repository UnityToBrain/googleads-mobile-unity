// Copyright (C) 2021 Google LLC
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

using UnityEngine;

using GoogleMobileAds.Common;

namespace GoogleMobileAds.Api
{
    /// <summary>
    /// App open ads are used to display ads when users enter your app. An AppOpenAd object
    /// contains all the data necessary to display an ad. Unlike interstitial ads, app open ads
    /// make it easy to provide an app branding so that users understand the context in which they
    /// see the ad.
    /// </summary>
    public class AppOpenAd
    {
        /// <summary>
        /// Loads an app open ad.
        /// </summary>
        public static void Load(string adUnitId,
                                ScreenOrientation orientation,
                                AdRequest request,
                                Action<AppOpenAd, LoadAdError> adLoadCallback)
        {
            var client = MobileAds.GetClientFactory().BuildAppOpenAdClient();
            client.CreateAppOpenAd();
            client.OnAdLoaded += (sender, args) =>
            {
                if (adLoadCallback != null)
                {
                    adLoadCallback(new AppOpenAd(client), null);
                }
            };

            client.OnAdFailedToLoad += (sender, args) =>
            {
                if (adLoadCallback != null)
                {
                    LoadAdError loadAdError = new LoadAdError(args.LoadAdErrorClient);
                    adLoadCallback(null, loadAdError);
                }
            };
            client.LoadAd(adUnitId, request, orientation);
        }

        // Loads a new app open ad.
        [Obsolete("Use AppOpenAd.Load().")]
        public static void LoadAd(string adUnitID,
            ScreenOrientation orientation,
            AdRequest request,
            Action<AppOpenAd, AdFailedToLoadEventArgs> adLoadCallback)
        {
            IAppOpenAdClient client = MobileAds.GetClientFactory().BuildAppOpenAdClient();
            client.CreateAppOpenAd();

            client.OnAdLoaded += (sender, args) =>
            {
                if (adLoadCallback != null)
                {
                    adLoadCallback(new AppOpenAd(client), null);
                }
            };

            client.OnAdFailedToLoad += (sender, args) =>
            {
                if (adLoadCallback != null)
                {
                    LoadAdError loadAdError = new LoadAdError(args.LoadAdErrorClient);
                    adLoadCallback(null, new AdFailedToLoadEventArgs()
                    {
                        LoadAdError = loadAdError,
                    });
                }
            };

            client.LoadAd(adUnitID, request, orientation);
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

        // Called when the ad is estimated to have earned money.
        [Obsolete("Use OnAdPaid.")]
        public event EventHandler<AdValueEventArgs> OnPaidEvent;
        // Full screen content events.
        [Obsolete("Use OnFullScreenAdFailed.")]
        public event EventHandler<AdErrorEventArgs> OnAdFailedToPresentFullScreenContent;
        [Obsolete("Use OnFullScreenAdOpened.")]
        public event EventHandler<EventArgs> OnAdDidPresentFullScreenContent;
        [Obsolete("Use OnFullScreenAdClosed.")]
        public event EventHandler<EventArgs> OnAdDidDismissFullScreenContent;
        [Obsolete("Use OnAdImpressionRecorded.")]
        public event EventHandler<EventArgs> OnAdDidRecordImpression;

        private IAppOpenAdClient _client;
        private bool _isLoaded;

        private AppOpenAd(IAppOpenAdClient client)
        {
            _isLoaded = true;
            _client = client;

            _client.OnAdClicked += () =>
            {
                if (OnAdClicked != null)
                {
                    OnAdClicked();
                }
            };

            _client.OnAdDidDismissFullScreenContent += (sender, args) =>
            {
                if (OnAdDidDismissFullScreenContent != null)
                {
                    OnAdDidDismissFullScreenContent(this, args);
                }
                if (OnAdFullScreenContentClosed != null)
                {
                    OnAdFullScreenContentClosed();
                }
            };

            _client.OnAdDidPresentFullScreenContent += (sender, args) =>
            {
                if (OnAdDidPresentFullScreenContent != null)
                {
                    OnAdDidPresentFullScreenContent(this, args);
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
                if (OnAdFailedToPresentFullScreenContent != null)
                {
                    OnAdFailedToPresentFullScreenContent(this,
                        new AdErrorEventArgs { AdError = new AdError(error.AdErrorClient) });
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

        /// <summary>
        /// Determines if the ad can be showen.
        /// </summary>
        public bool IsLoaded()
        {
            return _client != null && _isLoaded;
        }

        /// <summary>
        /// Shows a app open ad.
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
                _client.DestroyAppOpenAd();
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
