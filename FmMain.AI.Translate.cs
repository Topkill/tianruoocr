using DocumentFormat.OpenXml.Bibliography;
using Newtonsoft.Json;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrOCR.Helper;
using TrOCR.Helper.Models;

namespace TrOCR
{
    public partial class FmMain
    {
        // === 全局状态变量 ===

        // 当前选中的厂商 (例如: DeepSeek)
        private CustomAITransProvider _currentCustomTransProvider = null;

        // 当前选中的模式 (例如: 精确识别，包含 Prompt/Temperature 等)
        private AIMode _currentCustomTransMode = null;

        /// <summary>
        /// 加载 AI 翻译配置文件并初始化动态菜单
        /// </summary>
        public void LoadCustomOpenAITransMenus()
        {
            try
            {
                // ================= Step 1: 基础清理与准备 =================
                ToolStripMenuItem parentMenu = this.ai_menu_trans;
                if (parentMenu == null) return;

                // 清理旧的动态菜单
                for (int i = parentMenu.DropDownItems.Count - 1; i >= 0; i--)
                {
                    if (parentMenu.DropDownItems[i].Tag?.ToString() == "DynamicTransProvider")
                    {
                        parentMenu.DropDownItems.RemoveAt(i);
                    }
                }

                // 读取厂商列表(CustomOpenAITransProviders.json)
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "CustomOpenAITransProviders.json");
                if (!File.Exists(jsonPath))
                {
                    // 没文件 -> 绑定报错
                    parentMenu.MouseDown -= ShowConfigWarning_MouseDown;
                    parentMenu.MouseDown += ShowConfigWarning_MouseDown;
                    return;
                }

                var providers = JsonConvert.DeserializeObject<List<CustomAITransProvider>>(File.ReadAllText(jsonPath));
                if (providers == null || providers.Count == 0)
                {
                    // 空配置 -> 绑定报错
                    parentMenu.MouseDown -= ShowConfigWarning_MouseDown;
                    parentMenu.MouseDown += ShowConfigWarning_MouseDown;
                    return;
                }
                // 有配置 -> 解绑报错，恢复正常
                parentMenu.MouseDown -= ShowConfigWarning_MouseDown;

                // 添加分割线
                parentMenu.DropDownItems.Add(new ToolStripSeparator { Tag = "DynamicTransProvider" });

                // 读取 INI 记录 (上次选了谁)
                string lastProviderName = IniHelper.GetValue("OpenAICompatibleTrans", "LastProvider");
                string lastModeName = IniHelper.GetValue("OpenAICompatibleTrans", "LastMode");

                // 标记：是否已经成功恢复了某个选项
                bool isRestored = false;

                // ================= Step 2: 循环构建菜单 =================
                foreach (var provider in providers)
                {
                    // 2.1 创建厂商菜单项 (一级)
                    ToolStripMenuItem providerItem = new ToolStripMenuItem(provider.Name);
                    providerItem.Tag = "DynamicTransProvider";
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
                                /* 忽略配置读取错误 */
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
                            system_prompt = "You are a professional translator. Translate the user input directly, without any explanations",
                            prompt = "Please translate the following text into ${tolang}, only stating the final result, without any explanations:",
                            temperature = 1.0,
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
                        // 点击事件保持默认 (isStartupRestore = false)
                        modeItem.Click += (s, e) => SwitchToCustomTranAI(provider, mode);

                        providerItem.DropDownItems.Add(modeItem);

                        // --- 判断逻辑：尝试恢复 ---
                        if (!isRestored && provider.Name == lastProviderName && mode.mode == lastModeName)
                        {
                            //  修复点1：传入 true，静默恢复 
                            SwitchToCustomTranAI(provider, mode, true);
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
                            SwitchToCustomTranAI(provider, fallbackMode, true);

                            // UI打钩
                            if (providerItem.DropDownItems.Count > 0 && providerItem.DropDownItems[0] is ToolStripMenuItem firstItem)
                                firstItem.Checked = true;

                            isRestored = true;
                        }
                    }
                }

                // ================= Step 4: 全局终极兜底逻辑 =================
                // 只有当当前确确实实是 CustomOpenAI 且之前没恢复成功时，才模拟点击
                if (!isRestored && StaticValue.Translate_Current_API == "CustomOpenAI")
                {
                    // 找到第一个厂商菜单项 (跳过分割线)
                    foreach (ToolStripItem item in parentMenu.DropDownItems)
                    {
                        if (item is ToolStripMenuItem firstProviderItem && firstProviderItem.HasDropDownItems)
                        {
                            // 模拟点击第一个子项
                            if (firstProviderItem.DropDownItems[0] is ToolStripMenuItem firstOption)
                            {
                                firstOption.PerformClick();
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
        /// <param name="isStartupRestore">是否为启动恢复模式？</param>
        private void SwitchToCustomTranAI(CustomAITransProvider provider, AIMode mode, bool isStartupRestore = false)
        {
            try
            {
                // 1. 更新全局变量 (必须执行)
                this._currentCustomTransProvider = provider;
                this._currentCustomTransMode = mode;

                // 2.  逻辑分流 
                if (!isStartupRestore)
                {
                    // --- A. 用户主动点击 ---
                    // 切换全局接口状态，这会刷新 UI 布局
                    Trans_foreach("CustomOpenAI");

                    // 保存到 INI
                    try
                    {
                        IniHelper.SetValue("OpenAICompatibleTrans", "LastProvider", provider.Name);
                        IniHelper.SetValue("OpenAICompatibleTrans", "LastMode", mode.mode);
                        // 虽然 Trans_foreach 会做，但这里双重保险
                        IniHelper.SetValue("配置", "翻译接口", "CustomOpenAI");
                    }
                    catch { /* 忽略保存错误 */ }

                    // 更新菜单文本
                    this.ai_menu_trans.Text = $"AI√: {provider.Name} - {mode.mode}";
                }
                else
                {
                    // --- B. 启动静默恢复 ---
                    // 不调用 Trans_foreach，不写 INI。
                    // 仅当当前已经选中了 CustomOpenAI 时，才更新菜单上的文字显示
                    string currentApi = IniHelper.GetValue("配置", "翻译接口");
                    if (currentApi == "CustomOpenAI")
                    {
                        this.ai_menu_trans.Text = $"AI√: {provider.Name} - {mode.mode}";
                    }
                }

                // ================== 3. UI 视觉更新 (打钩) ==================
                // 无论何种模式，都要确保菜单的勾选状态正确
                foreach (ToolStripItem item in this.ai_menu_trans.DropDownItems)
                {
                    if (item is ToolStripSeparator)
                        continue;
                    // 跳过分割线，只处理菜单项
                    if (item is ToolStripMenuItem providerItem)
                    {
                        // 判断这是否是当前选中的厂商 (例如 "DeepSeek")
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
        /// OpenAICompatibleTrans Translate 执行入口 (被 Main_OCR_Thread 调用)
        /// </summary>
        public async Task<string> Trans_OpenAICompatible(string text, string fromLang, string toLang)
        {
            try
            {
                // 1. 检查配置
                if (this._currentCustomTransProvider == null)
                {
                    return "错误：未选择有效的翻译接口配置。请在菜单中重新选择";
                }
                string apiurl = _currentCustomTransProvider.ApiUrl.TrimEnd('/');
                if (!apiurl.EndsWith("/chat/completions")) apiurl += "/chat/completions";
                string system_prompt = _currentCustomTransMode.system_prompt;
                if (!string.IsNullOrEmpty(system_prompt))
                {
                    system_prompt = ReplaceLangPlaceholder(_currentCustomTransMode.system_prompt, fromLang, toLang);
                }
                string user_prompt = _currentCustomTransMode.prompt;
                if (!string.IsNullOrEmpty(user_prompt))
                {
                    user_prompt = ReplaceLangPlaceholder(_currentCustomTransMode.prompt, fromLang, toLang);

                }
                string assistant_prompt = _currentCustomTransMode.assistant_prompt;
                if (!string.IsNullOrEmpty(assistant_prompt))
                {
                    assistant_prompt = ReplaceLangPlaceholder(_currentCustomTransMode.assistant_prompt, fromLang, toLang);

                }
                //为什么要new一个副本出来，而不是直接赋值为_currentCustomTransMode，一是因为字段的值需要处理，不完全一致，二是因为_currentCustomTransMode是引用类型，直接赋值是浅拷贝，如果修改赋值后的变量会影响原_currentCustomTransMode
                AIMode aIMode = new AIMode
                {
                    mode = _currentCustomTransMode.mode,
                    description = _currentCustomTransMode.description,
                    system_prompt = system_prompt,
                    prompt = user_prompt,
                    assistant_prompt = assistant_prompt,
                    temperature = _currentCustomTransMode.temperature,
                    enable_thinking = _currentCustomTransMode.enable_thinking,
                    stream = _currentCustomTransMode.stream,
                    PromptOrder = new List<string>(_currentCustomTransMode.PromptOrder)

                };
                Action<string> streamCallback = null;
                bool isFirstToken = true;

                // 只有开启流式才挂载回调
                if (aIMode.stream == true)
                {
                    streamCallback = (token) =>
                    {
                        // 【关键防御】检查窗口是否已销毁
                        if (this.IsDisposed || !this.IsHandleCreated) return;
                        try { 
                            // 必须 Invoke 到 UI 线程
                            this.Invoke((MethodInvoker)delegate
                            {
                                // 二次检查（防止排队期间窗口被关）
                                if (this.IsDisposed || this.RichBoxBody_T.IsDisposed) return;
                                // ==================== 【修复开始】 ====================
                                // 1. 预处理 token：如果是首字，去掉开头的换行和空格
                                if (isFirstToken)
                                {
                                    // 如果整个 token 都是空白（比如 AI 先发了一个 "\n"），直接忽略，不进入初始化逻辑
                                    if (string.IsNullOrWhiteSpace(token)) return;

                                    // 去掉开头的空白
                                    token = token.TrimStart();

                                    // 如果 Trim 后没了，也忽略
                                    if (string.IsNullOrEmpty(token)) return;
                                }
                                // ==================== 【修复结束】 ====================
                                // --- 第一个字到达时的初始化 ---
                                if (isFirstToken)
                                {
                                    // 1. 标记开始流式，防止 translate_child 闪烁
                                    this.isTransStreaming = true;

                                    // 2. 隐藏加载动画 (重要体验优化：字出来了，圈圈就该停了)
                                    this.PictureBox1.Visible = false;
                                    this.PictureBox1.SendToBack();

                                    // 3. 清空之前的旧文本，准备接收新翻译
                                    this.RichBoxBody_T.Text = "";
                                   
                                    // 4. 确保翻译框可见
                                    this.RichBoxBody_T.Visible = true;
                                    // 【新增】禁用翻译框的工具栏
                                    RichBoxBody_T.SetToolbarEnabled(false);
                                    //禁用编辑
                                    this.RichBoxBody_T.richTextBox1.ReadOnly = true;


                                    isFirstToken = false;
                                }

                                // --- 追加文本 ---
                                this.RichBoxBody_T.richTextBox1.AppendText(token);

                                // --- 滚动到底部 ---
                                this.RichBoxBody_T.richTextBox1.SelectionStart = this.RichBoxBody_T.Text.Length;
                                this.RichBoxBody_T.richTextBox1.ScrollToCaret();
                            }); 
                        }
                        catch (ObjectDisposedException) { /* 忽略，线程安全退出 */ }
                        catch (InvalidOperationException) { /* 忽略 */ }
                    };
                
                }
                // 2. 使用 Task.Run 在后台执行耗时操作
                // 这样主线程不会卡死，而且你可以使用 await 等待它完成
                string result = await Task.Run(() =>
                {
                    // 这里调用底层的同步方法（它内部用 .Result 阻塞，但阻塞的是后台线程，不影响UI）
                    return OpenAICompatibleTranslate.Translate(
                    text,
                    apiurl,
                    _currentCustomTransProvider.ApiKey,
                    _currentCustomTransProvider.ModelName,
                    aIMode,
                    streamCallback // <--- 传入回调
                    );
                });

                return result;
            }
            catch (Exception ex)
            {
                return "翻译接口调用出错: " + ex.Message;
            }
        }

        // 辅助方法：安全的字符串替换
        private string ReplaceLangPlaceholder(string template, string from, string to)
        {
            // 如果原字段是 null 或空，直接返回 null，保持副本结构一致
            if (string.IsNullOrEmpty(template)) return null;

            // 处理默认语言参数
            string f = string.IsNullOrEmpty(from) || from == "auto" ? "Auto Detect" : from;
            string t = string.IsNullOrEmpty(to) ? "Simplified Chinese" : to;

            // 执行替换
            return template.Replace("${fromlang}", f).Replace("${tolang}", t);
        }
    }
}