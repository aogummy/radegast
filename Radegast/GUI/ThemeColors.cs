/*
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
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.If not, see<https://www.gnu.org/licenses/>.
 */

using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Radegast
{
    /// <summary>
    /// Central theme palette for light/dark mode. When dark_mode is true, returns dark colors;
    /// otherwise returns system (light) colors.
    /// </summary>
    public static class ThemeColors
    {
        // Dark palette
        private static readonly Color DarkWindowBack = Color.FromArgb(45, 45, 45);
        private static readonly Color DarkWindowText = Color.FromArgb(0xcf, 0xcf, 0xcf);   // #cfcfcf
        private static readonly Color DarkControlBack = Color.FromArgb(62, 62, 62);
        private static readonly Color DarkControlText = Color.FromArgb(0xcf, 0xcf, 0xcf);  // #cfcfcf
        private static readonly Color DarkNotificationBack = Color.FromArgb(52, 52, 56);
        private static readonly Color DarkLinkColor = Color.FromArgb(100, 180, 255);
        private static readonly Color DarkBorder = Color.FromArgb(0x4f, 0x4f, 0x4f);       // #4f4f4f - button edges, section borders
        private static readonly Color DarkButtonBack = Color.FromArgb(52, 52, 52);        // darker than ControlBack so dialog buttons aren't white
        private static readonly Color DarkButtonHover = Color.FromArgb(72, 72, 72);        // hover/press state

        public static bool IsDark(RadegastInstanceForms instance)
        {
            if (instance?.GlobalSettings == null) return false;
            try
            {
                return instance.GlobalSettings["dark_mode"].AsBoolean();
            }
            catch
            {
                return false;
            }
        }

        public static Color WindowBack(RadegastInstanceForms instance)
            => IsDark(instance) ? DarkWindowBack : SystemColors.Window;

        public static Color WindowText(RadegastInstanceForms instance)
            => IsDark(instance) ? DarkWindowText : SystemColors.WindowText;

        public static Color ControlBack(RadegastInstanceForms instance)
            => IsDark(instance) ? DarkControlBack : SystemColors.Control;

        public static Color ControlText(RadegastInstanceForms instance)
            => IsDark(instance) ? DarkControlText : SystemColors.ControlText;

        public static Color NotificationBack(RadegastInstanceForms instance)
        {
            if (instance == null) return SystemColors.Control;
            if (IsDark(instance)) return DarkNotificationBack;
            try
            {
                if (!instance.GlobalSettings["theme_compatibility_mode"].AsBoolean() && instance.PlainColors)
                    return Color.FromArgb(120, 220, 255);
            }
            catch { }
            return SystemColors.Control;
        }

        public static Color LinkColor(RadegastInstanceForms instance)
            => IsDark(instance) ? DarkLinkColor : Color.Blue;

        /// <summary>Border color for button edges and section borders in dark mode.</summary>
        public static Color Border(RadegastInstanceForms instance)
            => IsDark(instance) ? DarkBorder : SystemColors.ControlDark;

        /// <summary>
        /// Apply dark/light theme to ToolStrip tooltips on the form so hover tooltips are readable.
        /// Uses reflection to access the internal ToolTip on each ToolStrip.
        /// </summary>
        public static void ApplyTooltipTheme(Form form, RadegastInstanceForms instance)
        {
            if (form == null || instance == null) return;
            try
            {
                bool dark = IsDark(instance);
                FindAndApplyTooltips(form, instance, dark);
            }
            catch { }
        }

        private static void FindAndApplyTooltips(Control parent, RadegastInstanceForms instance, bool dark)
        {
            if (parent == null) return;
            foreach (Control c in parent.Controls)
            {
                if (c is ToolStrip ts)
                    ApplyTooltipToStrip(ts, instance, dark);
                else
                    FindAndApplyTooltips(c, instance, dark);
            }
        }

        private static void ApplyTooltipToStrip(ToolStrip strip, RadegastInstanceForms instance, bool dark)
        {
            try
            {
                var prop = strip.GetType().GetProperty("ToolTip",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if (prop?.GetValue(strip) is ToolTip tt)
                {
                    if (dark)
                    {
                        tt.BackColor = ControlBack(instance);
                        tt.ForeColor = ControlText(instance);
                        tt.OwnerDraw = true;
                        tt.Draw -= ToolTip_Draw;
                        tt.Draw += ToolTip_Draw;
                    }
                    else
                    {
                        tt.OwnerDraw = false;
                        tt.Draw -= ToolTip_Draw;
                    }
                }
            }
            catch { }
        }

        private static void ToolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            try
            {
                e.DrawBackground();
                e.DrawBorder();
                // DrawToolTipEventArgs has no ForeColor in .NET Framework; use the ToolTip we set
                var textColor = (sender is ToolTip tt) ? tt.ForeColor : SystemColors.InfoText;
                using (var brush = new SolidBrush(textColor))
                    e.Graphics.DrawString(e.ToolTipText, e.Font, brush, e.Bounds.X + 2, e.Bounds.Y + 2);
            }
            catch { }
        }

        /// <summary>
        /// Recursively apply current theme (dark or light) to a control and its children.
        /// Call this on Load and when dark_mode setting changes so dialogs and strips update.
        /// </summary>
        public static void ApplyThemeRecursive(Control control, RadegastInstanceForms instance)
        {
            if (control == null || instance == null) return;

            try
            {
                ApplyThemeToControl(control, instance);
                foreach (Control child in control.Controls)
                    ApplyThemeRecursive(child, instance);
            }
            catch
            {
                // Ignore errors on individual controls (e.g. custom or disposed)
            }
        }

        private static void ApplyThemeToControl(Control control, RadegastInstanceForms instance)
        {
            if (control == null || instance == null) return;

            if (ShouldSkipTheming(control)) return;

            bool dark = IsDark(instance);

            // ToolStrip, MenuStrip, StatusStrip: must set RenderMode so BackColor/ForeColor are used
            if (control is ToolStrip ts)
            {
                if (dark)
                {
                    ts.RenderMode = ToolStripRenderMode.Professional;
                    ts.BackColor = ControlBack(instance);
                    ts.ForeColor = ControlText(instance);
                }
                else
                {
                    ts.RenderMode = ToolStripRenderMode.System;
                    ts.BackColor = SystemColors.Control;
                    ts.ForeColor = SystemColors.ControlText;
                }
                return;
            }

            if (!dark)
            {
                // Light mode: apply system colors so toggling from dark restores light UI
                ApplyLightThemeToControl(control);
                return;
            }

            // Dark mode
            if (control is ButtonBase btn)
            {
                btn.BackColor = DarkButtonBack;
                btn.ForeColor = ControlText(instance);
                btn.UseVisualStyleBackColor = false;
                if (btn is Button button)
                {
                    button.FlatAppearance.BorderColor = Border(instance);
                    button.FlatAppearance.MouseOverBackColor = DarkButtonHover;
                    button.FlatAppearance.MouseDownBackColor = DarkButtonHover;
                    button.FlatStyle = FlatStyle.Flat;
                }
                return;
            }
            if (control is ListView lv)
            {
                lv.BackColor = WindowBack(instance);
                lv.ForeColor = WindowText(instance);
                return;
            }
            if (control is TreeView tv)
            {
                tv.BackColor = WindowBack(instance);
                tv.ForeColor = WindowText(instance);
                return;
            }
            if (control is ComboBox cb)
            {
                cb.BackColor = WindowBack(instance);
                cb.ForeColor = WindowText(instance);
                return;
            }
            if (control is TextBoxBase tb)
            {
                tb.BackColor = WindowBack(instance);
                tb.ForeColor = WindowText(instance);
                return;
            }
            if (control is LinkLabel link)
            {
                link.BackColor = ControlBack(instance);
                link.ForeColor = LinkColor(instance);
                link.LinkColor = LinkColor(instance);
                link.VisitedLinkColor = LinkColor(instance);
                link.ActiveLinkColor = ControlText(instance);
                return;
            }
            if (control is Label || control is GroupBox || control is Panel
                || control is Form || control is UserControl || control is TabPage)
            {
                if (control is Panel pnl && pnl.BackColor == Color.Transparent)
                    return;
                control.BackColor = control is Form || control is UserControl || control is Panel || control is TabPage
                    ? ControlBack(instance)
                    : control.BackColor;
                control.ForeColor = ControlText(instance);
                return;
            }
            // Generic fallback - ensure black/dark text becomes bright in dark mode
            control.BackColor = ControlBack(instance);
            control.ForeColor = ControlText(instance);
            if (control is ButtonBase b2) b2.UseVisualStyleBackColor = false;
        }

        private static void ApplyLightThemeToControl(Control control)
        {
            if (control is ButtonBase btn)
            {
                btn.BackColor = SystemColors.Control;
                btn.ForeColor = SystemColors.ControlText;
                btn.UseVisualStyleBackColor = true;
                if (btn is Button button)
                {
                    button.FlatAppearance.BorderColor = SystemColors.ControlDark;
                    button.FlatStyle = FlatStyle.Standard;
                }
                return;
            }
            if (control is ListView lv)
            {
                lv.BackColor = SystemColors.Window;
                lv.ForeColor = SystemColors.WindowText;
                return;
            }
            if (control is TreeView tv)
            {
                tv.BackColor = SystemColors.Window;
                tv.ForeColor = SystemColors.WindowText;
                return;
            }
            if (control is ComboBox cb)
            {
                cb.BackColor = SystemColors.Window;
                cb.ForeColor = SystemColors.WindowText;
                return;
            }
            if (control is TextBoxBase tb)
            {
                tb.BackColor = SystemColors.Window;
                tb.ForeColor = SystemColors.WindowText;
                return;
            }
            if (control is LinkLabel link)
            {
                link.BackColor = SystemColors.Control;
                link.ForeColor = SystemColors.ControlText;
                link.LinkColor = Color.Blue;
                link.VisitedLinkColor = Color.Purple;
                link.ActiveLinkColor = SystemColors.ControlText;
                return;
            }
            if (control is Label || control is GroupBox || control is Panel
                || control is Form || control is UserControl || control is TabPage)
            {
                if (control is Panel pnl && pnl.BackColor == Color.Transparent)
                    return;
                control.BackColor = control is Form || control is UserControl || control is Panel || control is TabPage
                    ? SystemColors.Control
                    : control.BackColor;
                control.ForeColor = SystemColors.ControlText;
                return;
            }
            control.BackColor = SystemColors.Control;
            control.ForeColor = SystemColors.ControlText;
            if (control is ButtonBase b2) b2.UseVisualStyleBackColor = true;
        }

        /// <summary>
        /// Recursively apply dark theme only (for callers that only want to run when dark).
        /// Prefer ApplyThemeRecursive so toggling and strips work correctly.
        /// </summary>
        public static void ApplyDarkThemeRecursive(Control control, RadegastInstanceForms instance)
        {
            if (control == null || !IsDark(instance)) return;
            ApplyThemeRecursive(control, instance);
        }

        private static bool ShouldSkipTheming(Control control)
        {
            // Skip 3D / OpenGL and other non-standard controls
            string name = control.Name ?? "";
            string typeName = control.GetType().Name ?? "";
            if (typeName.Contains("Scene") || typeName.Contains("GLControl") || typeName.Contains("OpenGL"))
                return true;
            if (name.IndexOf("scene", StringComparison.OrdinalIgnoreCase) >= 0)
                return true;
            return false;
        }
    }
}
