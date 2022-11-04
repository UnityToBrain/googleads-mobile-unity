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
    /// Banner views occupy a spot within an app's layout. They stay on screen while users are
    /// interacting with the app, and can refresh automatically after a certain period of time
    /// </summary>
    public class BannerView
    {
        /// <summary>
        /// Raised when an ad is loaded into the banner view.
        /// </summary>
        public event Action OnBannerAdLoaded;

        /// <summary>
        /// Raised when an ad fails to load into the banner view.
        /// </summary>
        public event Action<LoadAdError> OnBannerAdLoadFailed;

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

        // These are the ad callback events that can be hooked into.
        [Obsolete("Use OnBannerAdLoaded.")]
        public event EventHandler<EventArgs> OnAdLoaded;
        [Obsolete("Use OnBannerAdLoadFailed.")]
        public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;
        [Obsolete("Use OnFullScreenAdOpened.")]
        public event EventHandler<EventArgs> OnAdOpening;
        [Obsolete("Use OnFullScreenAdClosed.")]
        public event EventHandler<EventArgs> OnAdClosed;
        // Called when an ad is estimated to have earned money.
        [Obsolete("Use OnAdPaid.")]
        public event EventHandler<AdValueEventArgs> OnPaidEvent;

        /// <summary>
        /// Returns true if Destroy() has been called.
        /// </summary>
        public bool IsDestroyed { get { return _client == null; } }

        private IBannerClient _client;

        /// <summary>
        /// Creates a banner view and adds it to the view hierarchy.
        /// </summary>
        public BannerView(string adUnitId, AdSize adSize, AdPosition position)
        {
            _client = MobileAds.GetClientFactory().BuildBannerClient();
            _client.CreateBannerView(adUnitId, adSize, position);

            ConfigureBannerEvents();
        }

        /// <summary>
        /// Creates a banner view with a custom position.
        /// </summary>
        public BannerView(string adUnitId, AdSize adSize, int x, int y)
        {
            _client = MobileAds.GetClientFactory().BuildBannerClient();
            _client.CreateBannerView(adUnitId, adSize, x, y);

            ConfigureBannerEvents();
        }

        /// <summary>
        /// Destroys the ad client.
        /// </summary>
        public void Destroy()
        {
            if (_client != null)
            {
                _client.DestroyBannerView();
                _client = null;
            }
        }

        /// <summary>
        /// Returns the ad request response info.
        /// </summary>
        public ResponseInfo GetResponseInfo()
        {
            return _client != null ? new ResponseInfo(_client.GetResponseInfoClient()) : null;
        }

        /// <summary>
        /// Returns the height of the banner view in pixels.
        /// </summary>
        public float GetHeightInPixels()
        {
            return _client != null ? _client.GetHeightInPixels() : 0;
        }

        /// <summary>
        /// Returns the width of the banner view in pixels.
        /// </summary>
        public float GetWidthInPixels()
        {
            return _client != null ? _client.GetWidthInPixels() : 0;
        }

        /// <summary>
        /// Loads an into the banner view.
        /// </summary>
        public void LoadAd(AdRequest request)
        {
            if (_client != null)
            {
                _client.LoadAd(request);
            }
        }

        /// <summary>
        /// Shows the banner view from the screen.
        /// </summary>
        public void Show()
        {
            if (_client != null)
            {
                _client.ShowBannerView();
            }
        }

        /// <summary>
        /// Hides the banner view from the screen.
        /// </summary>
        public void Hide()
        {
            if (_client != null)
            {
                _client.HideBannerView();
            }
        }

        /// <summary>
        /// Set the position of the BannerView using standard position.
        /// </summary>
        public void SetPosition(AdPosition adPosition)
        {
            if (_client != null)
            {
                _client.SetPosition(adPosition);
            }
        }

        /// <summary>
        /// Set the position of the banner view using custom position.
        /// </summary>
        public void SetPosition(int x, int y)
        {
            if (_client != null)
            {
                _client.SetPosition(x, y);
            }
        }

        private void ConfigureBannerEvents()
        {
            _client.OnAdLoaded += (sender, args) =>
            {
                if (this.OnAdLoaded != null)
                {
                    this.OnAdLoaded(this, args);
                }
                if (OnBannerAdLoaded != null)
                {
                    OnBannerAdLoaded();
                }
            };
            _client.OnAdFailedToLoad += (sender, args) =>
            {
                if (this.OnAdFailedToLoad != null)
                {
                    LoadAdError loadAdError = new LoadAdError(args.LoadAdErrorClient);
                    this.OnAdFailedToLoad(this, new AdFailedToLoadEventArgs()
                    {
                        LoadAdError = loadAdError
                    });
                }
                if (OnBannerAdLoadFailed != null)
                {
                    LoadAdError loadAdError = new LoadAdError(args.LoadAdErrorClient);
                    OnBannerAdLoadFailed(loadAdError);
                }
            };

            _client.OnAdOpening += (sender, args) =>
            {
                if (this.OnAdOpening != null)
                {
                    this.OnAdOpening(this, args);
                }
                if (OnAdFullScreenContentOpened != null)
                {
                    OnAdFullScreenContentOpened();
                }
            };

            _client.OnAdClosed += (sender, args) =>
            {
                if (this.OnAdClosed != null)
                {
                    this.OnAdClosed(this, args);
                }
                if (OnAdFullScreenContentClosed != null)
                {
                    OnAdFullScreenContentClosed();
                }
            };

            _client.OnPaidEvent += (sender, args) =>
            {
                if (this.OnPaidEvent != null)
                {
                    this.OnPaidEvent(this, args);
                }
                if (OnAdPaid != null)
                {
                    OnAdPaid(args.AdValue);
                }
            };
            _client.OnAdClicked += () =>
            {
                if (OnAdClicked != null)
                {
                    OnAdClicked();
                }
            };
            _client.OnAdImpressionRecorded += () =>
            {
                if (OnAdImpressionRecorded != null)
                {
                    OnAdImpressionRecorded();
                }
            };
        }
    }
}
