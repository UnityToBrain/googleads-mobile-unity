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

namespace GoogleMobileAds.Api
{
    /// <summary>
    /// Allows apps that have opted into the
    /// <seealso href="http://developer.android.com/distribute/googleplay/families/about.html">
    /// Designed for Families </seealso> program to specify whether a given ad request should
    /// return Designed for Families-compliant ads. Please note that the method is ONLY intended
    /// for apps that have opted into Designed for Families.Please see the
    /// <seealso href="https://support.google.com/admob/answer/6223431?hl=en"> Google AdMob help
    /// center article</seealso> for more information about this setting.
    /// </summary>
    public enum TagForChildDirectedTreatment
    {
        Unspecified = -1,
        False = 0,
        True = 1,
    }
}
