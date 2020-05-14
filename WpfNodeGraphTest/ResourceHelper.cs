using PropertyTools.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfNodeGraphTest {
    static class ResourceHelper {

        public static string GetResourceFile(string filePath) {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(App));
            string name = assembly
                .GetManifestResourceNames()
                .Single(p => p.EndsWith(filePath));

            var resourceStream = assembly.GetManifestResourceStream(name);
            using (var reader = new StreamReader(resourceStream))
                return reader.ReadToEnd();
        }

        public static StreamReader GetResourceFileStream(string filePath) {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(App));
            string name = assembly
                .GetManifestResourceNames()
                .Single(p => p.EndsWith(filePath));

            var resourceStream = assembly.GetManifestResourceStream(name);
            return new StreamReader(resourceStream);
        }

        public static string Format(this string format, params object[] input) {
            return String.Format(format, input);
        }
    }
}
