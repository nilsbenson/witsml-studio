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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Avro;
using Avro.Specific;
using Caliburn.Micro;
using Energistics.Common;
using Energistics.Datatypes;
using Energistics.Protocol.Core;
using Newtonsoft.Json;
using PDS.WITSMLstudio.Desktop.Core.Connections;
using PDS.WITSMLstudio.Desktop.Core.Runtime;
using PDS.WITSMLstudio.Desktop.Core.ViewModels;

namespace PDS.WITSMLstudio.Desktop.Plugins.EtpBrowser.ViewModels
{
    /// <summary>
    /// Manages the behavior of the JSON Message user interface elements.
    /// </summary>
    /// <seealso cref="Caliburn.Micro.Screen" />
    /// <seealso cref="PDS.WITSMLstudio.Desktop.Plugins.EtpBrowser.ViewModels.ISessionAware" />
    public sealed class JsonMessageViewModel : Screen, ISessionAware
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(JsonMessageViewModel));
        private MessageHeader _currentHeader;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonMessageViewModel"/> class.
        /// </summary>
        public JsonMessageViewModel(IRuntimeService runtime)
        {
            Runtime = runtime;
            DisplayName = "JSON Message";
            Header = new TextEditorViewModel(runtime, "JavaScript");
            Message = new TextEditorViewModel(runtime, "JavaScript");
            MessageTypes = new BindableCollection<KeyValuePair<string, Type>>
            {
                new KeyValuePair<string, Type>("Messages...", null)
            };

            var recordType = typeof (ISpecificRecord);

            MessageTypes.AddRange(typeof(MessageHeader).Assembly.GetTypes()
                .Where(x => recordType.IsAssignableFrom(x) && HasProtocolProperty(x))
                .OrderBy(GetProtocol)
                .ThenBy(x => x.Name)
                .Select(x => new KeyValuePair<string, Type>($"{GetProtocol(x)} - {x.Name}", x)));

            SelectedMessageType = MessageTypes[0];
        }

        /// <summary>
        /// Gets the runtime service.
        /// </summary>
        /// <value>The runtime.</value>
        public IRuntimeService Runtime { get; }

        public BindableCollection<KeyValuePair<string, Type>> MessageTypes { get; }

        /// <summary>
        /// Gets or Sets the Parent <see cref="T:Caliburn.Micro.IConductor" />
        /// </summary>
        public new MainViewModel Parent
        {
            get { return (MainViewModel)base.Parent; }
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>The model.</value>
        public Models.EtpSettings Model
        {
            get { return Parent.Model; }
        }

        private TextEditorViewModel _header;

        /// <summary>
        /// Gets or sets the header editor.
        /// </summary>
        /// <value>The text editor view model.</value>
        public TextEditorViewModel Header
        {
            get { return _header; }
            set
            {
                if (!ReferenceEquals(_header, value))
                {
                    _header = value;
                    NotifyOfPropertyChange(() => Header);
                }
            }
        }

        private TextEditorViewModel _message;

        /// <summary>
        /// Gets or sets the message editor.
        /// </summary>
        /// <value>The text editor view model.</value>
        public TextEditorViewModel Message
        {
            get { return _message; }
            set
            {
                if (!ReferenceEquals(_message, value))
                {
                    _message = value;
                    NotifyOfPropertyChange(() => Message);
                }
            }
        }

        private bool _canExecute;

        /// <summary>
        /// Gets or sets a value indicating whether the Store protocol messages can be executed.
        /// </summary>
        /// <value><c>true</c> if Store protocol messages can be executed; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool CanExecute
        {
            get { return _canExecute; }
            set
            {
                if (_canExecute != value)
                {
                    _canExecute = value;
                    NotifyOfPropertyChange(() => CanExecute);
                }
            }
        }

        private KeyValuePair<string, Type> _selectedMessageType;

        /// <summary>
        /// Gets or sets the selected message type.
        /// </summary>
        /// <value>The selected message type.</value>
        public KeyValuePair<string, Type> SelectedMessageType
        {
            get { return _selectedMessageType; }
            set
            {
                _selectedMessageType = value;
                NotifyOfPropertyChange(() => SelectedMessageType);
                OnMessageTypeChanged();
            }
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        public void SendMessage()
        {
            Runtime.InvokeAsync(() =>
            {
                try
                {
                    var headerText = Header.Document.Text;
                    var messageText = Message.Document.Text;

                    var header = JsonConvert.DeserializeObject<MessageHeader>(headerText);
                    var message = JsonConvert.DeserializeObject(messageText, SelectedMessageType.Value) as ISpecificRecord;

                    Parent.Client.SendMessage(header, message);
                }
                catch (Exception ex)
                {
                    _log.Warn("Error sending message", ex);
                    Parent.LogClientError("Error sending message:", ex);
                }
            });
        }

        /// <summary>
        /// Called when the selected connection has changed.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public void OnConnectionChanged(Connection connection)
        {
        }

        /// <summary>
        /// Called when the <see cref="OpenSession" /> message is recieved.
        /// </summary>
        /// <param name="e">The <see cref="ProtocolEventArgs{T}" /> instance containing the event data.</param>
        public void OnSessionOpened(ProtocolEventArgs<OpenSession> e)
        {
            CanExecute = true;

            Runtime.Invoke(() =>
            {
                if (Header.Document.TextLength == 0)
                {
                    NewHeader();
                }
            });
        }

        /// <summary>
        /// Called when the <see cref="Energistics.EtpClient" /> web socket is closed.
        /// </summary>
        public void OnSocketClosed()
        {
            CanExecute = false;
        }

        /// <summary>
        /// Initializes a new ETP message header.
        /// </summary>
        public void NewHeader()
        {
            _currentHeader = new MessageHeader
            {
                MessageId = Parent.Client.NewMessageId()
            };

            OnHeaderChanged();
        }

        private void OnHeaderChanged()
        {
            Header.SetText(Parent.Client.Serialize(_currentHeader, true));
        }

        private void OnMessageTypeChanged()
        {
            if (_selectedMessageType.Key == MessageTypes[0].Key)
                return;

            var type = SelectedMessageType.Value;
            var message = Activator.CreateInstance(type) as ISpecificRecord;

            Message.SetText(Parent.Client.Serialize(message, true));
            UpdateCurrentHeader(message?.Schema);
        }

        private void UpdateCurrentHeader(Schema schema)
        {
            if (_currentHeader == null || schema == null) return;

            int protocol;
            if (int.TryParse(schema.GetProperty("protocol"), out protocol))
            {
                _currentHeader.Protocol = protocol;
                OnHeaderChanged();
            }

            int messageType;
            if (int.TryParse(schema.GetProperty("messageType"), out messageType))
            {
                _currentHeader.MessageType = messageType;
                OnHeaderChanged();
            }
        }

        private bool HasProtocolProperty(Type type)
        {
            return !string.IsNullOrWhiteSpace(GetProtocol(type));
        }

        /// <summary>
        /// Gets the protocol of the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The protocol of the type</returns>
        private string GetProtocol(Type type)
        {
            var record = Activator.CreateInstance(type) as ISpecificRecord;
            return record?.Schema.GetProperty("protocol");
        }
    }
}
