﻿using System.Linq;
using Caliburn.Micro;
using Energistics.DataAccess;
using PDS.Witsml.Studio.Runtime;

namespace PDS.Witsml.Studio.Plugins.WitsmlBrowser.ViewModels.Request
{
    /// <summary>
    /// Manages the behavior for the request UI elements.
    /// </summary>
    /// <seealso cref="Caliburn.Micro.Conductor{Caliburn.Micro.IScreen}.Collection.OneActive" />
    public class RequestViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(RequestViewModel));

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestViewModel"/> class.
        /// </summary>
        /// <param name="runtime">The runtime service.</param>
        public RequestViewModel(IRuntimeService runtime)
        {
            _log.Debug("Creating view model instance");
            Runtime = runtime;
        }

        /// <summary>
        /// Gets the Parent <see cref="T:Caliburn.Micro.IConductor" /> for this view model.
        /// </summary>
        public new MainViewModel Parent
        {
            get { return (MainViewModel)base.Parent; }
        }

        /// <summary>
        /// Gets the data model for this view model.
        /// </summary>
        /// <value>
        /// The WitsmlSettings data model.
        /// </value>
        public Models.WitsmlSettings Model
        {
            get { return Parent.Model; }
        }

        /// <summary>
        /// Gets the proxy to the WITSML web service.
        /// </summary>
        /// <value>
        /// The WITSML web service proxy.
        /// </value>
        public WITSMLWebServiceConnection Proxy
        {
            get { return Parent.Proxy; }
        }

        /// <summary>
        /// Gets the runtime service.
        /// </summary>
        /// <value>The runtime.</value>
        public IRuntimeService Runtime { get; private set; }

        /// <summary>
        /// Loads the screens for the request view model.
        /// </summary>
        internal void LoadScreens()
        {
            _log.Debug("Loading RequestViewModel screens");
            Items.Add(new SettingsViewModel(Runtime));
            //Items.Add(new TreeViewViewModel());
            //Items.Add(new TemplatesViewModel());
            Items.Add(new QueryViewModel(Runtime, Parent.XmlQuery));

            ActivateItem(Items.FirstOrDefault());
        }

        /// <summary>
        /// Called when initializing the request view model.
        /// </summary>
        protected override void OnInitialize()
        {
            _log.Debug("Initializing screen");
            base.OnInitialize();
            LoadScreens();
        }
    }
}
