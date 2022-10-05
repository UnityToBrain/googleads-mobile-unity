// Copyright (C) 2022 Google LLC
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

namespace GoogleMobileAds.Ump
{
    /// <summary>
    /// Debug settings for testing User Messaging Platform.
    /// </summary>
    [System.Serializable]
    public class ConsentDebugSettings
    {
        /// <summary>
        /// The debug geography for testing purposes.
        /// </summary>
        public DebugGeography TestDebugGeography;
        /// <summary>
        /// The debug geography for testing purposes.
        /// </summary>
        public List<string> TestDeviceHashedIds;

        public ConsentDebugSettings() { }
        public ConsentDebugSettings(DebugGeography debugGeography, List<string> testDeviceHashedIds)
        {
            TestDebugGeography = debugGeography;
            TestDeviceHashedIds = testDeviceHashedIds;
        }
    }
}
