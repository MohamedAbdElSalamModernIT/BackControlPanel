
using ClosedXML.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Infrastructure
{
    public class ExcelService : IExcelService
    {
        public byte[] GenerateExcell<T>(IEnumerable<T> records)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var excel = new ExcelPackage();
            var worksheet = excel.Workbook.Worksheets.Add("Sheet1");
            worksheet.TabColor = Color.Black;
            worksheet.DefaultRowHeight = 14;
            worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Row(1).Height = 20;
            worksheet.Row(1).Style.Font.Bold = true;

            char GetLetter(int order) => "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[order];

            var headers = typeof(T).GetProperties().Select(e => e.Name).ToList();


            Color colFromHex = ColorTranslator.FromHtml("#01bab4");

            void StyleHeader(ExcelWorksheet worksheet, Type type, int startRow, int startCol)
            {
                var headers = type.GetProperties().Select(e => e.Name).ToList();
                for (int i = 0; i < headers.Count; i++)
                {
                    // header style
                    worksheet.Row(startRow).Height = 18;
                    worksheet.Row(startRow).Style.Font.Bold = true;
                    worksheet.Cells[startRow, i + startCol].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.White);
                    worksheet.Cells[startRow, i + startCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[startRow, i + startCol].Style.Fill.BackgroundColor.SetColor(colFromHex);
                    worksheet.Cells[startRow, i + startCol].Style.Font.Color.SetColor(Color.White);
                    // header value
                    worksheet.Cells[startRow, i + startCol].Value = headers[i];
                }
            }

            void AddDataToExcelsheet<Td>(ExcelWorksheet worksheet, Td[] data, int startRow, int startCol) 
            {
                var headers = typeof(Td).GetProperties().Select(e => e.Name).ToList();
                for (int i = 0; i < headers.Count; i++)
                {
                    // adding column header values
                    StyleHeader(worksheet, typeof(Td), startRow, startCol);
                    // adding column values
                    for (int j = 0; j < data.Length; j++)
                    {
                        worksheet.Cells[j + startRow + 1, i + startCol].Value = data[j]
                            .GetType().GetProperty(headers[i])?
                            .GetValue(data[j])?.ToString();
                    }
                }
            }

            AddDataToExcelsheet(worksheet, records.ToArray(), 1, 1);

            worksheet.Cells.AutoFitColumns();
            var bytes = excel.GetAsByteArray();


            return bytes;

        }


        public XLWorkbook GenerateExcel<T>(IEnumerable<T> records)
        {
            var obj = records.FirstOrDefault();
            var headers = typeof(T).GetProperties().Select(e => e.Name).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("sheet1");
                var currentRow = 2;
                var headerCol = 1;
                worksheet.Clear();
                worksheet.SetAutoFilter(true);
                worksheet.Row(1).Height = 18;
                foreach (var head in headers)
                {
                    worksheet.Cell(1, headerCol).Value = head;
                    worksheet.AutoFilter.Column(headerCol).FilterType = XLFilterType.Regular;
                    worksheet.Cell(1, headerCol).Style.Fill.BackgroundColor = XLColor.BlueGray;
                    worksheet.Cell(1, headerCol).Style.Font.FontColor = XLColor.White;
                    worksheet.Cell(1, headerCol).Style.Font.Bold = true;
                    worksheet.Cell(1, headerCol).DataType = XLDataType.Text;
                    worksheet.Cell(1, headerCol).Style.Font.VerticalAlignment = XLFontVerticalTextAlignmentValues.Baseline;

                    worksheet.Cell(1, headerCol).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    headerCol++;
                }


                foreach (var record in records)
                {
                    var list = record;
                    for (int i = 0, j = 1; i < headers.Count; j++, i++)
                    {
                        worksheet.Cell(currentRow, j).DataType = XLDataType.Text;
                        worksheet.Cell(currentRow, j).Value = record.GetType()
                          .GetProperty(headers[i])
                          ?.GetValue(record)
                          ?.ToString();
                    }

                    worksheet.Row(currentRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Row(currentRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    currentRow++;
                }

                worksheet.Columns().AdjustToContents();

                return workbook;
            }
        }
        public XLWorkbook GenerateExcelDictionary(IEnumerable<IDictionary<string, object>> records)
        {
            var obj = records.FirstOrDefault();

            IDictionary<string, object> headers = obj;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("sheet1");
                var currentRow = 2;
                var headerCol = 1;
                worksheet.Clear();
                worksheet.SetAutoFilter(true);
                worksheet.Row(1).Height = 20;
                if (headers != null)
                {
                    foreach (var head in headers.Keys)
                    {
                        worksheet.Cell(1, headerCol).Value = head;
                        worksheet.AutoFilter.Column(headerCol).FilterType = XLFilterType.Regular;
                        worksheet.Cell(1, headerCol).Style.Fill.BackgroundColor = XLColor.BlueGray;
                        worksheet.Cell(1, headerCol).Style.Font.FontColor = XLColor.White;
                        worksheet.Cell(1, headerCol).Style.Font.Bold = true;
                        worksheet.Cell(1, headerCol).DataType = XLDataType.Text;
                        worksheet.Cell(1, headerCol).Style.Font.VerticalAlignment = XLFontVerticalTextAlignmentValues.Baseline;
                        worksheet.Cell(1, headerCol).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        headerCol++;
                    }

                    foreach (IDictionary<string, object> record in records)
                    {
                        var list = record.Values.ToList();
                        for (int i = 0, j = 1; i < list.Count; j++, i++)
                        {
                            worksheet.Cell(currentRow, j).Value = list[i].ToString();
                        }

                        worksheet.Row(currentRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        worksheet.Row(currentRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        currentRow++;
                    }
                }
                worksheet.Columns().AdjustToContents();
                return workbook;
            }
        }
    }
}