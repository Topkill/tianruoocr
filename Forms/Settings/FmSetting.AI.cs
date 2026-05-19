using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrOCR.Helper;
using TrOCR.Helper.Models;

namespace TrOCR
{
    public sealed partial class FmSetting
    {
        private void LoadCustomAIProviders()
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "CustomOpenAIProviders.json");
            List<CustomAIProvider> list = null;

            // 1. 读取 JSON
            if (File.Exists(jsonPath))
            {
                try
                {
                    string json = File.ReadAllText(jsonPath);
                    list = JsonConvert.DeserializeObject<List<CustomAIProvider>>(json);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("读取自定义接口配置失败: " + ex.Message);
                }
            }

            // 如果为空，创建一个空列表
            if (list == null) list = new List<CustomAIProvider>();

            // 2. 转换为 BindingList 并绑定到 ListBox
            _customProviders = new BindingList<CustomAIProvider>(list);

            lb_CustomProviders.DataSource = _customProviders;
            lb_CustomProviders.DisplayMember = "Name"; // ListBox 显示 "Name" 属性
            lb_CustomProviders.ValueMember = "Id";

            // 3. 绑定选中事件
            lb_CustomProviders.SelectedIndexChanged += Lb_CustomProviders_SelectedIndexChanged;

            // 4. 触发一次选中逻辑以初始化界面状态
            Lb_CustomProviders_SelectedIndexChanged(null, null);
        }
        private void Lb_CustomProviders_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 获取当前选中的项
            if (lb_CustomProviders.SelectedItem is CustomAIProvider item)
            {
                _currentEditingProvider = item;
                _isUserAction = false; //  暂停事件触发，防止 TextChanged 误伤

                // 填充右侧 TextBoxes
                txt_Name.Text = item.Name;
                txt_ApiUrl.Text = item.ApiUrl;
                txt_ApiKey.Text = item.ApiKey;
                txt_ModelName.Text = item.ModelName;
                txt_ConfigPath.Text = item.ModelConfigPath;

                // 启用右侧控件
                ToggleDetailPanel(true);

                _isUserAction = true; //  恢复事件
            }
            else
            {
                _currentEditingProvider = null;
                _isUserAction = false;

                // 清空右侧
                txt_Name.Clear();
                txt_ApiUrl.Clear();
                txt_ApiKey.Clear();
                txt_ModelName.Clear();
                txt_ConfigPath.Clear();

                // 禁用右侧控件
                ToggleDetailPanel(false);

                _isUserAction = true;
            }
        }

        // 辅助方法：控制右侧是否可编辑
        private void ToggleDetailPanel(bool enable)
        {
            txt_Name.Enabled = enable;
            txt_ApiUrl.Enabled = enable;
            txt_ApiKey.Enabled = enable;
            txt_ModelName.Enabled = enable;
            txt_ConfigPath.Enabled = enable;
            btn_BrowseConfig.Enabled = enable;
        }
        // 1. 名称修改 (特殊处理：需要刷新 ListBox 显示)
        private void txt_Name_TextChanged(object sender, EventArgs e)
        {
            if (!_isUserAction || _currentEditingProvider == null) return;

            _currentEditingProvider.Name = txt_Name.Text;

            // 强制 ListBox 刷新显示的文字
            _customProviders.ResetBindings();
        }

        // 2. 其他字段修改
        private void txt_ApiUrl_TextChanged(object sender, EventArgs e)
        {
			if (!_isUserAction || _currentEditingProvider == null) return;
    		_currentEditingProvider.ApiUrl = txt_ApiUrl.Text;
        }

        private void txt_ApiKey_TextChanged(object sender, EventArgs e)
        {
            if (!_isUserAction || _currentEditingProvider == null) return;
            _currentEditingProvider.ApiKey = txt_ApiKey.Text;
        }

        private void txt_ModelName_TextChanged(object sender, EventArgs e)
        {
            if (!_isUserAction || _currentEditingProvider == null) return;
            _currentEditingProvider.ModelName = txt_ModelName.Text;
        }

        private void txt_ConfigPath_TextChanged(object sender, EventArgs e)
        {
            if (!_isUserAction || _currentEditingProvider == null) return;
            _currentEditingProvider.ModelConfigPath = txt_ConfigPath.Text;
        }
        // 添加按钮
        private void btn_Add_Provider_Click(object sender, EventArgs e)
        {
            var newItem = new CustomAIProvider
            {
                Name = "OpenAI兼容 " + (_customProviders.Count + 1),
                ApiUrl = "https://api.openai.com/v1",
                ModelName = "gpt-4o"
            };

            _customProviders.Add(newItem);

            // 自动选中新加的这一项
            lb_CustomProviders.SelectedItem = newItem;
        }

        // 删除按钮
        private void btn_Del_Provider_Click(object sender, EventArgs e)
        {
            if (lb_CustomProviders.SelectedItem is CustomAIProvider item)
            {
                if (MessageBox.Show($"确定删除接口“{item.Name}”吗？", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _customProviders.Remove(item);
                }
            }
        }

        // 浏览配置文件按钮
        private void btn_BrowseConfig_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "选择模型配置文件 (JSON)";
            dlg.Filter = "JSON Files|*.json|All Files|*.*";
            dlg.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fullPath = dlg.FileName;
                string appPath = AppDomain.CurrentDomain.BaseDirectory;
				// 确保 appPath 以斜杠结尾 (防御前缀误判)
				string separator = Path.DirectorySeparatorChar.ToString();
				if (!appPath.EndsWith(separator))
				{
					appPath += separator;
				}

                // 如果文件在程序目录下，转为相对路径（更美观，便携）
                if (fullPath.StartsWith(appPath, StringComparison.OrdinalIgnoreCase))
                {
                    txt_ConfigPath.Text = fullPath.Substring(appPath.Length).TrimStart('\\', '/');
                }
                else
                {
                    txt_ConfigPath.Text = fullPath;
                }
            }
        }

        private void LoadCustomAITransProviders()
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "CustomOpenAITransProviders.json");
            List<CustomAITransProvider> list = null;

            // 1. 读取 JSON
            if (File.Exists(jsonPath))
            {
                try
                {
                    string json = File.ReadAllText(jsonPath);
                    list = JsonConvert.DeserializeObject<List<CustomAITransProvider>>(json);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("读取自定义翻译接口配置失败: " + ex.Message);
                }
            }

            // 如果为空，创建一个空列表
            if (list == null) list = new List<CustomAITransProvider>();

            // 2. 转换为 BindingList 并绑定到 ListBox
            _customTransProviders = new BindingList<CustomAITransProvider>(list);

            lb_CustomTransProviders.DataSource = _customTransProviders;
            lb_CustomTransProviders.DisplayMember = "Name"; // ListBox 显示 "Name" 属性
            lb_CustomTransProviders.ValueMember = "Id";

            // 3. 绑定选中事件
            lb_CustomTransProviders.SelectedIndexChanged += lb_CustomTransProviders_SelectedIndexChanged;

            // 4. 触发一次选中逻辑以初始化界面状态
            lb_CustomTransProviders_SelectedIndexChanged(null, null);
        }
        private void lb_CustomTransProviders_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 获取当前选中的项
            if (lb_CustomTransProviders.SelectedItem is CustomAITransProvider item)
            {
                _currentEditingTransProvider = item;
                _isUserActionTrans = false; //  暂停事件触发，防止 TextChanged 误伤

                // 填充右侧 TextBoxes
                txt_Trans_Name.Text = item.Name;
                txt_Trans_ApiUrl.Text = item.ApiUrl;
                txt_Trans_ApiKey.Text = item.ApiKey;
                txt_Trans_ModelName.Text = item.ModelName;
                txt_Trans_ConfigPath.Text = item.ModelConfigPath;
				txt_Trans_Source.Text = item.Source;
				txt_Trans_Target.Text = item.Target;

                // 启用右侧控件
                ToggleTransDetailPanel(true);

                _isUserActionTrans = true; //  恢复事件
            }
            else
            {
                _currentEditingTransProvider = null;
                _isUserActionTrans = false;

                // 清空右侧
                txt_Trans_Name.Clear();
                txt_Trans_ApiUrl.Clear();
                txt_Trans_ApiKey.Clear();
                txt_Trans_ModelName.Clear();
                txt_Trans_ConfigPath.Clear();
				txt_Trans_Source.Clear();
				txt_Trans_Target.Clear();

                // 禁用右侧控件
                ToggleTransDetailPanel(false);

                _isUserActionTrans = true;
            }
        }

        // 辅助方法：控制右侧是否可编辑
        private void ToggleTransDetailPanel(bool enable)
        {
            txt_Trans_Name.Enabled = enable;
            txt_Trans_ApiUrl.Enabled = enable;
            txt_Trans_ApiKey.Enabled = enable;
            txt_Trans_ModelName.Enabled = enable;
            txt_Trans_ConfigPath.Enabled = enable;
            btn_Trans_BrowseConfig.Enabled = enable;
            txt_Trans_Source.Enabled = enable;
            txt_Trans_Target.Enabled = enable;
        }
        // 1. 名称修改 (特殊处理：需要刷新 ListBox 显示)
        private void txt_Trans_Name_TextChanged(object sender, EventArgs e)
        {
            if (!_isUserActionTrans || _currentEditingTransProvider == null) return;

            _currentEditingTransProvider.Name = txt_Trans_Name.Text;

            // 强制 ListBox 刷新显示的文字
            _customTransProviders.ResetBindings();
        }

        // 2. 其他字段修改
        private void txt_Trans_ApiUrl_TextChanged(object sender, EventArgs e)
        {
			if (!_isUserActionTrans || _currentEditingTransProvider == null) return;
            _currentEditingTransProvider.ApiUrl = txt_Trans_ApiUrl.Text;
        }

        private void txt_Trans_ApiKey_TextChanged(object sender, EventArgs e)
        {
            if (!_isUserActionTrans || _currentEditingTransProvider == null) return;
            _currentEditingTransProvider.ApiKey = txt_Trans_ApiKey.Text;
        }

        private void txt_Trans_ModelName_TextChanged(object sender, EventArgs e)
        {
            if (!_isUserActionTrans || _currentEditingTransProvider == null) return;
            _currentEditingTransProvider.ModelName = txt_Trans_ModelName.Text;
        }

        private void txt_Trans_ConfigPath_TextChanged(object sender, EventArgs e)
        {
            if (!_isUserActionTrans || _currentEditingTransProvider == null) return;
            _currentEditingTransProvider.ModelConfigPath = txt_Trans_ConfigPath.Text;
        }
        private void txt_Trans_Source_TextChanged(object sender, EventArgs e)
        {
            if (!_isUserActionTrans || _currentEditingTransProvider == null) return;
            _currentEditingTransProvider.Source = txt_Trans_Source.Text;
        }
        private void txt_Trans_Target_TextChanged(object sender, EventArgs e)
        {
            if (!_isUserActionTrans || _currentEditingTransProvider == null) return;
            _currentEditingTransProvider.Target = txt_Trans_Target.Text;
        }
        // 添加按钮
        private void btn_Trans_Add_Provider_Click(object sender, EventArgs e)
        {
            var newItem = new CustomAITransProvider
            {
                Name = "OpenAI兼容 " + (_customTransProviders.Count + 1),
                ApiUrl = "https://api.openai.com/v1",
                ModelName = "gpt-4o"
            };

            _customTransProviders.Add(newItem);

            // 自动选中新加的这一项
            lb_CustomTransProviders.SelectedItem = newItem;
        }

        // 删除按钮
        private void btn_Trans_Del_Provider_Click(object sender, EventArgs e)
        {
            if (lb_CustomTransProviders.SelectedItem is CustomAITransProvider item)
            {
                if (MessageBox.Show($"确定删除接口“{item.Name}”吗？", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _customTransProviders.Remove(item);
                }
            }
        }

        // 浏览配置文件按钮
        private void btn_Trans_BrowseConfig_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "选择模型配置文件 (JSON)";
            dlg.Filter = "JSON Files|*.json|All Files|*.*";
            dlg.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fullPath = dlg.FileName;
                string appPath = AppDomain.CurrentDomain.BaseDirectory;
				// 确保 appPath 以斜杠结尾 (防御前缀误判)
				string separator = Path.DirectorySeparatorChar.ToString();
				if (!appPath.EndsWith(separator))
				{
					appPath += separator;
				}

                // 如果文件在程序目录下，转为相对路径（更美观，便携）
                if (fullPath.StartsWith(appPath, StringComparison.OrdinalIgnoreCase))
                {
                    txt_Trans_ConfigPath.Text = fullPath.Substring(appPath.Length).TrimStart('\\', '/');
                }
                else
                {
                    txt_Trans_ConfigPath.Text = fullPath;
                }
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            // 禁用按钮，防止重复点击，并提示用户
            button.Enabled = false;
            button.Text = "测试中...";

            try
            {
                string testImagePath = "test.png";

                // 检查测试文件是否存在
                if (!File.Exists(testImagePath))
                {
                    MessageBox.Show($"测试图片 '{testImagePath}' 未找到！\n\n请将一张图片放在程序的运行目录下（和.exe文件一起），并重命名为 test.png。", "测试文件缺失", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string resultText = "";
                // 使用 Task.Run 将耗时的OCR操作放到后台线程，避免UI卡死
                //await Task.Run(() =>
                //{
                //	// 使用 using 语句确保从文件加载的图片资源能被正确释放
                //	using (var localImage = new Bitmap(testImagePath))
                //	{
                //        // 这里我们直接调用我们之前优化好的静态PaddleOCRHelper
                //        // 它内部已经处理了引擎的创建、使用和销毁
                //        // resultText = PaddleOCRHelper.RecognizeText(localImage);

                        
                //    }
                //});
                await Task.Run(async() =>
                {
                    // 使用 using 语句确保从文件加载的图片资源能被正确释放
                    using (var localImage = new Bitmap(testImagePath))
                    {
                        //  使用 await 关键字来异步等待识别结果
                        // 调用 RecognizeTextAsync 方法，它会返回一个 Task<string>
                        // await 会“暂停”这个方法，直到后台线程完成识别并将结果返回，期间UI不会卡死
                        //resultText = await PaddleOCRHelper.Instance.RecognizeTextAsync(localImage);

                    }
                });
                //// 使用 Task.Run 将耗时的OCR操作放到后台线程，避免UI卡死
                //await Task.Run(() =>
                //{
                //	// 使用 using 语句确保从文件加载的图片资源能被正确释放
                //	using (var localImage = new Bitmap(testImagePath))
                //	{
                //		// --- 这里是修改的核心 ---
                //		// 1. 获取搜狗OCR所需的图片（这里我们直接使用原图，不进行缩放）
                //		// 依然建议创建一个克隆来传递，这是一个很好的隔离实践
                //		using (var imageForOcr = new Bitmap(localImage))
                //		{
                //			try
                //			{
                //				// 2. 调用OcrHelper中的搜狗识别方法
                //				// 这个方法在 FmMain.cs 的 SougouOCR 方法中被使用
                //				var jsonResult = OcrHelper.SgBasicOpenOcr(imageForOcr);

                //				// 3. 简单解析返回的JSON结果以获取文本
                //				// 模仿 FmMain.cs 中对搜狗结果的处理
                //				var jObject = Newtonsoft.Json.Linq.JObject.Parse(jsonResult);
                //				var jArray = (Newtonsoft.Json.Linq.JArray)jObject["result"];

                //				var sb = new System.Text.StringBuilder();
                //				foreach (var item in jArray)
                //				{
                //					sb.AppendLine(item["content"].ToString());
                //				}
                //				resultText = sb.ToString().Trim();

                //				if (string.IsNullOrEmpty(resultText))
                //				{
                //					resultText = "***搜狗OCR未识别到文本***";
                //				}
                //			}
                //			catch (Exception ex)
                //			{
                //				resultText = $"搜狗OCR识别出错: {ex.Message}";
                //			}
                //		}
                //	}
                //});
                // // 使用 Task.Run 将耗时的OCR操作放到后台线程，避免UI卡死
                // await Task.Run(() =>
                // {
                //     // 使用 using 语句确保从文件加载的图片资源能被正确释放
                //     using (var localImage = new Bitmap(testImagePath))
                //     {
                //         try
                //         {
                //             // --- 这里是修改的核心 ---
                //             // 1. 调用OcrHelper中的方法，将图片转换为字节数组
                //             byte[] imageBytes = OcrHelper.ImgToBytes(localImage);

                //             // 2. 调用微信OCR的核心识别方法
                //             // OcrHelper.WeChat 是一个异步方法，我们用 .GetAwaiter().GetResult() 在后台线程中同步等待它完成
                //             string result = OcrHelper.WeChat(imageBytes).GetAwaiter().GetResult();

                //             resultText = string.IsNullOrEmpty(result) ? "***微信OCR未识别到文本***" : result;
                //         }
                //         catch (Exception ex)
                //         {
                //             resultText = $"微信OCR识别出错: {ex.Message}";
                //         }
                //     }
                // });
                // 在UI线程上显示结果，确认测试已执行
                MessageBox.Show($"paddle识别完成！\n\n识别结果（前50个字符）:\n{new string(resultText.Take(50).ToArray())}", "测试完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"测试过程中发生错误: {ex.Message}", "测试失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 无论成功与否，最后都恢复按钮状态
                button.Enabled = true;
                button.Text = "button1";
            }
        }
    }
}
