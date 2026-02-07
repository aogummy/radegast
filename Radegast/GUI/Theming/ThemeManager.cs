/**
 * Radegast Metaverse Client
 * Copyright(c) 2009-2014, Radegast Development Team
 * Copyright(c) 2016-2025, Sjofn, LLC
 * All rights reserved.
 *
 * Radegast is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DarkModeForms;
using OpenMetaverse;
using OpenMetaverse.StructuredData;

namespace Radegast
{
    /// <summary>
    /// Central theme management: wraps DarkModeCS, persists theme preference,
    /// applies theme to forms, and raises ThemeChanged when the user changes theme.
    /// </summary>
    public sealed class ThemeManager
    {
        private const string ThemeModeKey = "theme_mode";
        private readonly RadegastInstanceForms _instance;
        private readonly Dictionary<Form, DarkModeCS> _formHandles = new Dictionary<Form, DarkModeCS>();

        public ThemeManager(RadegastInstanceForms instance)
        {
            _instance = instance ?? throw new ArgumentNullException(nameof(instance));
            LoadPreferenceFromSettings();
        }

        /// <summary>Current user preference (Light, Dark, or System).</summary>
        public ThemePreference CurrentPreference { get; private set; } = ThemePreference.System;

        /// <summary>Fired when the theme preference changes (e.g. from Settings).</summary>
        public event EventHandler ThemeChanged;

        /// <summary>
        /// Resolves effective theme: for System, uses Windows AppsUseLightTheme (1 = light, 0 = dark).
        /// </summary>
        public bool IsEffectiveDarkMode
        {
            get
            {
                switch (CurrentPreference)
                {
                    case ThemePreference.Dark: return true;
                    case ThemePreference.Light: return false;
                    case ThemePreference.System:
                    default:
                        return DarkModeCS.GetWindowsColorMode() <= 0;
                }
            }
        }

        private void LoadPreferenceFromSettings()
        {
            try
            {
                if (_instance.GlobalSettings == null) return;
                if (!_instance.GlobalSettings.ContainsKey(ThemeModeKey)) return;
                var val = _instance.GlobalSettings[ThemeModeKey];
                if (val.Type == OSDType.Unknown) return;
                int i = val.AsInteger();
                if (i >= 0 && i <= 2)
                    CurrentPreference = (ThemePreference)i;
            }
            catch
            {
                CurrentPreference = ThemePreference.System;
            }
        }

        private static DarkModeCS.DisplayMode ToDisplayMode(ThemePreference preference)
        {
            switch (preference)
            {
                case ThemePreference.Dark: return DarkModeCS.DisplayMode.DarkMode;
                case ThemePreference.Light: return DarkModeCS.DisplayMode.ClearMode;
                case ThemePreference.System:
                default: return DarkModeCS.DisplayMode.SystemDefault;
            }
        }

        /// <summary>
        /// Applies the current theme to the given form using DarkModeCS.
        /// Always registers the form so it receives updates when preference changes; when
        /// theme_compatibility_mode is on and user did not explicitly choose Dark, applies light mode.
        /// </summary>
        public void ApplyToForm(Form form)
        {
            if (form == null) return;
            if (_instance.GlobalSettings == null) return;

            try
            {
                bool compatibilityMode = _instance.GlobalSettings.ContainsKey("theme_compatibility_mode") &&
                    _instance.GlobalSettings["theme_compatibility_mode"].AsBoolean();
                var displayMode = (compatibilityMode && CurrentPreference != ThemePreference.Dark)
                    ? DarkModeCS.DisplayMode.ClearMode
                    : ToDisplayMode(CurrentPreference);

                DarkModeCS dm;
                if (_formHandles.TryGetValue(form, out dm))
                {
                    dm.ColorMode = displayMode;
                    return;
                }

                dm = new DarkModeCS(form, _ColorizeIcons: true, _RoundedPanels: false)
                {
                    ColorMode = displayMode
                };
                _formHandles[form] = dm;
                form.FormClosed += (s, e) => _formHandles.Remove((Form)s);
            }
            catch (Exception ex)
            {
                Logger.Warn("ThemeManager.ApplyToForm failed", ex);
            }
        }

        /// <summary>
        /// Sets the theme preference, persists to settings, and notifies listeners.
        /// Optionally re-applies theme to all tracked forms.
        /// </summary>
        public void SetPreference(ThemePreference preference, bool reapplyToOpenForms = true)
        {
            if (CurrentPreference == preference) return;
            CurrentPreference = preference;

            try
            {
                _instance.GlobalSettings[ThemeModeKey] = OSD.FromInteger((int)preference);
                _instance.GlobalSettings.Save();
            }
            catch (Exception ex)
            {
                Logger.Warn("ThemeManager: failed to save theme_mode", ex);
            }

            if (reapplyToOpenForms)
            {
                var displayMode = ToDisplayMode(preference);
                foreach (var kv in _formHandles)
                {
                    try
                    {
                        kv.Value.ColorMode = displayMode;
                    }
                    catch { /* form may be disposed */ }
                }
            }

            ThemeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
