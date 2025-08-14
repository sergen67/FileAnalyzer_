# FileAnalyzer (TXT/DOCX/PDF)

Konsol uygulaması: dosya seç -> içerik oku -> kelime & noktalama analizi -> sonuçları yazdır + logla.

## Özellikler
- TXT (UTF-8, TR fallback), DOCX (DocX), PDF (PdfPig)
- Noktalama İşareti sayımı
- Serilog + ILogger ile `Logs/` klasörüne günlük `.txt` log

## Kurulum
- .NET Framework 4.7.2/4.8
- NuGet: `Serilog`, `Serilog.Extensions.Logging`, `Serilog.Sinks.File`, `UglyToad.PdfPig`, `Xceed.Words.NET`

## Çalıştırma
Uygulamayı başlat -> tür seç (`1=TXT, 2=DOCX, 3=PDF`) -> dosya seç -> sonuçlar konsolda.

