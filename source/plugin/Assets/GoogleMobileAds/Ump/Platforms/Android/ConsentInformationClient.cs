#if UNITY_ANDROID
// Copyright (C) 2022 Google LLC.
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
using UnityEngine;
using GoogleMobileAds.Ump.Common;
using System.Reflection;

namespace GoogleMobileAds.Ump.Android
{
    public class ConsentInformationClient : IConsentInformationClient
    {
        private static readonly ConsentInformationClient instance = new ConsentInformationClient();
        private readonly AndroidJavaObject userMessagingPlatformClass;
        private readonly AndroidJavaObject consentInformation;
        private readonly AndroidJavaObject playerClass;
        private readonly AndroidJavaObject activity;
        internal OnConsentInfoUpdateSuccessListener onSuccess;
        internal OnConsentInfoUpdateFailureListener onFailure;

        private ConsentInformationClient()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                playerClass = new AndroidJavaClass(Utils.UnityActivityClassName);
                activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
                userMessagingPlatformClass =
                        new AndroidJavaClass(Utils.UserMessagingPlatformClassName);
                consentInformation = userMessagingPlatformClass.CallStatic<AndroidJavaObject>(
                        "getConsentInformation", activity);
            }
        }

        public static ConsentInformationClient Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Requests consent information update.
        /// </summary>
        /// <param name="request">The request params.</param>
        /// <param name="onSuccess">Called when ConsentStatus is updated.
        /// <param name="onError">Called on consent form request error.
        /// Includes a UmpResult argument if show failed. </param>
        public void RequestConsentInfoUpdate(ConsentRequestParameters request,
                Action onConsentInfoUpdateSuccessCallback,
                Action<FormError> onConsentInfoUpdateFailureCallback)
        {
            onSuccess = new OnConsentInfoUpdateSuccessListener(onConsentInfoUpdateSuccessCallback);
            onFailure = new OnConsentInfoUpdateFailureListener(onConsentInfoUpdateFailureCallback);
            AndroidJavaObject consentRequestParameters =
                    Utils.GetConsentRequestParametersJavaObject(request, activity);

            if (Application.platform == RuntimePlatform.Android)
            {
                consentInformation.Call("requestConsentInfoUpdate", activity,
                                        consentRequestParameters, onSuccess, onFailure);
            }
        }

        /// <summary>
        /// Clears all consent status from persistent storage.
        /// </summary>
        public void ResetInfo()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                consentInformation.Call("reset");
            }
            else
            {
                throw new InvalidOperationException(@"Called " + MethodBase.GetCurrentMethod().Name
                                                    + " on non-Android runtime");
            }
        }

        /// <summary>
        /// Gets the current consent status.
        /// <para>This value is cached between app sessons and can be read before
        /// requesting updated parameters.</para>
        /// </summary>
        public int GetConsentStatus()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return consentInformation.Call<int>("getConsentStatus");
            }
            throw new InvalidOperationException(@"Called " + MethodBase.GetCurrentMethod().Name +
                    " on non-Android runtime");
        }

        /// <summary>
        /// Returns <c>true</c> if a <see cref="GoogleMobileAds.Ump.Api.ConsentForm">
        /// ConsentForm</see> is available, <c>false</c> otherwise.
        /// </summary>
        public bool IsConsentFormAvailable()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return consentInformation.Call<bool>("isConsentFormAvailable");
            }
            throw new InvalidOperationException(@"Called " + MethodBase.GetCurrentMethod().Name +
                    " on non-Android runtime");
        }
    }
}
#endif
