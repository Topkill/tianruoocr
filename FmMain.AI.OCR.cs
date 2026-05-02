using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using TrOCR.Helper;
using TrOCR.Helper.Models;

namespace TrOCR
{
    public partial class FmMain
    {
        // === 全局状态变量 ===

        // 当前选中的厂商 (例如: DeepSeek)
        private CustomAIProvider _currentCustomProvider = null;

        // 当前选中的模式 (例如: 精确识别，包含 Prompt/Temperature 等)
        private AIMode _currentCustomMode = null;

        /// <summary>
        /// 【重构版】加载所有自定义 AI 接口到菜单，并自动恢复状态
        /// </summary>
        public void LoadCustomOpenAIMenus()
        {
            try
            {
                // ================= Step 1: 基础清理与准备 =================
                ToolStripMenuItem parentMenu = this.ai_menu;
                if (parentMenu == null) return;

                // 清理旧的动态菜单
                for (int i = parentMenu.DropDownItems.Count - 1; i >= 0; i--)
                {
                    if (parentMenu.DropDownItems[i].Tag?.ToString() == "DynamicProvider")
                    {
                        parentMenu.DropDownItems.RemoveAt(i);
                    }
                }

                // 读取厂商列表(CustomOpenAIProviders.json)
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "CustomOpenAIProviders.json");
                // 情况 A: 文件不存在
                if (!File.Exists(jsonPath))
                {
                    // 确保“报错事件”是绑定状态
                    // (先减一次防止重复绑定，再加一次)
                    parentMenu.MouseDown -= ShowConfigWarning_MouseDown;
                    parentMenu.MouseDown += ShowConfigWarning_MouseDown;
                    return;
                }

                var providers = JsonConvert.DeserializeObject<List<CustomAIProvider>>(File.ReadAllText(jsonPath));
                // 情况 B: 文件存在但内容为空
                if (providers == null || providers.Count == 0)
                {
                    parentMenu.MouseDown -= ShowConfigWarning_MouseDown;
                    parentMenu.MouseDown += ShowConfigWarning_MouseDown;
                    return;
                }
                // 情况 C: 成功加载了数据
                parentMenu.MouseDown -= ShowConfigWarning_MouseDown;

                // 添加分割线
                parentMenu.DropDownItems.Add(new ToolStripSeparator { Tag = "DynamicProvider" });

                // 读取 INI 记录 (上次选了谁)
                string lastProviderName = IniHelper.GetValue("OpenAICompatible", "LastProvider");
                string lastModeName = IniHelper.GetValue("OpenAICompatible", "LastMode");

                // 标记：是否已经成功恢复了某个选项（防止多个厂商都去恢复）
                bool isRestored = false;

                // ================= Step 2: 循环构建菜单 =================
                foreach (var provider in providers)
                {
                    // 2.1 创建厂商菜单项 (一级)
                    ToolStripMenuItem providerItem = new ToolStripMenuItem(provider.Name);
                    providerItem.Tag = "DynamicProvider";
                    parentMenu.DropDownItems.Add(providerItem);

                    List<AIMode> availableModes = new List<AIMode>();

                    // 2.2 尝试加载子菜单配置 (二级)
                    if (!string.IsNullOrEmpty(provider.ModelConfigPath))
                    {
                        // 处理相对路径/绝对路径
                        string configFullPath = provider.ModelConfigPath;
                        if (!Path.IsPathRooted(configFullPath))
                            configFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFullPath);

                        if (File.Exists(configFullPath))
                        {
                            try
                            {
                                var configObj = JsonConvert.DeserializeObject<AIConfig>(File.ReadAllText(configFullPath, Encoding.UTF8));
                                if (configObj != null && configObj.modes != null)
                                {
                                    availableModes.AddRange(configObj.modes);
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("配置文件存在，但读取出错：" + ex.Message);
                            }
                        }
                    }

                    // 2.3 如果没有加载到任何模式，添加一个“默认模式”保底
                    if (availableModes.Count == 0)
                    {
                        availableModes.Add(new AIMode
                        {
                            mode = "默认模式_内置",
                            system_prompt = "You are a professional OCR engine. Recognize the text in the image and output it directly. " +
                            "Do not use markdown code blocks. Do not output any explanations. Maintain the original line breaks and indentation. " +
                            "If the image contains code, remember to preserve the formatting.",
                            prompt = "Please identify the text in the picture, only the final result and without any explanations.",
                            temperature = 0.5,
                            PromptOrder = new List<string> { "system_prompt", "assistant_prompt", "prompt" }
                        });
                    }

                    // 2.4 将模式列表渲染到 UI，并检查是否匹配上次记录
                    //成功匹配
                    bool foundMatchInThisProvider = false; // 标记：在这个厂商里是否找到了上次的模式

                    foreach (var mode in availableModes)
                    {
                        ToolStripMenuItem modeItem = new ToolStripMenuItem(mode.mode);
                        modeItem.ToolTipText = mode.description;
                        // 注意：点击事件里不传参数，默认 isStartupRestore=false，代表用户点击
                        modeItem.Click += (s, e) => SwitchToCustomAI(provider, mode);

                        providerItem.DropDownItems.Add(modeItem);

                        // --- 判断逻辑：尝试恢复 ---
                        if (!isRestored && provider.Name == lastProviderName && mode.mode == lastModeName)
                        {
                            //  修复点1：传入 true，表示这是启动时的静默恢复，不要强制修改全局 INI 
                            SwitchToCustomAI(provider, mode, true);
                            modeItem.Checked = true;
                            isRestored = true;
                            foundMatchInThisProvider = true;
                        }
                    }

                    // ================= Step 3: 单个厂商内的兜底逻辑 =================
                    if (!isRestored && provider.Name == lastProviderName)
                    {
                        // 情况A: 我没找到具体的模式 (foundMatchInThisProvider == false)
                        // 原因可能是：配置文件改了(模式名变了)，或者配置删了(退化成默认模式)
                        // 解决：强制选中列表里的第一个模式
                        if (!foundMatchInThisProvider && availableModes.Count > 0)
                        {
                            var fallbackMode = availableModes[0];
                            //  修复点2：传入 true，静默恢复 
                            SwitchToCustomAI(provider, fallbackMode, true);

                            // UI打钩
                            if (providerItem.DropDownItems.Count > 0 && providerItem.DropDownItems[0] is ToolStripMenuItem firstItem)
                                firstItem.Checked = true;

                            isRestored = true;
                        }
                    }
                }

                // ================= Step 4: 全局终极兜底逻辑 =================
                // 只有当当前程序确实被配置为 CustomOpenAI，但上面没找到对应配置时，才模拟点击
                if (!isRestored && this.interface_flag == "CustomOpenAI")
                {
                    // 找到第一个厂商菜单项 (跳过分割线)
                    foreach (ToolStripItem item in parentMenu.DropDownItems)
                    {
                        if (item is ToolStripMenuItem firstProviderItem && firstProviderItem.HasDropDownItems)
                        {
                            // 模拟点击第一个子项
                            if (firstProviderItem.DropDownItems[0] is ToolStripMenuItem firstOption)
                            {
                                firstOption.PerformClick(); // 这里触发点击事件，视为用户操作，会强制刷新
                                isRestored = true;
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("加载自定义 AI 菜单失败: " + ex.Message);
            }
        }

        /// <summary>
        /// 切换当前使用的 AI 上下文
        /// </summary>
        /// <param name="provider">厂商</param>
        /// <param name="mode">模式</param>
        /// <param name="isStartupRestore">是否为启动恢复模式？如果是 true，则不写入INI，也不触发全局切换</param>
        private void SwitchToCustomAI(CustomAIProvider provider, AIMode mode, bool isStartupRestore = false)
        {
            try
            {
                // 1. 更新内部变量 (必须执行，否则 OCR 无法工作)
                this._currentCustomProvider = provider;
                this._currentCustomMode = mode;

                // 2.  逻辑分流 
                if (!isStartupRestore)
                {
                    // --- A. 用户主动点击 (User Action) ---
                    // 调用标准切换流程，这会改变全局 engine，刷新界面，并触发重绘
                    OCR_foreach("CustomOpenAI");

                    // 保存到 INI，因为是用户自己点的
                    try
                    {
                        IniHelper.SetValue("OpenAICompatible", "LastProvider", provider.Name);
                        IniHelper.SetValue("OpenAICompatible", "LastMode", mode.mode);
                       //虽然 OCR_foreach 会做，但这里双重保险
                        IniHelper.SetValue("配置", "接口", "CustomOpenAI");
                    }
                    catch { /* 忽略保存错误 */ }

                    // 更新菜单标题
                    this.ai_menu.Text = $"AI√: {provider.Name} - {mode.mode}";
                }
                else
                {
                    // --- B. 启动静默恢复 (Startup Restore) ---
                    // 我们只更新内部变量，不强制切换全局接口 (OCR_foreach)，也不写配置。
                    // 这样如果用户 INI 里是 "百度"，就不会被覆盖成 "CustomOpenAI"。

                    // 唯一需要做的是：如果当前全局接口 *恰好* 已经是 CustomOpenAI，
                    // 我们需要顺手把菜单标题更新了，否则菜单会显示默认值
                    string currentInterface = IniHelper.GetValue("配置", "接口");
                    if (currentInterface == "CustomOpenAI")
                    {
                        this.ai_menu.Text = $"AI√: {provider.Name} - {mode.mode}";
                    }
                }

                // ================== 3. 菜单勾选状态遍历 (视觉一致性) ==================
                // 无论是否是启动恢复，都要把菜单里的勾勾打对
                foreach (ToolStripItem item in this.ai_menu.DropDownItems)
                {
                    if (item is ToolStripSeparator)
                        continue;
                    // 跳过分割线，只处理菜单项
                    if (item is ToolStripMenuItem providerItem)
                    {
                        bool isTargetProvider = (providerItem.Text == provider.Name);
                        providerItem.Checked = isTargetProvider;

                        if (providerItem.HasDropDownItems)
                        {
                            foreach (ToolStripItem subItem in providerItem.DropDownItems)
                            {
                                if (subItem is ToolStripMenuItem modeItem)
                                {
                                    if (isTargetProvider)
                                    {
                                        // 关键逻辑：只有在厂商匹配的情况下，才去比对模式名称
                                        // 这样可以避免不同厂商有同名模式(如"默认模式")导致的误勾选
                                        bool isTargetMode = (modeItem.Text == mode.mode);
                                        modeItem.Checked = isTargetMode;
                                    }
                                    else
                                    {
                                        modeItem.Checked = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"切换接口时发生错误：\n{ex.Message}\n\n堆栈信息：\n{ex.StackTrace}", "错误提示");
            }
        }

        /// <summary>
        /// 自定义 AI 接口的执行入口
        /// </summary>
        public void OCR_OpenAICompatible()
        {
            // 1. 防御性检查
            if (_currentCustomProvider == null)
            {
                typeset_txt = "错误：未选择有效的接口配置。请在菜单中重新选择。";
                split_txt = typeset_txt;
                return;
            }

            try
            {
                // 2. 准备参数
                string apiurl = _currentCustomProvider.ApiUrl.TrimEnd('/');
                if (!apiurl.EndsWith("/chat/completions")) apiurl += "/chat/completions";

                // ==================== 【新增代码：定义流式回调】 ====================
                Action<string> streamCallback = null;
                bool isFirstToken = true; // 标记是否是第一个字

                // 只有当开启流式输出时，才实例化回调
                if (_currentCustomMode != null && _currentCustomMode.stream==true)
                {
                    streamCallback = (token) =>
                    {
                        // 【关键防御】检查窗口是否已销毁
                        if (this.IsDisposed || !this.IsHandleCreated) return;
                        try
                        {
                            // 必须使用 Invoke 回到 UI 线程操作控件
                            this.Invoke((MethodInvoker)delegate
                            {
                                // 二次检查（防止排队期间窗口被关）
                                if (this.IsDisposed || this.RichBoxBody.IsDisposed) return;
                                // ==================== 【修复开始】 ====================
                                // 在处理 isFirstToken 逻辑之前，先处理 token 内容
                                if (isFirstToken)
                                {
                                    // 1. 如果收到的第一个包纯粹是换行或空格，直接丢弃，继续等待下一个包
                                    if (string.IsNullOrWhiteSpace(token)) return;

                                    // 2. 如果包含内容但开头有换行（例如 "\n你好"），去掉开头的空白
                                    token = token.TrimStart();

                                    // 3. 再次检查 Trim 后是否为空，防止 token 只是空格的情况
                                    if (string.IsNullOrEmpty(token)) return;
                                }
                                // ==================== 【修复结束】 ====================
                                // ---如果是第一个字，执行“早产”逻辑---
                                if (isFirstToken)
                                {
                                    // 1. 关闭加载窗口 (模拟 Main_OCR_Thread_last 的部分逻辑)
                                    if (fmloading != null) fmloading.FmlClose = "窗体已关闭";

                                    // 2. 强制显示主窗口
                                    this.Visible = true;
                                    this.Show();
                                    this.WindowState = FormWindowState.Normal;
                                    this.TopMost = true; // 暂时置顶确保可见
                                    if (IniHelper.GetValue("工具栏", "顶置") == "False") this.TopMost = false;

                                    // 3. 准备界面布局 (调用 TransClick 确保双栏布局正确)
                                    // 注意：这里不需要清空文本，因为我们要追加，但需要确保文本框可见
                                    this.RichBoxBody.Visible = true;
                                    this.RichBoxBody.Text = ""; // 清空之前的占位符
                                     // 【新增】禁用 OCR 结果框的工具栏
                                    this.RichBoxBody.SetToolbarEnabled(false);
                                    // 【新增】锁定文本框，禁止用户在生成期间乱按
                                    this.RichBoxBody.richTextBox1.ReadOnly = true;

                                    // 标记流式开始，防止 TextChanged 触发定时器
                                    this.isStreaming = true;
                                    // 【新增】暂时移除 TextChanged 事件，防止流式输出时频繁触发
                                    this.RichBoxBody.richTextBox1.TextChanged -= RichBoxBody_TextChanged;
                                    isFirstToken = false;
                                    
                                }

                                // ---追加文本---
                                this.RichBoxBody.richTextBox1.AppendText(token);

                                // 滚动到最后
                                this.RichBoxBody.richTextBox1.SelectionStart = this.RichBoxBody.Text.Length;
                                this.RichBoxBody.richTextBox1.ScrollToCaret();
                            });
                        }
                        catch (ObjectDisposedException) { /* 忽略，线程安全退出 */ }
                        catch (InvalidOperationException) { /* 忽略 */ }
                    };
                }

                // 3.  直接调用接口 
                string result = OpenAICompatibleHelper.OCR(
                    image_screen,
                    apiurl,
                    _currentCustomProvider.ApiKey,
                    _currentCustomProvider.ModelName,
                   _currentCustomMode,
                   streamCallback // <--- 传入回调
                );

                // 4. 处理结果
                if (string.IsNullOrEmpty(result))
                {
                    typeset_txt = "接口返回为空，请检查网络或 Key 是否正确。";
                }
                else
                {
                    typeset_txt = result;
                }
                split_txt = typeset_txt;
            }
            catch (Exception ex)
            {
                typeset_txt = $"接口调用出错: {ex.Message}";
                split_txt = typeset_txt;
            }
        }
        /*
         * 补充：发现一个没啥影响的小bug（ai翻译应该也有）：
         * 流式输出有时候直接一瞬间出现结果，没有打字机过程，原因是ui线程：
         * WinForms 的 UI 刷新机制是基于消息队列的。当你通过 Invoke 疯狂地向 UI 线程塞入 AppendText 指令时（比如 1 秒钟塞了 50 次），UI 线程会优先执行逻辑代码（修改 Text 属性），而把“重绘界面”（Paint）的任务往后排。结果就是：逻辑上文本已经一点点加上去了，但视觉上 UI 线程决定“攒一波大的”再绘制，导致你看到的是瞬间出来的结果。
         * 这个bug不用修，如果想修：
         * 可以使用一种节流刷新方案，对性能影响小，又不会导致每个打字都等1段时间，导致从打字到显示完结果的总时长变长变慢：
         * // 在类成员变量里加一个记录时间的变量
            private long lastUpdateTime = 0;

            // ... 在 streamCallback 内部 ...

            this.Invoke((MethodInvoker)delegate
            {
                // ... 前面的逻辑 ...

                // ---追加文本---
                this.RichBoxBody.richTextBox1.AppendText(token);

                // 滚动到最后
                this.RichBoxBody.richTextBox1.SelectionStart = this.RichBoxBody.Text.Length;
                this.RichBoxBody.richTextBox1.ScrollToCaret();

                // ==================== 【性能优化的流式刷新】 ====================
                long now = DateTime.Now.Ticks / 10000; // 转换为毫秒
    
                // 如果距离上次刷新超过了 100ms，或者是第一个字，则强制刷新
                if (now - lastUpdateTime > 100 || isFirstToken) // 注意：这里isFirstToken逻辑可能要在前面处理完后更新
                {
                    this.RichBoxBody.richTextBox1.Update();
                    lastUpdateTime = now;
                }
                // 否则，就交给 Windows 自己决定什么时候画（通常会攒一波一起画）
                // ==========================================================
            });
         */
        //下面是一种未来可选的优化方式，使用总路由方法，根据不同的 Type 调用不同的 接口实现，需要配合：
        /*类要实现Type字段
         * if (interface_flag == "CustomOpenAI")
        {
            OCR_Custom_Router();
            fmloading.FmlClose = "窗体已关闭";
            Invoke(new OcrThread(Main_OCR_Thread_last));
            return;
        }
        */
        /// <summary>
        /// 自定义接口的统一执行入口 (支持 OpenAI/Anthropic 等多种协议)
        /// </summary>
        //public void OCR_Custom_Router()
        //{
        //    // 防御性检查
        //    if (_currentCustomProvider == null)
        //    {
        //        typeset_txt = "错误：未选择有效的接口配置。";
        //        split_txt = typeset_txt;
        //        return;
        //    }

        //    try
        //    {
        //        string result = "";

        //        //  核心路由：根据配置的 Type 字段决定调用哪个 Helper 
        //        // 假设您的 CustomAIProvider 类里已经加了 Type 字段
        //        //string type = _currentCustomProvider.Type ?? "OpenAI"; // 默认为 OpenAI
        //        string type =  "OpenAI"; // 默认为 OpenAI

        //        switch (type)
        //        {
        //            //case "Anthropic":
        //                // === Anthropic兼容模式 ===
        //                // 约定：ApiUrl 存 AK, ApiKey 存 SK
        //                //result = ClaudeHelper.GeneralBasic(
        //                //    image_screen, // 截图
        //                //    _currentCustomProvider.ApiUrl, // API Key
        //                //    _currentCustomProvider.ApiKey  // Secret Key
        //                //
        //                //);
        //                //break;

        //            case "OpenAI":
        //            default:
        //                // === OpenAI 兼容模式 (DeepSeek, Kimi, etc.) ===
        //                string sysPrompt = _currentCustomMode?.system_prompt ?? "";
        //                string userPrompt = _currentCustomMode?.prompt ?? "请识别图片中的文字";

        //                // 处理高级参数 (Temperature 等)
        //                double? temp = _currentCustomMode?.temperature;
        //                bool? thinking = _currentCustomMode?.enable_thinking;

        //                //result = OpenAICompatibleHelper.OCR_V3(
        //                //    image_screen,
        //                //    _currentCustomProvider.ApiUrl,
        //                //    _currentCustomProvider.ApiKey,
        //                //    _currentCustomProvider.ModelName,
        //                //    sysPrompt,
        //                //    userPrompt,
        //                //    _currentCustomMode?.assistant_prompt,
        //                //    temp,
        //                //    thinking
        //                //);
        //                break;
        //        }

        //        // 统一处理结果
        //        if (string.IsNullOrEmpty(result))
        //        {
        //            typeset_txt = "接口返回为空。";
        //        }
        //        else
        //        {
        //            typeset_txt = result;
        //        }
        //        split_txt = typeset_txt;
        //    }
        //    catch (Exception ex)
        //    {
        //        typeset_txt = $"接口调用出错: {ex.Message}";
        //        split_txt = typeset_txt;
        //    }
        //}
    }
}