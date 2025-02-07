using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barbecut.DTO
{
	public class OperationPayDTO
	{
		public string PayPerson { get; set; } = string.Empty;
		public string ReceivePayPerson { get; set; } = string.Empty;
		public decimal Value {  get; set; } 
	}
}
