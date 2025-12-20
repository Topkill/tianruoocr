using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrOCR.Models;

namespace TrOCR.Interfaces
{
    // 所有接口设置控件都要实现这个接口
    public interface IOCRSettingControl
    {
        // 传入配置对象，控件自动把数据填入 TextBox
        void LoadConfig(OCRBaseProfile profile);

        // 控件把 TextBox 里的内容保存回配置对象
        void SaveConfig(OCRBaseProfile profile);
    }
    public interface ITranslateSettingControl
    {
        // 传入配置对象，控件自动把数据填入 TextBox
        void LoadConfig(TranslateBaseProfile profile);

        // 控件把 TextBox 里的内容保存回配置对象
        void SaveConfig(TranslateBaseProfile profile);
    }
}
