using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuestPDF.Helpers;
using Travel.Web.DTOs.BannerDtos;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Services.CategoryServices;
using Travel.Web.Services.DestinationServices;
using Travel.Web.Services.ReservationServices;
using Travel.Web.Services.TourServices;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Travel.Web.DTOs.ReservationDtos;
using Travel.Web.Services.ReservationServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class TourController(ITourService tourService,IMapper mapper,ICategoryService categoryService,IDestinationService destinationService, IReservationService reservationService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tours = await tourService.GetTourByCategoryAsync();
            return View(tours);
        }


        [HttpGet]
        public async Task<ActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await categoryService.GetAllAsync(), "Id", "Name");

            ViewBag.Destinations = new SelectList(await destinationService.GetAllAsync(), "Id", "City");



            return View();
        } 


        [HttpPost]
        public async Task<ActionResult> Create(CreateTourDto createTourDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createTourDto);
            }
            await tourService.CreateAsync(createTourDto);
            return RedirectToAction(nameof(Index));

        }


        public async Task<ActionResult> ToggleTourStatus(string id)
        {
            var hasValue= await tourService.GetTourByIdAsync(id);
            if(hasValue is null)
            {
                return NotFound();
            }

            await tourService.ChangeStatusAsync(hasValue);
            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await tourService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var tour= await tourService.GetTourByIdAsync(id);
            if(tour is null )
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(
                await categoryService.GetAllAsync(),
                "Id",
                "Name",
                tour.CategoryId
                );


            ViewBag.Destinations = new SelectList(
                await destinationService.GetAllAsync(),
                "Id",
                "City",
                tour.DestinationId
    );

            var updateTourDto = mapper.Map<UpdateTourDto>(tour);

            return View(updateTourDto);

            
        }


        [HttpPost]
        public async Task<IActionResult> Edit(UpdateTourDto updateTourDto)
        {
            if (!ModelState.IsValid) {
                return View(updateTourDto);
            }

            await tourService.UpdateAsync(updateTourDto);
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> DownloadParticipantsExcel(string id)
        {
            var tour = await tourService.GetTourByIdAsync(id);

            if (tour == null)
            {
                return NotFound();
            }

            var participants =
                await reservationService.GetTourParticipantReportAsync(id);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Tur Katilimci Raporu");

            string[] headers =
            {
        "Ad Soyad",
        "E-posta",
        "Telefon",
        "Tur",
        "Tur Tarihi",
        "Yetişkin",
        "Çocuk",
        "Toplam Kişi",
        "Rezervasyon Tarihi",
        "Toplam Ücret",
        "Durum"
    };

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
            }

            var headerRange = worksheet.Range(1, 1, 1, headers.Length);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            int row = 2;

            foreach (var item in participants)
            {
                worksheet.Cell(row, 1).Value = item.FullName;
                worksheet.Cell(row, 2).Value = item.Email;
                worksheet.Cell(row, 3).Value = item.Telephone;
                worksheet.Cell(row, 4).Value = item.TourName;

                worksheet.Cell(row, 5).Value =
                    $"{item.StartDate:dd.MM.yyyy} - {item.EndDate:dd.MM.yyyy}";

                worksheet.Cell(row, 6).Value = item.Adult;
                worksheet.Cell(row, 7).Value = item.Child;
                worksheet.Cell(row, 8).Value = item.TotalPeople;

                worksheet.Cell(row, 9).Value =
                    item.ReservationCreatedDate.ToString("dd.MM.yyyy HH:mm");

                worksheet.Cell(row, 10).Value = (double)item.TotalPrice;
                worksheet.Cell(row, 10).Style.NumberFormat.Format = "#,##0.00";

                worksheet.Cell(row, 11).Value = item.Status;

                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            var fileName = $"Tur_Katilimci_Raporu_{id}.xlsx";

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadParticipantsPdf(string id)
        {
            var tour = await tourService.GetTourByIdAsync(id);

            if (tour == null)
            {
                return NotFound();
            }

            var participants =
                await reservationService.GetTourParticipantReportAsync(id);

            using var stream = new MemoryStream();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A3.Landscape());
                    page.Margin(20);
                    page.DefaultTextStyle(x => x.FontSize(8));

                    page.Header()
                        .Text($"Tur Katılımcı Raporu - {tour.Name}")
                        .Bold()
                        .FontSize(16);

                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(1.2f);
                            columns.RelativeColumn(1.8f);
                            columns.RelativeColumn(1.8f);
                            columns.RelativeColumn(0.7f);
                            columns.RelativeColumn(0.7f);
                            columns.RelativeColumn(0.8f);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(1.2f);
                            columns.RelativeColumn(1.1f);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(PdfHeaderCell).Text("Ad Soyad");
                            header.Cell().Element(PdfHeaderCell).Text("E-posta");
                            header.Cell().Element(PdfHeaderCell).Text("Telefon");
                            header.Cell().Element(PdfHeaderCell).Text("Tur");
                            header.Cell().Element(PdfHeaderCell).Text("Tur Tarihi");
                            header.Cell().Element(PdfHeaderCell).Text("Yetişkin");
                            header.Cell().Element(PdfHeaderCell).Text("Çocuk");
                            header.Cell().Element(PdfHeaderCell).Text("Toplam Kişi");
                            header.Cell().Element(PdfHeaderCell).Text("Rezervasyon Tarihi");
                            header.Cell().Element(PdfHeaderCell).Text("Toplam Ücret");
                            header.Cell().Element(PdfHeaderCell).Text("Durum");
                        });

                        foreach (var item in participants)
                        {
                            table.Cell().Element(PdfBodyCell).Text(item.FullName);
                            table.Cell().Element(PdfBodyCell).Text(item.Email);
                            table.Cell().Element(PdfBodyCell).Text(item.Telephone);
                            table.Cell().Element(PdfBodyCell).Text(item.TourName);

                            table.Cell().Element(PdfBodyCell).Text(
                                $"{item.StartDate:dd.MM.yyyy} - {item.EndDate:dd.MM.yyyy}");

                            table.Cell().Element(PdfBodyCell).Text(item.Adult.ToString());
                            table.Cell().Element(PdfBodyCell).Text(item.Child.ToString());
                            table.Cell().Element(PdfBodyCell).Text(item.TotalPeople.ToString());

                            table.Cell().Element(PdfBodyCell).Text(
                                item.ReservationCreatedDate.ToString("dd.MM.yyyy HH:mm"));

                            table.Cell().Element(PdfBodyCell).Text(
                                $"{item.TotalPrice:N2} TL");

                            table.Cell().Element(PdfBodyCell).Text(item.Status);
                        }
                    });
                });
            });

            document.GeneratePdf(stream);

            return File(
                stream.ToArray(),
                "application/pdf",
                $"Tur_Katilimci_Raporu_{id}.pdf");
        }

        private static IContainer PdfHeaderCell(IContainer container)
        {
            return container
                .Background(Colors.Grey.Lighten2)
                .Border(0.5f)
                .Padding(4);
        }

        private static IContainer PdfBodyCell(IContainer container)
        {
            return container
                .Border(0.5f)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(4);
        }


    }
}
