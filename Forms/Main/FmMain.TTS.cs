using System;
using System.IO;
using System.Media;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrOCR.Helper;

namespace TrOCR
{
	public partial class FmMain
	{
		/// <summary>
		/// 启动TTS文本朗读功能，在新线程中执行TTS_thread方法
		/// </summary>
		public void TTS()
		{
			new Thread(TTS_thread).Start();
		}

		/// <summary>
		/// TTS文本朗读线程函数，负责获取文本内容、检测语言、下载语音数据并调用播放方法
		/// </summary>
		public void TTS_thread()
		{
            try
            {
                // 1. === 获取 Token (使用 using 自动释放) ===
                string text;
                HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}?{1}", "http://aip.baidubce.com/oauth/2.0/token", "grant_type=client_credentials&client_id=iQekhH39WqHoxur5ss59GpU4&client_secret=8bcee1cee76ed60cdfaed1f2c038584d"));

                using (HttpWebResponse tokenResponse = (HttpWebResponse)tokenRequest.GetResponse())
                using (Stream responseStream = tokenResponse.GetResponseStream())
                using (StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8")))
                {
                    text = streamReader.ReadToEnd();
                }
                // <--- 在这里，tokenResponse, responseStream, 和 streamReader 已被自动关闭和释放

                string text2 = !contain_ch(htmltxt) ? "zh" : "zh";

                // 2. === 构建 TTS 请求 ===
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Concat(new string[]
                {
            		"http://tsn.baidu.com/text2audio?lan=" + text2 + "&ctp=1&cuid=abcdxxx&tok=",
            		((JObject)JsonConvert.DeserializeObject(text))["access_token"].ToString(),
            		"&tex=",
            		HttpUtility.UrlEncode(htmltxt.Replace("***", "")),
            		"&vol=9&per=0&spd=5&pit=5"
                }));
                httpWebRequest.Method = "POST";

                byte[] array2;

                // 3. === 获取音频流 (使用 using 自动释放) ===
                // 修复了原版代码在 while 循环中重复调用 GetResponseStream() 的 Bug
                using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                using (Stream audioStream = httpWebResponse.GetResponseStream()) // <-- 只调用一次
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    byte[] array = new byte[16384];
                    int num;
                    while ((num = audioStream.Read(array, 0, array.Length)) > 0)
                    {
                        memoryStream.Write(array, 0, num);
                    }
                    array2 = memoryStream.ToArray();
                }
                // <--- 在这里，httpWebResponse, audioStream, 和 memoryStream 已被自动关闭和释放

                ttsData = array2;
                if (speak_copyb == "朗读" || voice_count == 0)
                {
                    Invoke(new Translate(Speak_child));
                    speak_copyb = "";
                }
                else
                {
                    Invoke(new Translate(TTS_child));
                }
                voice_count++;
            }
            catch (Exception ex)
            {
                if (ex.ToString().IndexOf("Null") <= -1)
                {
                    MessageBox.Show("文本过长，请使用右键菜单中的选中朗读！", "提醒");
                }
            }
        }

		/// <summary>
		/// TTS文本朗读播放函数，在UI线程中执行，负责播放已下载的语音数据
		/// </summary>
		public void TTS_child()
		{
			// 检查主文本框或翻译文本框是否有内容
			if (RichBoxBody.Text != null || RichBoxBody_T.Text != "")
			{
				// 如果正在播放，则关闭播放并返回
				if (speaking)
				{
					HelpWin32.mciSendString("close media", null, 0, IntPtr.Zero);
					speaking = false;
					return;
				}
				// 获取系统临时目录路径
				var tempPath = Path.GetTempPath();
				// 构造临时音频文件路径
				var text = tempPath + "\\声音.mp3";
				try
				{
					// 将语音数据写入临时文件
					File.WriteAllBytes(text, ttsData);
				}
				catch
				{
					// 如果写入失败，尝试使用另一个文件名
					text = tempPath + "\\声音1.mp3";
					File.WriteAllBytes(text, ttsData);
				}
				// 播放音频文件
				PlaySong(text);
				// 设置播放状态为正在播放
				speaking = true;
			}
		}

		/// <summary>
		/// 主文本框语音朗读点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void Main_Voice_Click(object sender, EventArgs e)
		{
			RichBoxBody.Focus();
			speak_copyb = "朗读";
			htmltxt = RichBoxBody.SelectText;
			HelpWin32.SendMessage(Handle, 786, 590);
		}

		/// <summary>
		/// 翻译文本框语音朗读点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void Trans_Voice_Click(object sender, EventArgs e)
		{
			RichBoxBody_T.Focus();
			speak_copyb = "朗读";
			htmltxt = RichBoxBody_T.SelectText;
			HelpWin32.SendMessage(Handle, 786, 590);
		}

		/// <summary>
		/// 执行文本语音朗读功能
		/// </summary>
		public void Speak_child()
		{
			// 检查主文本框或翻译文本框是否有内容
			if (RichBoxBody.Text != null || RichBoxBody_T.Text != "")
			{
				var tempPath = Path.GetTempPath();
				var text = tempPath + "\\声音.mp3";
				try
				{
					File.WriteAllBytes(text, ttsData);
				}
				catch
				{
					text = tempPath + "\\声音1.mp3";
					File.WriteAllBytes(text, ttsData);
				}
				PlaySong(text);
				speaking = true;
			}
		}

		/// <summary>
		/// 播放指定的音频文件
		/// </summary>
		/// <param name="file">音频文件路径</param>
		public void PlaySong(string file)
		{
			HelpWin32.mciSendString("close media", null, 0, IntPtr.Zero);
			HelpWin32.mciSendString("open \"" + file + "\" type mpegvideo alias media", null, 0, IntPtr.Zero);
			HelpWin32.mciSendString("play media notify", null, 0, Handle);
		}
	}
}
