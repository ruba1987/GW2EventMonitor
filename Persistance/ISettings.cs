﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistance
{
    public interface ISettings
    {
        void RefreshData(object settingsInfo);
    }
}
