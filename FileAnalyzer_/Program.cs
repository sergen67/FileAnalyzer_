using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using FileAnalyzer_.Extensions;
using ILogger = Microsoft.Extensions.Logging.ILogger;


namespace FileAnalyzer_
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var logsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            Directory.CreateDirectory(logsDir);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine(logsDir, "log-.txt"),
                              rollingInterval: RollingInterval.Day,
                              retainedFileCountLimit: 7,
                              shared: true)
                .CreateLogger();

            var loggerFactory = LoggerFactory.Create(b => b.AddSerilog(dispose: true));
            ILogger logger = loggerFactory.CreateLogger<Program>();


            //Dosya Türü Seçimi ve Çıktı
            try
            {
                logger.LogInformation("-----HOŞGELDİNİZ-----");

                var chosenExt = AskFileType(logger);
                if (chosenExt == null)
                {
                    Console.WriteLine("Dosya türü seçilmedi. Program sonlandırılıyor.");
                    return;
                }


                var filter = BuildFilter(chosenExt);


                string selectedPath = null;
                using (var dlg = new OpenFileDialog
                {
                    Filter = filter,
                    Title = $"{chosenExt.ToUpperInvariant()} dosyası seçiniz",
                    DefaultExt = chosenExt,
                    FilterIndex = 1,
                    CheckFileExists = true,
                    Multiselect = false
                })
                {
                    if (dlg.ShowDialog() != DialogResult.OK)
                    {
                        logger.LogInformation("Dosya seçimi iptal edildi.");
                        Console.WriteLine("Dosya seçilmedi. Program sonlandırılıyor.");
                        return;
                    }
                    selectedPath = dlg.FileName;
                }


                IFileReader reader = FileReaderFactory.Create(selectedPath, loggerFactory);
                string content = reader.ReadContent(selectedPath) ?? string.Empty;

                var res = TextAnalyzer.Analyze(content);

                Console.WriteLine("Dosya Analizi Sonuçları:");
                Console.WriteLine($"Farklı Kelime Sayısı: {res.DistinctWordCount}");

                Console.WriteLine("\nKelimeler (en çoktan en aza):");
                foreach (var w in res.TopWords)
                    Console.WriteLine($"{w.Word}: {w.Count}");

                Console.WriteLine("\nNoktalama İşaretleri:");
                foreach (var kvp in res.PunctuationCounts)
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");

                logger.LogInformation("Analiz tamamlandı. Dosya: {File}", selectedPath);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Dosya analizi sırasında hata oluştu");
                Console.WriteLine("Hata: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Analiz tamamlandı. Çıkmak için bir tuşa basın...");
                Console.ReadKey();
                Log.CloseAndFlush();
            }
        }


        private static string AskFileType(ILogger logger)
        {
            while (true)
            {
                Console.Write("Hangi tür? 1=TXT, 2=DOCX, 3=PDF  (çıkmak için Q): ");
                var key = Console.ReadKey(intercept: true).KeyChar;
                Console.WriteLine();

                switch (char.ToLowerInvariant(key))
                {
                    case '1': logger.LogInformation("Tür seçildi: TXT"); return ".txt";
                    case '2': logger.LogInformation("Tür seçildi: DOCX"); return ".docx";
                    case '3': logger.LogInformation("Tür seçildi: PDF"); return ".pdf";
                    case 'q': logger.LogInformation("Kullanıcı çıktı"); return null;
                    default: Console.WriteLine("Geçersiz seçim, tekrar dene."); break;
                }
            }
        }

        private static string BuildFilter(string ext)
        {
            if (string.IsNullOrWhiteSpace(ext)) return "All Files (*.*)|*.*";
            ext = ext.Trim().ToLowerInvariant();
            if (!ext.StartsWith(".")) ext = "." + ext;

            switch (ext)
            {
                case ".txt": return "Text Files (*.txt)|*.txt";
                case ".docx": return "Word Documents (*.docx)|*.docx";
                case ".pdf": return "PDF Files (*.pdf)|*.pdf";
                default: return "All Files (*.*)|*.*";
            }
        }
    }
}
