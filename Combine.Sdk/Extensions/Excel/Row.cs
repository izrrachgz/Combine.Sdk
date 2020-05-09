using System.Linq;
using DocumentFormat.OpenXml;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Spreadsheet;
using Combine.Sdk.Extensions.CommonObjects;

namespace Combine.Sdk.Extensions.Excel
{
  /// <summary>
  /// Provides extension methods for Row type objects
  /// </summary>
  public static class RowExtensions
  {
    /// <summary>
    /// Checks if the given Row value is null or it does not contain
    /// any cell in it
    /// </summary>
    /// <param name="row">Row to evaluate</param>
    /// <returns>True or False</returns>
    public static bool IsNotValid(this Row row)
    {
      return row == null || !row.Descendants<Cell>().Any();
    }

    /// <summary>
    /// Gets the cell at the given index from the
    /// row
    /// </summary>
    /// <param name="row">Row reference</param>
    /// <param name="index">Index position</param>
    /// <returns>Cell</returns>
    public static Cell GetCell(this Row row, uint index = 0)
    {
      if (row.IsNotValid() || row.Descendants<Cell>().Count() - 1 < index) return null;
      return row.Descendants<Cell>().ElementAt((int)index);
    }

    /// <summary>
    /// Gets the cell at the specified reference from the
    /// row
    /// </summary>
    /// <param name="row">Row reference</param>
    /// <param name="reference">Reference position</param>
    /// <returns>Cell</returns>
    public static Cell GetCell(this Row row, string reference)
    {
      if (row.IsNotValid() || reference.IsNotValid())
        return null;
      return row.Descendants<Cell>()
        .FirstOrDefault(c => c.CellReference.InnerText.Equals(reference));
    }

    /// <summary>
    /// Gets the cell index position in the row
    /// at the specified cell reference
    /// </summary>
    /// <param name="row">Row reference</param>
    /// <param name="reference">Reference position</param>
    /// <returns>Index position</returns>
    public static int GetCellIndex(this Row row, string reference)
    {
      if (row.IsNotValid() || reference.IsNotValid())
        return -1;
      Cell cell = row.Descendants<Cell>()
        .FirstOrDefault(c => c.CellReference.InnerText.Equals(reference));
      if (cell.IsNotValid())
        return -1;
      return row.Descendants<Cell>()
        .ToList()
        .IndexOf(cell);
    }

    /// <summary>
    /// Changes the two specified columns in the
    /// row
    /// </summary>
    /// <param name="row">Row reference</param>
    /// <param name="first">Index position column to swap</param>
    /// <param name="swap">Index position column to swap for</param>
    public static void SwapColumns(this Row row, uint first, uint swap)
    {
      if (row.IsNotValid() || first.Equals(swap))
        return;
      int total = row.Descendants<Cell>().Count();
      if (first > total || swap > total)
        return;
      IEnumerable<Cell> cells = row.Descendants<Cell>();
      Cell o = cells.ElementAt((int)first);
      Cell d = cells.ElementAt((int)swap);
      o.Remove();
      d.Remove();
      row.InsertAt(d, (int)first);
      row.InsertAt(o, (int)swap);
    }

    /// <summary>
    /// Changes the two specified columns in the
    /// row
    /// </summary>
    /// <param name="row">Row reference</param>
    /// <param name="first">Reference position column to swap</param>
    /// <param name="swap">Reference position column to swap for</param>
    public static void SwapColumns(this Row row, string first, string swap)
    {
      if (row.IsNotValid() || first.IsNotValid() || swap.IsNotValid() || first.Equals(swap))
        return;
      int start = row.GetCellIndex(first);
      int end = row.GetCellIndex(swap);
      if (start < 0 || end < 0)
        return;
      row.SwapColumns((uint)start, (uint)end);
    }
  }
}
