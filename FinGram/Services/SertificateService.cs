using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;
namespace FinGram.Services
{
    public class CertificateService
    {
        public byte[] GenerateCertificate(string userName, string courseName, int score, DateTime date)
        {
            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Header().Text("Сертификат о прохождении курса").SemiBold().FontSize(28).AlignCenter();
                    page.Content().PaddingVertical(40).Column(col =>
                    {
                        col.Item().Text($"Участник: {userName}");
                        col.Item().Text($"Курс: {courseName}");
                        col.Item().Text($"Дата завершения: {date:dd.MM.yyyy}");
                        col.Item().Text($"Результат: {score} баллов");
                        col.Item().PaddingTop(20).Text("Поздравляем с успешным завершением курса!");
                    });
                    page.Footer().AlignCenter().Text("FinGram · Онлайн-платформа по финансовой грамотности");
                });
            }).GeneratePdf();

            return pdfBytes;
        }
    }
}
