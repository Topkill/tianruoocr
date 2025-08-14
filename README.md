## 天若 OCR 文字识别 v6.6.6

天若 OCR 文字识别 v6.6.6 版是基于 https://github.com/AnyListen/tianruoocr 项目进行完善制作而成。

如果能帮到你，欢迎给个 star


## 版本更新
### V6.6.6

#### 更新内容

1. 更新版本号

2. 升级框架至 `.net framework 4.8.1`

3. 修复失效 `ocr` 接口:

      - 百度 ocr
      - 腾讯 ocr

   修复失效翻译接口：

   - 百度翻译
   - 腾讯翻译

   增加 `ocr` 接口：

   - 微信 ocr
     - 目前只支持 x64 版，x86 版暂时无法使用

   增加翻译接口：

   - bing 翻译
   - Microsoft 翻译
   - Yandex 翻译

4. 翻译接口支持用户手动设置源语言和目标语言，默认自动检测源语言和自动判断目标语言

#### 已知问题

- bing 翻译结果有时会丢失换行符，暂时没找到原因
- 快速翻译功能会自动复制选中文本到粘贴板,暂不修复
- 任务管理器里天若的进程名在识别文本后会变为耗时时间
- 可能还有隐藏性bug

#### 未来计划

增加更多接口

#### 注意事项

- 进入设置页面，修改设置后，只有关闭掉设置页面，才会保存设置
- 设置 - 常规 - 音效 - 复制到剪贴板：这个设置选项指的是识别结果自动复制到粘贴板，和音效无关，未来会改一下说明文本
- google 翻译依旧需要正确的网络环境才能使用，如果网络环境不满足，翻译结果会提示 `Translation failed: 发送请求时出错。`
- 本人是 win10 系统，功能测试正常，在 win11 系统下可能会有一些不符合预期的表现

作者未来不一定会有时间更新，欢迎更多开发者参与本项目的开发！

##  特别鸣谢
本项目的开发离不开以下优秀开源项目的支持，感谢以下优秀开源项目的开发者：

https://github.com/AnyListen/tianruoocr：提供了基础代码

https://github.com/wangfreexx/wangfreexx-tianruoocr-cl-paddle：参考修复百度和腾讯翻译接口

https://github.com/ZGGSONG/STranslate：参考实现发现 Gtranslate 库

https://github.com/d4n3436/GTranslate：基于它增加 Microsoft 翻译，Yandex 翻译，重构了 Google 翻译

https://github.com/ZGGSONG/WeChatOcr：基于此项目增加微信 ocr

https://github.com/pot-app/pot-desktop：参考实现增加更多接口





