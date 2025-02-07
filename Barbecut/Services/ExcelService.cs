using Barbecut.DTO;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barbecut.Services
{
	public class ExcelService
	{
		public async void ShareConsumption(EventDTO _eventDTO, List<PayPersonDTO> _listPayPersonDTO, List<OperationPayDTO> _listOperationPayDTO)
		{
			string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"EventConsumption_{_eventDTO.Name}.xlsx");

			using (var workbook = new XLWorkbook())
			{
				var worksheet = workbook.Worksheets.Add("Consumption Summary");

				worksheet.Cell(1, 1).Value = "EVENT CONSUMPTION SUMMARY";
				worksheet.Cell(1, 1).Style.Font.Bold = true;
				worksheet.Range("A1:G1").Merge();
				worksheet.Range("A1:G1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
				worksheet.Cell(2, 1).Style.Font.Bold = true;
				worksheet.Cell(2, 1).Value = _eventDTO.Name;
				worksheet.Range("A2:G2").Merge();
				worksheet.Range("A2:G2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
				worksheet.Cell(3, 1).Value = "Date";
				worksheet.Cell(3, 1).Style.Font.Bold = true;
				worksheet.Cell(3, 2).Value = _eventDTO.Date.ToString("dd/MM/yyyy");

				int row = 5;
				worksheet.Cell(row, 1).Value = "Item";
				worksheet.Cell(row, 1).Style.Font.Bold = true;
				worksheet.Range(worksheet.Cell(row, 1), worksheet.Cell(row, 4)).Merge();
				worksheet.Cell(row, 5).Value = "Who Bought";
				worksheet.Cell(row, 5).Style.Font.Bold = true;
				worksheet.Range(worksheet.Cell(row, 5), worksheet.Cell(row, 8)).Merge();
				worksheet.Cell(row, 9).Value = "Amount Paid";
				worksheet.Cell(row, 9).Style.Font.Bold = true;
				worksheet.Range(worksheet.Cell(row, 9), worksheet.Cell(row, 11)).Merge();
				worksheet.Cell(row, 12).Value = "Consume List";
				worksheet.Cell(row, 12).Style.Font.Bold = true;
				worksheet.Range(worksheet.Cell(row,  12), worksheet.Cell(row, 17)).Merge();
				worksheet.Cell(row, 18).Value = "Value per Person";
				worksheet.Cell(row, 18).Style.Font.Bold = true;
				worksheet.Range(worksheet.Cell(row, 18), worksheet.Cell(row, 20)).Merge();

				foreach (var payPerson in _listPayPersonDTO)
				{
					row++;
					worksheet.Cell(row, 1).Value = payPerson.Item;
					worksheet.Range(worksheet.Cell(row, 1), worksheet.Cell(row, 4)).Merge();
					worksheet.Cell(row, 5).Value = payPerson.NamePerson;
					worksheet.Range(worksheet.Cell(row, 5), worksheet.Cell(row, 8)).Merge();
					worksheet.Cell(row, 9).Value = $"R$ {payPerson.ItemTotal.ToString("F2")}";
					worksheet.Range(worksheet.Cell(row, 9), worksheet.Cell(row, 11)).Merge();
					worksheet.Cell(row, 12).Value = string.Join(", ", payPerson.ListConsumePersonItem);
					worksheet.Range(worksheet.Cell(row, 12), worksheet.Cell(row, 17)).Merge();
					worksheet.Cell(row, 18).Value = $"R$ {payPerson.PriceForPerson.ToString("F2")}";
					worksheet.Range(worksheet.Cell(row, 18), worksheet.Cell(row, 20)).Merge();
				}

				row += 2;
				worksheet.Cell(row, 1).Value = "Operations";
				worksheet.Cell(row, 1).Style.Font.Bold = true;
				worksheet.Range(worksheet.Cell(row, 1), worksheet.Cell(row, 4)).Merge();
				row++;
				worksheet.Cell(row, 1).Value = "Who";
				worksheet.Cell(row, 1).Style.Font.Bold = true;
				worksheet.Range(worksheet.Cell(row, 1), worksheet.Cell(row, 4)).Merge();
				worksheet.Cell(row, 5).Value = "Pay";
				worksheet.Cell(row, 5).Style.Font.Bold = true;
				worksheet.Range(worksheet.Cell(row, 5), worksheet.Cell(row, 8)).Merge();
				worksheet.Cell(row, 9).Value = "To";
				worksheet.Cell(row, 9).Style.Font.Bold = true;
				worksheet.Range(worksheet.Cell(row, 9), worksheet.Cell(row, 11)).Merge();

				foreach (var operation in _listOperationPayDTO)
				{
					row++;
					worksheet.Cell(row, 1).Value = operation.ReceivePayPerson;
					worksheet.Range(worksheet.Cell(row, 1), worksheet.Cell(row, 4)).Merge();
					worksheet.Cell(row, 5).Value = $"R$ {operation.Value.ToString("F2")}";
					worksheet.Range(worksheet.Cell(row, 5), worksheet.Cell(row, 8)).Merge();
					worksheet.Cell(row, 9).Value = operation.PayPerson;
					worksheet.Range(worksheet.Cell(row, 9), worksheet.Cell(row, 11)).Merge();

				}

				workbook.SaveAs(filePath);
			}

			try
			{
				await Share.Default.RequestAsync(new ShareFileRequest
				{
					Title = "Abrir Excel",
					File = new ShareFile(filePath)
				});
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Erro", $"Falha ao abrir o arquivo: {ex.Message}", "OK");
			}
		}
	}
}
