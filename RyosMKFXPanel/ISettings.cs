using System.Collections.Generic;

namespace RyosMKFXPanel {
    internal interface ISettings {
        Dictionary<string, string> SettingsGet();
        void SettingsLoad(in Dictionary<string, string> settings);
    }
}
