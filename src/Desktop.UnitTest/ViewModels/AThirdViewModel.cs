﻿//----------------------------------------------------------------------- 
// PDS WITSMLstudio Desktop, 2017.1
//
// Copyright 2017 Petrotechnical Data Systems
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

using Caliburn.Micro;

namespace PDS.WITSMLstudio.Desktop.Core.ViewModels
{
    /// <summary>
    /// An IPluginViewModel for testing.
    /// </summary>
    /// <seealso cref="Caliburn.Micro.Screen" />
    /// <seealso cref="PDS.WITSMLstudio.Desktop.Core.ViewModels.IPluginViewModel" />
    public sealed class AThirdViewModel : Screen, IPluginViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AThirdViewModel"/> class.
        /// </summary>
        public AThirdViewModel()
        {
            DisplayName = DisplayOrder.ToString();
        }

        /// <summary>
        /// Gets the display order of the plug-in when loaded by the main application shell
        /// </summary>
        public int DisplayOrder => 300;

        /// <summary>
        /// Gets the sub title to display in the main application title bar.
        /// </summary>
        public string SubTitle => DisplayName;
    }
}
