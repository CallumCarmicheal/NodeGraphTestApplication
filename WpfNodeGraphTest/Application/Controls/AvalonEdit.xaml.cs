using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace WpfNodeGraphTest.Application.Controls {
    /// <summary>
    /// Interaction logic for AvalonEdit.xaml
    /// </summary>
    public partial class AvalonEdit {
        public SearchPanel Search { get; set; } = null;


        public AvalonEdit() {
            InitializeComponent();
            Search = SearchPanel.Install(this);

            AvalonEditGlobalStyling.StyleChanged += AvalonEditGlobalStyling_StyleChanged;
            reloadStyle();
        }

        #region Properties

        private AvalonEditGlobalStyling.ColorStyle colorStyle = AvalonEditGlobalStyling.ColorStyle.Dark;
        private AvalonEditGlobalStyling.StyleSetting styleSettings = AvalonEditGlobalStyling.StyleSetting.UseGlobal;
        private AvalonEditGlobalStyling.SyntaxLanguage syntaxLanguage = AvalonEditGlobalStyling.SyntaxLanguage.CSharp;
        
        public AvalonEditGlobalStyling.ColorStyle ColorStyle {
            get {
                if (styleSettings == AvalonEditGlobalStyling.StyleSetting.UseGlobal)
                    return AvalonEditGlobalStyling.ColorTheme;

                return colorStyle;
            }

            set {
                if (styleSettings == AvalonEditGlobalStyling.StyleSetting.UseInstance) {
                    colorStyle = value;

                    reloadStyle();
                }
            }
        }

        public AvalonEditGlobalStyling.StyleSetting StyleSetting {
            get {
                return styleSettings;
            }

            set {
                if (value == AvalonEditGlobalStyling.StyleSetting.UseGlobal) {
                    AvalonEditGlobalStyling.StyleChanged -= AvalonEditGlobalStyling_StyleChanged;
                    AvalonEditGlobalStyling.StyleChanged += AvalonEditGlobalStyling_StyleChanged;
                    reloadStyle();
                } else {
                    AvalonEditGlobalStyling.StyleChanged -= AvalonEditGlobalStyling_StyleChanged;
                }
            }
        }

        public AvalonEditGlobalStyling.SyntaxLanguage Syntax {
            get {
                return syntaxLanguage;
            }

            set {
                syntaxLanguage = value;
                reloadStyle();
            }
        }

        #endregion // Properties

        #region Theming

        Brush themeBackground = null;
        Brush themeTextDefault = null;

        private void AvalonEditGlobalStyling_StyleChanged(AvalonEditGlobalStyling.ColorStyle newStyle) => reloadStyle();

        private void reloadStyle() {
            string languageFile;

            switch (this.syntaxLanguage) {
            case AvalonEditGlobalStyling.SyntaxLanguage.Javascript:
                languageFile = "Resources.Javascript";
                break;
            case AvalonEditGlobalStyling.SyntaxLanguage.XML:
                languageFile = "Resources.XmlDoc";
                break;

            default:
            case AvalonEditGlobalStyling.SyntaxLanguage.CSharp:
                languageFile = "Resources.CSharp";
                break;
            }

            // Check if we are using the dark theme
            bool useDarkTheme = ColorStyle == AvalonEditGlobalStyling.ColorStyle.Dark;

            // If we are using the dark theme load Resources/CSharp-Dark.xshd
            if (useDarkTheme) {
                languageFile += "-Dark.xshd";
                themeBackground = new SolidColorBrush(Color.FromArgb(0xff, 0x35, 0x35, 0x36));
                themeTextDefault = new SolidColorBrush(Color.FromArgb(0xff, 0xab, 0xb2, 0xbf));
            }

            // If we are using the light theme load Resources/CSharp-Light.xshd
            else {
                languageFile += "-Light.xshd";
                themeBackground = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
                themeTextDefault = new SolidColorBrush(Color.FromArgb(0xff, 0x35, 0x35, 0x36));
            }

            // Load the syntax file
            using (var stream = ResourceHelper.GetResourceFileStream(languageFile))
            using (var reader = new XmlTextReader(stream))
                SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);

            // Setup the brushes and set the properties
            themeTextDefault.Freeze();
            themeBackground.Freeze();
            Background = themeBackground;
            Foreground = themeTextDefault;

            // Force a re-render (might not be required)
            InvalidateVisual();
        }
        #endregion
    }
}
