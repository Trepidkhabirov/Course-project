using QuestPDF;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using QuestPDF.Fluent;
using System.Reflection.Metadata;
using translog_APIшка.Model;
namespace translog_APIшка.Services;

public class pdfService
{
  public byte[] GenerateReceiptPdf(Order order)
{
    var document = QuestPDF.Fluent.Document.Create(container =>
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A5);
            page.Margin(30);
            page.Background().Background(Colors.White);

            page.Content().Column(column =>
            {
                column.Item().Padding(20).Column(header =>
                {
                    header.Item().Text("ТРАНСЛОГ").FontSize(28).Bold()
                        .FontColor(Colors.Black).AlignCenter();
                    header.Item().Text("ГРУЗОПЕРЕВОЗКИ").FontSize(10).AlignCenter().LetterSpacing(0.2f);
                });

                column.Item().Height(15);

                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text("КВИТАНЦИЯ ОБ ОПЛАТЕ").FontSize(11) .Bold();
                        c.Item().Text($"№ {order.OrderId}").FontSize(22).Bold();
                    });
                    row.ConstantItem(120).Column(c =>
                    {
                        c.Item().Text("ДАТА").FontSize(10).AlignRight();
                        c.Item().Text($"{order.ReceivedAt:dd.MM.yyyy}").FontSize(14)
                            .Bold().AlignRight();
                    });
                });
                column.Item().Padding(15).Row(row =>
                {
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text("ОТКУДА").FontSize(9).FontColor("#7a8ba8");
                        c.Item().Text(order.DeparturePoint ?? "—").FontSize(13)
                            .Bold();
                    });
                    row.ConstantItem(30).AlignMiddle().Text("→").FontSize(18).AlignCenter();
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text("КУДА").FontSize(9).FontColor("#7a8ba8");
                        c.Item().Text(order.ArrivalPoint ?? "—").FontSize(13)
                            .Bold().FontColor("#1D2D50");
                    });
                });

                void DetailRow(ColumnDescriptor col, string label, string value)
                {
                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text(label).FontSize(12).FontColor("#7a8ba8");
                        r.ConstantItem(150).Text(value).FontSize(12).Bold()
                            .FontColor("#1D2D50").AlignRight();
                    });
                    col.Item().Height(8);
                }

                DetailRow(column, "Объём груза", $"{order.VolumeM3} м³");
                DetailRow(column, "Дистанция", $"{order.DistanceKm} км");
                DetailRow(column, "Дата отправления", $"{order.DepartureTime:dd.MM.yyyy}");
                DetailRow(column, "Дата прибытия", $"{order.ArrivalTime:dd.MM.yyyy}");

                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("ИТОГО К ОПЛАТЕ").FontSize(13).Bold();
                    row.ConstantItem(150).Text($"{order.Price} рублей").FontSize(20)
                        .Bold().AlignRight();
                });

                
            });
        });
    });

    return document.GeneratePdf();
}
}