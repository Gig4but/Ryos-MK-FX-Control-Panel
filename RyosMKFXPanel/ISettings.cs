using System.Collections.Generic;

namespace RyosMKFXPanel {
    internal interface ISettings {
        Dictionary<string, string> GetSettings();
        void LoadSettings(in Dictionary<string, string> settings);
    }
}
