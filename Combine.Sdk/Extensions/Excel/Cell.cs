using System;
using Newtonsoft.Json;
using DocumentFormat.OpenXml;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using Combine.Sdk.Extensions.CommonObjects;

namespace Combine.Sdk.Extensions.Excel
{
  /// <summary>
  /// Provides extension methods for Cell type objects
  /// </summary>
  public static class CellExtensions
  {
    /// <summary>
    /// Checks if the given Cell value is null
    /// </summary>
    /// <param name="cell">Cell to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this Cell cell)
    {
      return cell == null;
    }

    /// <summary>
    /// Sets the cell representation value by
    /// determining the object type
    /// </summary>
    /// <param name="cell">Cell reference</param>
    /// <param name="e">Value</param>
    /// <returns>Task State</returns>
    public static async Task SetValue(this Cell cell, object e)
    {
      if (cell.IsNotValid()) return;
      e = e == DBNull.Value ? null : e;
      e = e ?? @"";
      CellValue value;
      EnumValue<CellValues> dataType = new EnumValue<CellValues>(CellValues.String);
      if (e.IsString())
      {
        value = new CellValue($"{e}");
      }
      else if (e.IsNumber())
      {
        value = new CellValue($"{e}");
        dataType = new EnumValue<CellValues>(CellValues.Number);
      }
      else if (e.IsDateTime())
      {
        value = new CellValue($"{e:G}");
      }
      else if (e.IsTimeSpan())
      {
        value = new CellValue($"{e:T}");
      }
      else
      {
        value = new CellValue(await Task.Run(() => JsonConvert.SerializeObject(e)));
      }
      cell.CellValue = value;
      cell.DataType = dataType;
    }
  }
}
