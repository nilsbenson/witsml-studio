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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PDS.WITSMLstudio.Desktop.ViewModels
{
    [TestClass]
    public class ConnectionViewModelTests : ConnectionViewModelTestBase
    {
        [TestMethod]
        public void ConnectionViewModel_Witsml_Can_Accept_Connection_Changes_With_DataItem()
        {
            var newName = "xxx";

            // Initialze the Edit Item by setting the DataItem
            _witsmlConnectionVm.DataItem = _witsmlConnection;
            _witsmlConnectionVm.InitializeEditItem();

            // Make a change to the edit item
            _witsmlConnectionVm.EditItem.Name = newName;

            // Accept the changes
            _witsmlConnectionVm.AcceptConnectionChanges();

            Assert.AreEqual(newName, _witsmlConnectionVm.DataItem.Name);
        }
    }
}
