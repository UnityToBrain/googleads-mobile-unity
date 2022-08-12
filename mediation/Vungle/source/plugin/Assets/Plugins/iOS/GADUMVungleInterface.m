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

#import <VungleSDK/VungleSDK.h>

#pragma mark - Utility

typedef NS_ENUM(NSUInteger, GADUMVungleConsentStatus) {
  GADUMVungleConsentStatusUnknown = -1,
  GADUMVungleConsentStatusAccepted = 0,
  GADUMVungleConsentStatusDenied = 1
};

static const char *NSStringToCString(NSString *_Nonnull string) {
  const char *utf8String = [string UTF8String];
  if (!utf8String) {
    utf8String = "";
  }

  // Unity recommends that string values returned from a native method should be UTF–8 encoded and
  // allocated on the heap.
  char *cString = (char *)malloc(strlen(utf8String) + 1);
  strcpy(cString, utf8String);
  return cString;
}

/**
 Convert the `consentStatus` obtained from Unity to Vungle's VungleConsentStatus enum.
 Vungle's Android and iOS consent enum ordinals do not share the same value so it needs to be
 converted for iOS.
 */
static VungleConsentStatus GADUMVungleConsentFromUnity(int vungleConsentStatus) {
  switch (vungleConsentStatus) {
    case GADUMVungleConsentStatusAccepted:
      return VungleConsentAccepted;
    case GADUMVungleConsentStatusDenied:
      return VungleConsentDenied;
    default:
      // Vungle does not have an "unknown" value and the Unity wrapper API should not reach this
      // point.
      return -1;
  }
}

static VungleCCPAStatus GADUMVungleCCPAStatusFromUnity(int vungleCCPAStatus) {
  switch (vungleCCPAStatus) {
    case GADUMVungleConsentStatusAccepted:
      return VungleCCPAAccepted;
    case GADUMVungleConsentStatusDenied:
      return VungleCCPADenied;
    default:
      // Vungle does not have an "unknown" value and the Unity wrapper API should not reach this
      // point.
      return -1;
  }
}

/**
 Convert the `consentStatus` obtained from Vungle to the Unity wrapper consent status enum.
 Vungle's Android and iOS consent enum ordinals do not share the same value so it needs to be
 converted for iOS.
 */
static int GADUMVungleUnityConsentFromVungleConsent(VungleConsentStatus vungleConsent) {
  switch (vungleConsent) {
    case VungleConsentAccepted:
      return GADUMVungleConsentStatusAccepted;
    case VungleConsentDenied:
      return GADUMVungleConsentStatusDenied;
    default:
      // Vungle does not have an "unknown" value and the Unity wrapper API should not reach this
      // point.
      return GADUMVungleConsentStatusUnknown;
  }
}

static int GADUMVungleUnityConsentFromVungleCCPAStatus(VungleCCPAStatus vungleCCPAStatus) {
  switch (vungleCCPAStatus) {
    case VungleCCPAAccepted:
      return GADUMVungleConsentStatusAccepted;
    case VungleCCPADenied:
      return GADUMVungleConsentStatusDenied;
    default:
      // Vungle does not have an "unknown" value and the Unity wrapper API should not reach this
      // point.
      return GADUMVungleConsentStatusUnknown;
  }
}

#pragma mark - GADUMVungleInterface implementation

void GADUMVungleUpdateConsentStatus(int consentStatus, const char *consentMessageVersion) {
  VungleConsentStatus status = GADUMVungleConsentFromUnity(consentStatus);
  [[VungleSDK sharedSDK] updateConsentStatus:status consentMessageVersion:@(consentMessageVersion)];
}

int GADUMVungleGetCurrentConsentStatus() {
  VungleConsentStatus status = [[VungleSDK sharedSDK] getCurrentConsentStatus];
  return GADUMVungleUnityConsentFromVungleConsent(status);
}

const char *GADUMVungleGetCurrentConsentMessageVersion() {
  NSString *vungleConsentMessageVersion = [[VungleSDK sharedSDK] getConsentMessageVersion];
  if (!vungleConsentMessageVersion.length) {
    vungleConsentMessageVersion = @"";
  }
  return NSStringToCString(vungleConsentMessageVersion);
}

void GADUMVungleUpdateCCPAStatus(int ccpaStatus) {
  VungleCCPAStatus vungleCCPAStatus = GADUMVungleCCPAStatusFromUnity(ccpaStatus);
  [[VungleSDK sharedSDK] updateCCPAStatus:vungleCCPAStatus];
}

int GADUMVungleGetCCPAStatus() {
  VungleCCPAStatus vungleCCPAStatus = [[VungleSDK sharedSDK] getCurrentCCPAStatus];
  return GADUMVungleUnityConsentFromVungleCCPAStatus(vungleCCPAStatus);
}
