﻿namespace Snippets6.Lifecycle
{
    using NServiceBus;
    using NServiceBus.Settings;

    #region lifecycle-iwanttorunbeforeconfigurationisfinalized

    class RunBeforeConfigurationIsFinalized : IFinalizeConfiguration
    {
        public void Run(SettingsHolder settings)
        {
            // update config instance
            settings.Set("key", "value");
            // after this config.Settings will be frozen
        }
    }

    #endregion
}
