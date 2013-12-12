﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventloopPlugin
{
    /// <summary>
    /// Implements a simple event loop to which plugins can register. The Loop fires its event in a fixed interval that can be specified
    /// in the config file. The EventArgs with which the event is invoked contain the time in milliseconds since the application is running
    /// </summary>
    public class Eventloop
    {
        public static Eventloop Instance;

        /// <summary>
        /// Event Handler to which other plugins can register
        /// </summary>
        public event EventHandler<TickEventArgs> TickFired;

        public Eventloop()
        {
            readIntervalFromConfig();
            stopwatch.Start();
            Task.Factory.StartNew(tickFired, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// Reads the interval in which the events are fired from the App.cfg file
        /// </summary>
        private void readIntervalFromConfig()
        {
            string eventloopConfigPath = this.GetType().Assembly.Location;
            Configuration config = ConfigurationManager.OpenExeConfiguration(eventloopConfigPath);

            string configValue = config.AppSettings.Settings["tickinterval"].Value;
            int.TryParse(configValue, out tickInterval);
        }

        /// <summary>
        /// Function that fires the event periodically
        /// </summary>
        private void tickFired()
        {
            while (true)
            {
                if (TickFired != null)
                    TickFired(this, new TickEventArgs(stopwatch.Elapsed));
                Thread.Sleep(tickInterval);
            }
        }

        private int tickInterval = 30;
        private Stopwatch stopwatch = new Stopwatch();
    }
}
