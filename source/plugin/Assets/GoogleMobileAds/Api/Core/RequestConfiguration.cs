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

using System.Collections.Generic;

namespace GoogleMobileAds.Api
{
    /// <summary>
    /// Global configuration that will be used for every <see cref="AdRequest"/>
    /// </summary>
    public class RequestConfiguration
    {
        /// <summary>
        /// Sets a maximum ad content rating. AdMob ads returned for these requests have
        /// a content rating at or below the level set.
        /// </summary>
        public MaxAdContentRating MaxAdContentRating { get; private set; }

        /// <summary>
        /// Allows apps that have opted into the Designed for Families program to specify whether
        /// a given ad request should return Designed for Families-compliant ads.
        /// </summary>
        public TagForChildDirectedTreatment? TagForChildDirectedTreatment { get; private set; }

        /// <summary>
        /// Indicates the publisher specified that the ad request should receive treatment for
        /// users in the European Economic Area(EEA) under the age of consent.
        /// </summary>
        public TagForUnderAgeOfConsent? TagForUnderAgeOfConsent { get; private set; }

        /// <summary>
        /// The test device IDs corresponding to test device which will always request
        /// test ads. Returns an empty list if test device IDs were not previously set.
        /// </summary>
        public List<string> TestDeviceIds { get; private set; }

        /// <summary>
        /// Controls whether the Google Mobile Ads SDK Same App Key is enabled.
        /// The value set persists across app sessions. The key is enabled by default.
        /// </summary>
        public bool? SameAppKeyEnabled { get; private set; }

        private RequestConfiguration(Builder builder)
        {
            MaxAdContentRating = builder.MaxAdContentRating;
            TagForChildDirectedTreatment = builder.TagForChildDirectedTreatment;
            TagForUnderAgeOfConsent = builder.TagForUnderAgeOfConsent;
            TestDeviceIds = builder.TestDeviceIds;
            SameAppKeyEnabled = builder.SameAppKeyEnabled;
        }

        public Builder ToBuilder()
        {
          Builder builder = new Builder()
              .SetMaxAdContentRating(this.MaxAdContentRating)
              .SetTagForChildDirectedTreatment(this.TagForChildDirectedTreatment)
              .SetTagForUnderAgeOfConsent(this.TagForUnderAgeOfConsent)
              .SetTestDeviceIds(this.TestDeviceIds);
          if (this.SameAppKeyEnabled.HasValue)
          {
              builder.SetSameAppKeyEnabled(this.SameAppKeyEnabled.Value);
          }
          return builder;
        }

        public class Builder
        {
            internal MaxAdContentRating MaxAdContentRating { get; private set; }
            internal TagForChildDirectedTreatment? TagForChildDirectedTreatment { get; private set; }
            internal TagForUnderAgeOfConsent? TagForUnderAgeOfConsent { get; private set; }
            internal List<string> TestDeviceIds { get; private set; }
            internal bool? SameAppKeyEnabled { get; private set; }

            public Builder()
            {
                MaxAdContentRating = null;
                TagForChildDirectedTreatment = null;
                TagForUnderAgeOfConsent = null;
                TestDeviceIds = new List<string>();
                SameAppKeyEnabled = null;
            }

            /// <summary>
            /// The maximum ad content rating. All Google ads will have this content rating or lower.
            /// </summary>
            public Builder SetMaxAdContentRating(MaxAdContentRating maxAdContentRating)
            {
                this.MaxAdContentRating = maxAdContentRating;
                return this;
            }

            /// <summary>
            /// Allows apps that have opted into the Designed for Families program to specify whether
            /// a given ad request should return Designed for Families-compliant ads.
            /// </summary>
            public Builder SetTagForChildDirectedTreatment(
                TagForChildDirectedTreatment? tagForChildDirectedTreatment)
            {
                TagForChildDirectedTreatment = tagForChildDirectedTreatment;
                return this;
            }

            /// <summary>
            /// Indicates the publisher specified that the ad request should receive treatment for
            /// users in the European Economic Area(EEA) under the age of consent.
            /// </summary>
            public Builder SetTagForUnderAgeOfConsent(
                TagForUnderAgeOfConsent? tagForUnderAgeOfConsent)
            {
                TagForUnderAgeOfConsent = tagForUnderAgeOfConsent;
                return this;
            }

            /// <summary>
            /// The test device IDs corresponding to test device which will always request
            /// test ads. Returns an empty list if test device IDs were not previously set.
            /// </summary>
            public Builder SetTestDeviceIds(List<string> testDeviceIds)
            {
                TestDeviceIds = testDeviceIds;
                return this;
            }

            /// <summary>
            /// Controls whether the Google Mobile Ads SDK Same App Key is enabled.
            /// The value set persists across app sessions. The key is enabled by default.
            /// </summary>
            public Builder SetSameAppKeyEnabled(bool enabled)
            {
              SameAppKeyEnabled = enabled;
              return this;
            }

            public RequestConfiguration build()
            {
                return new RequestConfiguration(this);
            }

        }

    }
}
