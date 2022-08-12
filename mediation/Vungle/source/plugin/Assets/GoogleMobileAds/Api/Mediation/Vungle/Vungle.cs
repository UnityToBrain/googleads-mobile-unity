﻿// Copyright 2018 Google LLC
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

using GoogleMobileAds.Common.Mediation.Vungle;
using GoogleMobileAds.Mediation;

namespace GoogleMobileAds.Api.Mediation.Vungle
{
    public enum VungleConsent
    {
        UNKNOWN = -1,
        ACCEPTED = 0,
        DENIED = 1
    }

    public class Vungle
    {
        public static readonly IVungleClient client = VungleClientFactory.CreateVungleClient();

        public static void UpdateConsentStatus(VungleConsent consentStatus,
                                               String consentMessageVersion)
        {
            client.UpdateConsentStatus(consentStatus, consentMessageVersion);
        }

        public static VungleConsent GetCurrentConsentStatus()
        {
            return client.GetCurrentConsentStatus();
        }

        public static String GetCurrentConsentMessageVersion()
        {
            return client.GetCurrentConsentMessageVersion();
        }

        public static void UpdateCCPAStatus(VungleConsent consentStatus)
        {
            client.UpdateCCPAStatus(consentStatus);
        }

        public static VungleConsent GetCCPAStatus()
        {
            return client.GetCCPAStatus();
        }
    }
}
