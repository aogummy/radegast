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
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.If not, see<https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace Radegast
{
    public sealed class SettingsForms : Settings
    {
        public static readonly Dictionary<string, FontSetting> DefaultFontSettings = new Dictionary<string, FontSetting>()
        {
            {"Normal", new FontSetting {
                Name = "Normal",
                ForeColor = SystemColors.ControlText.ToSKColor(),
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"StatusBlue", new FontSetting {
                Name = "StatusBlue",
                ForeColor = SKColors.Blue,
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"StatusDarkBlue", new FontSetting {
                Name = "StatusDarkBlue",
                ForeColor = SKColors.DarkBlue,
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"LindenChat", new FontSetting {
                Name = "LindenChat",
                ForeColor = SKColors.DarkGreen,
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"ObjectChat", new FontSetting {
                Name = "ObjectChat",
                ForeColor = SKColors.DarkCyan,
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"StartupTitle", new FontSetting {
                Name = "StartupTitle",
                ForeColor = SystemColors.ControlText.ToSKColor(),
                BackColor = SKColors.Transparent,
                Font = new Font(FontSetting.DefaultFont, FontStyle.Bold),
            }},
            {"Alert", new FontSetting {
                Name = "Alert",
                ForeColor = SKColors.DarkRed,
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"Error", new FontSetting {
                Name = "Error",
                ForeColor = SKColors.Red,
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"OwnerSay", new FontSetting {
                Name = "OwnerSay",
                ForeColor = SKColors.DarkGoldenrod,
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"Timestamp", new FontSetting {
                Name = "Timestamp",
                ForeColor = SystemColors.GrayText.ToSKColor(),
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"Name", new FontSetting {
                Name = "Name",
                ForeColor = SystemColors.ControlText.ToSKColor(),
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"Notification", new FontSetting {
                Name = "Notification",
                ForeColor = SystemColors.ControlText.ToSKColor(),
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"IncomingIM", new FontSetting {
                Name = "IncomingIM",
                ForeColor = SystemColors.ControlText.ToSKColor(),
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"OutgoingIM", new FontSetting {
                Name = "OutgoingIM",
                ForeColor = SystemColors.ControlText.ToSKColor(),
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"Emote", new FontSetting {
                Name = "Emote",
                ForeColor = SystemColors.ControlText.ToSKColor(),
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"Self", new FontSetting {
                Name = "Self",
                ForeColor = SystemColors.ControlText.ToSKColor(),
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"MentionMe", new FontSetting {
                Name = "MentionMe",
                ForeColor = SystemColors.ControlText.ToSKColor(),
                BackColor = SKColors.Yellow,
                Font = FontSetting.DefaultFont,
            }},
            {"MentionOthers", new FontSetting {
                Name = "MentionOthers",
                ForeColor = SystemColors.ControlText.ToSKColor(),
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
        };

        /// <summary>Default chat font/color settings for dark mode (readable on dark backgrounds).</summary>
        public static readonly Dictionary<string, FontSetting> DefaultDarkFontSettings = new Dictionary<string, FontSetting>()
        {
            {"Normal", new FontSetting {
                Name = "Normal",
                ForeColor = SKColors.White,
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"StatusBlue", new FontSetting {
                Name = "StatusBlue",
                ForeColor = SKColors.CornflowerBlue,
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"StatusDarkBlue", new FontSetting {
                Name = "StatusDarkBlue",
                ForeColor = SKColors.LightSteelBlue,
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"LindenChat", new FontSetting {
                Name = "LindenChat",
                ForeColor = SKColors.LightGreen,
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"ObjectChat", new FontSetting {
                Name = "ObjectChat",
                ForeColor = SKColors.Cyan,
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"StartupTitle", new FontSetting {
                Name = "StartupTitle",
                ForeColor = SKColors.White,
                BackColor = SKColors.Transparent,
                Font = new Font(FontSetting.DefaultFont, FontStyle.Bold),
            }},
            {"Alert", new FontSetting {
                Name = "Alert",
                ForeColor = SKColors.Salmon,
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"Error", new FontSetting {
                Name = "Error",
                ForeColor = SKColors.LightCoral,
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"OwnerSay", new FontSetting {
                Name = "OwnerSay",
                ForeColor = SKColors.Gold,
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"Timestamp", new FontSetting {
                Name = "Timestamp",
                ForeColor = SKColors.DarkGray,
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"Name", new FontSetting {
                Name = "Name",
                ForeColor = SKColors.White,
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"Notification", new FontSetting {
                Name = "Notification",
                ForeColor = SKColors.White,
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"IncomingIM", new FontSetting {
                Name = "IncomingIM",
                ForeColor = SKColors.White,
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"OutgoingIM", new FontSetting {
                Name = "OutgoingIM",
                ForeColor = SKColors.White,
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"Emote", new FontSetting {
                Name = "Emote",
                ForeColor = SKColors.White,
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"Self", new FontSetting {
                Name = "Self",
                ForeColor = SKColors.White,
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
            {"MentionMe", new FontSetting {
                Name = "MentionMe",
                ForeColor = SKColors.White,
                BackColor = SKColors.DarkGoldenrod,
                Font = FontSetting.DefaultFont,
            }},
            {"MentionOthers", new FontSetting {
                Name = "MentionOthers",
                ForeColor = SKColors.White,
                BackColor = SKColors.Transparent,
                Font = FontSetting.DefaultFont,
            }},
        };

        /// <summary>Returns the default font settings for the given theme (light or dark).</summary>
        public static Dictionary<string, FontSetting> GetDefaultFontSettings(bool isDarkMode)
            => isDarkMode ? DefaultDarkFontSettings : DefaultFontSettings;

        public class FontSetting
        {
            [IgnoreDataMember]
            public static readonly Font DefaultFont = new Font(FontFamily.GenericSansSerif, 8.0f);

            [IgnoreDataMember]
            public Font Font;

            [IgnoreDataMember]
            public SKColor ForeColor;

            [IgnoreDataMember]
            public SKColor BackColor;


            public string Name;

            public string ForeColorString
            {
                get => ForeColor.Alpha == 0 ? "Transparent" : ColorTranslator.ToHtml(ForeColor.ToDrawingColor());
                set => ForeColor = string.Equals(value, "Transparent", StringComparison.OrdinalIgnoreCase)
                    ? SKColors.Transparent
                    : ColorTranslator.FromHtml(value).ToSKColor();
            }

            public string BackColorString
            {
                get => BackColor.Alpha == 0 ? "Transparent" : ColorTranslator.ToHtml(BackColor.ToDrawingColor());
                set => BackColor = string.Equals(value, "Transparent", StringComparison.OrdinalIgnoreCase)
                    ? SKColors.Transparent
                    : ColorTranslator.FromHtml(value).ToSKColor();
            }

            public string FontString
            {
                get
                {
                    if (Font != null)
                    {
                        TypeConverter converter = TypeDescriptor.GetConverter(typeof(Font));
                        return converter.ConvertToString(Font);
                    }
                    return null;
                }
                set
                {
                    try
                    {
                        TypeConverter converter = TypeDescriptor.GetConverter(typeof(Font));
                        Font = converter.ConvertFromString(value) as Font;
                    }
                    catch (Exception)
                    {
                        Font = DefaultFont;
                    }

                }
            }

            public override string ToString()
            {
                return Name;
            }
        }

        public SettingsForms(string fileName) : base(fileName)
        {

        }
    }
}
