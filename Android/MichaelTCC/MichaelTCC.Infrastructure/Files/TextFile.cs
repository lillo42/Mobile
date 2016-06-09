using Java.IO;
using System.Text;

namespace MichaelTCC.Infrastructure.Files
{
    public class TextFile
    {
        public static void Save(File filesDir, string filename, string text)
        {
            var file = new File(filesDir, filename);
            try
            {
                using (var fileWriter = new FileWriter(file))
                {
                    fileWriter.Write(text);
                    fileWriter.Flush();
                }
            }
            finally
            {
                if (file != null)
                    file.Dispose();
            }
        }

        public static string Read(File filesDir, string filename)
        {
            BufferedReader br = null;
            var sb = new StringBuilder();
            var file = new File(filesDir, filename);
            try
            {
                using (var fileReader = new FileReader(file))
                {
                    br = new BufferedReader(fileReader);
                    string line = string.Empty;
                    while ((line = br.ReadLine()) != null)
                        sb.Append(line);
                }
            }
            finally
            {
                if(br != null)
                {
                    br.Close();
                    br.Dispose();
                }

                if (file != null)
                    file.Dispose();
            }

            return sb.ToString();
        }

    }
}