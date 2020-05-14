using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNodeGraphTest.Application.Controls {
   

    public static class AvalonEditGlobalStyling {
        public delegate void DStyleChanged(ColorStyle newStyle);
        public static event DStyleChanged StyleChanged;

        public enum StyleSetting {
            UseGlobal,
            UseInstance
        }

        public enum ColorStyle {
            Light,
            Dark
        }

        private static ColorStyle _ColorTheme = ColorStyle.Dark;
        public static ColorStyle ColorTheme {
            get { return _ColorTheme; }
            set {
                if (value != _ColorTheme) {
                    _ColorTheme = value;
                    StyleChanged?.Invoke(value);
                }
            }
        }
    }
}
