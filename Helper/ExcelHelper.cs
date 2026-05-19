using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static TrOCR.Helper.BaiduOcrHelper;  // 引用百度单元格类
using static TrOCR.Helper.TencentOcrHelper; // 引用腾讯单元格类

namespace TrOCR.Helper
{
    //导出的excel表格样式和剪贴板粘贴到excel的表格样式不同
    public static class ExcelHelper
    {
        // 重载方法，用于处理百度的 CellInfo
        public static void ExportToExcel(List<CellInfo> bodyCells, List<string> headerTexts, List<string> footerTexts, Form owner)
        {
            // 将百度的 CellInfo 转换为通用的 TableCell
            var genericCells = bodyCells.Select(c => new TableCell
            {
                Text = c.Words,
                Row = c.RowStart,
                Col = c.ColStart,
                // 注意百度的 end 是不包含的边界，所以跨度是 end - start
                RowSpan = c.RowEnd - c.RowStart,
                ColSpan = c.ColEnd - c.ColStart
            }).ToList();

            GenerateExcelFile(genericCells, headerTexts, footerTexts, owner);
        }

        // 重载方法，用于处理腾讯的 TableCell
        public static void ExportToExcel(List<TableCell> bodyCells, List<string> headerTexts, List<string> footerTexts, Form owner)
        {
            GenerateExcelFile(bodyCells, headerTexts, footerTexts, owner);
        }

        // 核心的 Excel 文件生成逻辑
        private static void GenerateExcelFile(List<TableCell> cells, List<string> headerTexts, List<string> footerTexts, Form owner)
        {
            if (cells == null || !cells.Any())
            {
                MessageBox.Show("没有可供导出的表格数据。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel 文件|*.xlsx";
                sfd.ValidateNames = true;
                sfd.FileName = $"TrOCR_表格_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (sfd.ShowDialog(owner) == DialogResult.OK)
                {
                    try
                    {
                        Action work = () =>
                        {
                            using (var workbook = new XLWorkbook())
                            {
                                var worksheet = workbook.Worksheets.Add("识别结果");

                                // --- 优化 1: 全局样式预设 ---
                                // 为整个工作表设置基础样式，这比在循环中逐个设置快几个数量级
                                //worksheet.Style.Font.FontName = "等线";
                                //worksheet.Style.Font.FontSize = 11;
                                worksheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center; // 所有单元格默认垂直居中
                                worksheet.Style.Alignment.WrapText = true; // 所有单元格默认自动换行

                                int currentRow = 1;
                                int columnCount = cells.Any() ? cells.Max(c => c.Col + c.ColSpan) : 1;

                                // --- 优化 2: 高效写入表头 ---
                                if (headerTexts != null && headerTexts.Any())
                                {
                                    var headerRange = worksheet.Range(currentRow, 1, currentRow + headerTexts.Count - 1, columnCount);
                                    // 批量设置表头样式
                                    headerRange.Style.Font.Bold = true;
                                    headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                    // 批量填充数据和合并
                                    for (int i = 0; i < headerTexts.Count; i++)
                                    {
                                        worksheet.Cell(currentRow + i, 1).Value = headerTexts[i];
                                        if (columnCount > 1) worksheet.Range(currentRow + i, 1, currentRow + i, columnCount).Merge();
                                    }
                                    currentRow += headerTexts.Count;
                                }

                                int tableBodyStartRow = currentRow;

                                // --- 优化 3: 纯数据填充循环 ---
                                // 这个循环现在变得非常“纯粹”，只负责填入数据，速度极快
                                foreach (var cellInfo in cells)
                                {
                                    worksheet.Cell(tableBodyStartRow + cellInfo.Row, 1 + cellInfo.Col).Value = cellInfo.Text;
                                }

                                // --- 优化 4: 批量合并单元格 ---
                                // 在所有数据都填充完毕后，再一次性处理所有需要合并的单元格
                                foreach (var cellInfo in cells.Where(c => c.RowSpan > 1 || c.ColSpan > 1))
                                {
                                    worksheet.Range(
                                        tableBodyStartRow + cellInfo.Row,
                                        1 + cellInfo.Col,
                                        tableBodyStartRow + cellInfo.Row + cellInfo.RowSpan - 1,
                                        1 + cellInfo.Col + cellInfo.ColSpan - 1
                                    ).Merge();
                                }

                                int bodyRowCount = cells.Any() ? cells.Max(c => c.Row + c.RowSpan) : 0;
                                currentRow += bodyRowCount;

                                // --- 优化 5: 批量处理数据区域的对齐 ---
                                // 对所有数据单元格进行一次性的智能对齐
                                var dataRange = worksheet.Range(tableBodyStartRow, 1, currentRow - 1, columnCount);
                                foreach (var cell in dataRange.Cells())
                                {
                                    // 优先判断单元格的底层类型是否为数字，这是最高效的方式
                                    if (cell.Value.IsNumber)
                                    {
                                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                        continue; // 处理完毕，进入下一次循环
                                    }

                                    // 如果不是数字类型，则按字符串处理
                                    string textValue = cell.GetValue<string>().Trim();
                                    if (string.IsNullOrEmpty(textValue))
                                    {
                                        continue; // 空单元格不处理
                                    }

                                    // --- 核心逻辑：先净化字符串，再进行判断 ---

                                    // 1. 移除千位分隔符逗号
                                    string cleanText = textValue.Replace(",", "");

                                    // 2. 判断净化后的字符串是否为百分比
                                    if (cleanText.EndsWith("%") && decimal.TryParse(cleanText.Substring(0, cleanText.Length - 1), out _))
                                    {
                                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                    }
                                    // 3. 判断净化后的字符串是否为普通数字（可以带负号和小数点）
                                    else if (decimal.TryParse(cleanText, out _))
                                    {
                                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                    }
                                    // 4. 如果以上都不是，则视为普通文本
                                    else
                                    {
                                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                    }
                                }

                                // --- 优化 6: 高效写入表尾 ---
                                if (footerTexts != null && footerTexts.Any())
                                {
                                    int footerCellCount = footerTexts.Count;
                                    if (footerCellCount > 0)
                                    {
                                        // 计算每部分大致应占的列数
                                        int colsPerFooter = columnCount / footerCellCount;
                                        int extraCols = columnCount % footerCellCount;
                                        int currentColumn = 1;

                                        for (int i = 0; i < footerCellCount; i++)
                                        {
                                            var cell = worksheet.Cell(currentRow, currentColumn);
                                            cell.Value = footerTexts[i];
                                            // cell.Style.Font.Italic = true;
                                            // ↓↓↓↓↓↓ 可选：为表尾也应用智能水平对齐 ↓↓↓↓↓↓
                                            // if (decimal.TryParse(footerTexts[i], out _))
                                            // {
                                            //     // 如果是纯数字，则靠右对齐
                                            //     cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                            // }
                                            // else
                                            // {
                                            //     // 否则，作为文本，靠左对齐
                                            //     cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                            // }
                                            // ↑↑↑↑↑↑ 新增结束 ↑↑↑↑↑↑

                                            // 计算当前单元格要合并的列数
                                            int currentColspan = colsPerFooter + (i < extraCols ? 1 : 0);

                                            // 确保最后一个单元格能填满剩余所有空间
                                            if (i == footerCellCount - 1)
                                            {
                                                currentColspan = columnCount - currentColumn + 1;
                                            }

                                            if (currentColspan > 1)
                                            {
                                                var rangeToMerge = worksheet.Range(currentRow, currentColumn, currentRow, currentColumn + currentColspan - 1);
                                                rangeToMerge.Merge();
                                                // 对整个合并后的范围设置样式
                                                rangeToMerge.Style.Font.Italic = true;
                                            }
                                            else
                                            {
                                                cell.Style.Font.Italic = true;
                                            }
                                            currentColumn += currentColspan;
                                        }
                                        currentRow++;
                                    }
                                }

                                // --- 优化 7: 统一添加边框 ---
                                // 所有内容都完成后，对整个有效区域一次性添加边框
                                var fullRange = worksheet.Range(1, 1, currentRow - 1, columnCount);
                                fullRange.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                                fullRange.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                                // --- 优化 8: 放弃 AdjustToContents，采用固定宽度（速度最快） ---
                                // 这是为了极致速度。如果希望列宽自适应，请看下面的“备选方案”，或者放弃优化8，依旧使用AdjustToContents
                                //worksheet.Columns().Width = 20; // 为所有列设置一个合理的固定宽度
                                worksheet.Columns().AdjustToContents(1, 20); // 自动调整，允许的最小宽度为1.0，最大宽度为20.0（字符宽度单位）

                                //// --- 优化 8 (备选): 估算列宽，代替 AdjustToContents ---
                                //// 创建一个字典来存储每列的最大字符数
                                //var maxChars = new Dictionary<int, int>();
                                //var fullRange = worksheet.Range(1, 1, currentRow - 1, columnCount);

                                //foreach (var cell in fullRange.Cells())
                                //{
                                //    // 跳过被合并的单元格，只计算每个合并区域左上角第一个单元格
                                //    if (cell.IsMerged() && cell.Address != cell.MergedRange().FirstCell().Address) continue;

                                //    int length = cell.GetValue<string>().Length;
                                //    int col = cell.Address.ColumnNumber;

                                //    // 考虑合并单元格的宽度分配
                                //    int colSpan = cell.IsMerged() ? cell.MergedRange().ColumnCount() : 1;
                                //    int avgLength = length / colSpan; // 将内容长度均分给被合并的列

                                //    for (int i = 0; i < colSpan; i++)
                                //    {
                                //        int currentCol = col + i;
                                //        if (!maxChars.ContainsKey(currentCol) || avgLength > maxChars[currentCol])
                                //        {
                                //            maxChars[currentCol] = avgLength;
                                //        }
                                //    }
                                //}

                                //// 根据最大字符数设置列宽 (这里的 1.2 是一个经验系数，您可以微调)
                                //foreach (var pair in maxChars)
                                //{
                                //    // +2 是为了留出一些边距
                                //    worksheet.Column(pair.Key).Width = Math.Min(pair.Value * 1.2 + 2, 100); // 设置一个最大宽度上限，防止过宽
                                //}

                                workbook.SaveAs(sfd.FileName);
                            }
                        };

                        // 后续的加载窗体逻辑保持不变
                        Action<Exception> onError = (ex) =>
                        {
                            MessageBox.Show($"导出 Excel 文件时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        };
                        var loadingForm = new FmLoadingExport(work, onError);
                        var result = loadingForm.ShowDialog(owner);

                        if (result == DialogResult.OK)
                        {
                            // 1.准备好带有清晰说明的消息
                            string successMessage = $"表格已成功导出到:\n{sfd.FileName}\n\n点击“确定”将打开文件所在文件夹。";

                            // 2. 显示带有“确定”和“取消”按钮的消息框
                            DialogResult msgBoxResult = MessageBox.Show(owner, successMessage, "导出成功",
                                                                        MessageBoxButtons.OKCancel, // <-- 使用 OKCancel
                                                                        MessageBoxIcon.Information);//想要去掉声音，改成MessageBoxIcon.None，不过这样一来图标也没了

                            // 3. 只在用户点击“确定”时，才执行打开文件夹的操作
                            if (msgBoxResult == DialogResult.OK)
                            {
                                try
                                {
                                    System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{sfd.FileName}\"");
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(owner, $"无法打开文件夹: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            // 如果用户点击“取消”或关闭按钮，则什么也不做，弹窗直接消失。
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show($"导出异常:{ex.Message}");
                    }
                    finally
                    {
                        //治标方案,重新初始化，重置网络
                        // NetworkInitializer.Reinitialize();
                    }
                }
            }
        }
        //原始实现，用于参考，此方法不支持跨行跨列的合并单元格
        [Obsolete("此方法已被新版本替代，不支持合并单元格和加载提示等。")]
        public static void ExportToExcel(DataTable tableData, List<string> headerTexts, List<string> footerTexts)
        {
            if (tableData == null || tableData.Rows.Count == 0)
            {
                MessageBox.Show("没有可供导出的表格数据。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel 文件|*.xlsx";
                sfd.ValidateNames = true;
                sfd.FileName = $"TrOCR_表格_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("识别结果");
                            int currentRow = 1;
                            int columnCount = tableData.Columns.Count > 0 ? tableData.Columns.Count : 1;

                            // --- 新增：写入表头 ---
                            if (headerTexts != null && headerTexts.Count > 0)
                            {
                                foreach (var header in headerTexts)
                                {
                                    var cell = worksheet.Cell(currentRow, 1);
                                    cell.Value = header;
                                    // 合并单元格，让表头居中显示
                                    worksheet.Range(currentRow, 1, currentRow, columnCount).Merge();
                                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    cell.Style.Font.Bold = true;
                                    currentRow++;
                                }
                                // 在表头和表格主体之间留一个空行
                                // currentRow++;
                            }

                            // --- 写入表格主体数据 ---
                            if (tableData.Rows.Count > 0)
                            {
                                worksheet.Cell(currentRow, 1).InsertTable(tableData);
                                currentRow += tableData.Rows.Count + 1; // +1 是因为InsertTable会自带一个表头行
                            }

                            // --- 方案零：写入表尾 ---
                            // if (footerTexts != null && footerTexts.Count > 0)
                            // {
                            //     // 在表格主体和表尾之间留一个空行
                            //     // currentRow++;
                            //     foreach (var footer in footerTexts)
                            //     {
                            //         var cell = worksheet.Cell(currentRow, 1);
                            //         cell.Value = footer;
                            //         worksheet.Range(currentRow, 1, currentRow, columnCount).Merge();
                            //         cell.Style.Font.Italic = true;
                            //         currentRow++;
                            //     }
                            // }
                            // 方案一：---  优化后的写入表尾逻辑  ---
                            if (footerTexts != null && footerTexts.Any())
                            {
                                // currentRow++; // 在表格和表尾之间留一个空行

                                int footerCellCount = footerTexts.Count;
                                if (footerCellCount > 0)
                                {
                                    // 计算每部分大致应占的列数
                                    int colsPerFooter = columnCount / footerCellCount;
                                    int extraCols = columnCount % footerCellCount;
                                    int currentColumn = 1;

                                    for (int i = 0; i < footerCellCount; i++)
                                    {
                                        var cell = worksheet.Cell(currentRow, currentColumn);
                                        cell.Value = footerTexts[i];
                                        cell.Style.Font.Italic = true;

                                        // 计算当前单元格要合并的列数
                                        int currentColspan = colsPerFooter + (i < extraCols ? 1 : 0);

                                        // 确保最后一个单元格能填满剩余所有空间
                                        if (i == footerCellCount - 1)
                                        {
                                            currentColspan = columnCount - currentColumn + 1;
                                        }

                                        if (currentColspan > 1)
                                        {
                                            worksheet.Range(currentRow, currentColumn, currentRow, currentColumn + currentColspan - 1).Merge();
                                        }

                                        currentColumn += currentColspan;
                                    }
                                    // 因为只占用一行，所以最后 currentRow++ 一次即可
                                    currentRow++;
                                }
                            }

                            //方案二： --- ↓↓↓↓↓↓ 修改表尾写入逻辑 ↓↓↓↓↓↓ ---
                            // if (footerTexts != null && footerTexts.Count > 0)
                            // {
                            //     // 在表格主体和表尾之间留一个空行
                            //     currentRow++; 

                            //     // 在同一行内，从左到右依次写入所有表尾文本
                            //     for (int i = 0; i < footerTexts.Count; i++)
                            //     {
                            //         // i是从0开始的，所以列号是 i + 1
                            //         var cell = worksheet.Cell(currentRow, i + 1); 
                            //         cell.Value = footerTexts[i];
                            //         cell.Style.Font.Italic = true;
                            //     }
                            // }
                            // --- ↑↑↑↑↑↑ 修改结束 ↑↑↑↑↑↑ ---

                            // 自动调整列宽
                            worksheet.Columns().AdjustToContents();

                            workbook.SaveAs(sfd.FileName);
                            MessageBox.Show($"表格已成功导出到:\n{sfd.FileName}", "导出成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"导出 Excel 文件时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}