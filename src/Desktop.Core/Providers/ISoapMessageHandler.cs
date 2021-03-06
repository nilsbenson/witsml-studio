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

using System.ComponentModel.Composition;

namespace PDS.WITSMLstudio.Desktop.Core.Providers
{
    /// <summary>
    /// Defines methods that can be used to process SOAP messages.
    /// </summary>
    [InheritedExport]
    public interface ISoapMessageHandler
    {
        /// <summary>
        /// Logs the SOAP request message.
        /// </summary>
        /// <param name="action">The SOAP action.</param>
        /// <param name="message">The SOAP message.</param>
        void LogRequest(string action, string message);

        /// <summary>
        /// Logs the SOAP response message.
        /// </summary>
        /// <param name="action">The SOAP action.</param>
        /// <param name="message">The SOAP message.</param>
        void LogResponse(string action, string message);
    }
}
