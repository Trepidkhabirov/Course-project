using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using translog_APIшка.Model;

namespace translog_APIшка.Services;

public class PdfNakladnaya
{
    public byte[] GenerateNakladnayaPdf(Order order)
    {
        var driver = order.Vehicle?
            .Drivers?
            .FirstOrDefault();

        var driverName = driver?.User?.FullName ?? "Не указан";

        var vehicleModel = order.Vehicle?.Model ?? "Не указана";
        var brand =order.Vehicle?.Brand ?? "Не указано"; 

        var vehicleNumber = order.Vehicle?.LicensePlate ?? "Не указан";

        var document = QuestPDF.Fluent.Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A5);
                page.Margin(25);
                page.Background().Background(Colors.White);

                page.Content().Column(column =>
                {
                    column.Item().AlignCenter().Column(header =>
                    {
                        header.Item()
                            .Text("ТРАНСПОРТНАЯ НАКЛАДНАЯ")
                            .FontSize(22)
                            .Bold();

                        header.Item()
                            .Text($"№ {order.OrderId}")
                            .FontSize(16);

                        header.Item()
                            .Text($"Дата: {order.ReceivedAt:dd.MM.yyyy}")
                            .FontSize(11);
                    });

                    column.Item().Height(25);

                    column.Item()
                        .Text("Маршрут")
                        .Bold()
                        .FontSize(15);

                    column.Item()
                        .Text($"{order.DeparturePoint} → {order.ArrivalPoint}")
                        .FontSize(13);

                    column.Item().Height(20);

                    void Detail(string title, string value)
                    {
                        column.Item().Row(r =>
                        {
                            r.RelativeItem()
                                .Text(title)
                                .FontSize(12);

                            r.ConstantItem(180)
                                .AlignRight()
                                .Text(value)
                                .FontSize(12)
                                .Bold();
                        });

                        column.Item().Height(8);
                    }

                    Detail("Объём груза", $"{order.VolumeM3} м³");
                    Detail("Вес груза", $"{order.Weight} т");
                    Detail("Расстояние", $"{order.DistanceKm} км");

                    Detail(
                        "Дата отправления",
                        order.DepartureTime?.ToString("dd.MM.yyyy") ?? "-"
                    );

                    Detail(
                        "Дата прибытия",
                        order.ArrivalTime?.ToString("dd.MM.yyyy") ?? "-"
                    );

                    column.Item().Height(20);

                    column.Item()
                        .Text("Информация о водителе")
                        .Bold()
                        .FontSize(15);

                    column.Item().Height(10);

                    Detail("Водитель", driverName);
                    Detail("Модель машины", $"{brand} {vehicleModel}");
                    Detail("Номер машины", vehicleNumber);

                    column.Item().Height(40);

                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("Отправитель");
                            c.Item().Height(20);
                            c.Item().LineHorizontal(1);
                        });

                        row.ConstantItem(40);

                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("Водитель");
                            c.Item().Height(20);
                            c.Item().LineHorizontal(1);
                        });
                    });
                });
            });
        });

        return document.GeneratePdf();
    }
}

