﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConMan
{
    public static class Settings
    {
        public static void Configure(Action<SettingsManager> configuration)
        {
            configuration?.Invoke(SettingsManager.GlobalInstance);
        }
    }
}
