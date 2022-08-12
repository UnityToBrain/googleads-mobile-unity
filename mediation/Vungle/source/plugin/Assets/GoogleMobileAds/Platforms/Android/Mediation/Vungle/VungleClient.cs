// Copyright 2018 Google LLC
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

#if UNITY_ANDROID

using System;
using UnityEngine;

using GoogleMobileAds.Api.Mediation.Vungle;
using GoogleMobileAds.Common.Mediation.Vungle;

namespace GoogleMobileAds.Android.Mediation.Vungle
{
    public class VungleClient : IVungleClient
    {
        private static VungleClient instance = new VungleClient();
        private VungleClient() {}

        private const string VUNGLE_CLASS_NAME = "com.vungle.warren.Vungle";
        private const string VUNGLE_CONSENT_ENUM_NAME = "com.vungle.warren.Vungle$Consent";

        public static VungleClient Instance
        {
            get
            {
                return instance;
            }
        }

        public void UpdateConsentStatus(VungleConsent consentStatus,
                                        String consentMessageVersion)
        {
            if (consentStatus == VungleConsent.UNKNOWN)
            {
                MonoBehaviour.print("[Vungle Plugin] Cannot call 'UpdateConsentStatus()' " +
                        "with unknown consent status.");
                return;
            }

            AndroidJavaObject vungleConsentStatusObject =
                    GetConsentStatusFromVungleConsent(consentStatus);
            AndroidJavaClass vungle = new AndroidJavaClass(VUNGLE_CLASS_NAME);
            vungle.CallStatic("updateConsentStatus",
                    vungleConsentStatusObject, consentMessageVersion);
        }

        public VungleConsent GetCurrentConsentStatus()
        {
            AndroidJavaClass vungle = new AndroidJavaClass(VUNGLE_CLASS_NAME);
            AndroidJavaObject consentStatus = vungle.CallStatic<AndroidJavaObject>("getConsentStatus");
            return GetVungleConsentFromConsentStatus(consentStatus);
        }

        public String GetCurrentConsentMessageVersion()
        {
            AndroidJavaClass vungle = new AndroidJavaClass(VUNGLE_CLASS_NAME);
            return vungle.CallStatic<String>("getConsentMessageVersion");
        }

        public void UpdateCCPAStatus(VungleConsent consentStatus)
        {
            if (consentStatus == VungleConsent.UNKNOWN)
            {
                MonoBehaviour.print("[Vungle Plugin] Cannot call 'UpdateCCPAStatus()' " +
                        "with unknown consent status.");
                return;
            }

            AndroidJavaObject vungleConsentStatusObject =
                    GetConsentStatusFromVungleConsent(consentStatus);
            AndroidJavaClass vungle = new AndroidJavaClass(VUNGLE_CLASS_NAME);
            vungle.CallStatic("updateCCPAStatus", vungleConsentStatusObject);
        }

        public VungleConsent GetCCPAStatus()
        {
            AndroidJavaClass vungle = new AndroidJavaClass(VUNGLE_CLASS_NAME);
            AndroidJavaObject consentStatus = vungle.CallStatic<AndroidJavaObject>("getCCPAStatus");
            return GetVungleConsentFromConsentStatus(consentStatus);
        }

        private AndroidJavaObject GetConsentStatusFromVungleConsent(VungleConsent vungleConsent)
        {
            AndroidJavaClass vungleConsentEnum = new AndroidJavaClass(VUNGLE_CONSENT_ENUM_NAME);
            switch (vungleConsent)
            {
                case VungleConsent.ACCEPTED:
                    return vungleConsentEnum.GetStatic<AndroidJavaObject>("OPTED_IN");
                case VungleConsent.DENIED:
                    return vungleConsentEnum.GetStatic<AndroidJavaObject>("OPTED_OUT");
                default:
                    return null;
            }
        }

        private VungleConsent GetVungleConsentFromConsentStatus(AndroidJavaObject consentStatus)
        {
            if (consentStatus == null)
            {
                return VungleConsent.UNKNOWN;
            }

            int vungleConsentValue = consentStatus.Call<int>("ordinal");

            AndroidJavaClass vungleConsentEnum = new AndroidJavaClass(VUNGLE_CONSENT_ENUM_NAME);
            int optedInConsentValue = vungleConsentEnum
                    .GetStatic<AndroidJavaObject>("OPTED_IN")
                    .Call<int>("ordinal");
            int optedOutConsentValue = vungleConsentEnum
                    .GetStatic<AndroidJavaObject>("OPTED_OUT")
                    .Call<int>("ordinal");

            if (vungleConsentValue == optedInConsentValue)
            {
                return VungleConsent.ACCEPTED;
            }
            else if (vungleConsentValue == optedOutConsentValue)
            {
                return VungleConsent.DENIED;
            }
            return VungleConsent.UNKNOWN;
        }
    }
}

#endif
