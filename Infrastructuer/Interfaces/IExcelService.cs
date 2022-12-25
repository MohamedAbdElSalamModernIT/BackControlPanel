

using ClosedXML.Excel;
using System.Collections.Generic;

namespace Infrastructure
{
    public interface IExcelService
    {
        byte[] GenerateExcell<T>(IEnumerable<T> records);
        XLWorkbook GenerateExcel<T>(IEnumerable<T> records);
        XLWorkbook GenerateExcelDictionary(IEnumerable<IDictionary<string, object>> records);
    }
}

