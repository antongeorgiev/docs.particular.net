﻿namespace Core4.DataBus.FileShare
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure, string databusPath)
        {
            #region FileShareDataBus

            configure.FileShareDataBus(databusPath);

            #endregion
        }

    }
}

