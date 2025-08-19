﻿using System;
using System.Drawing;

namespace TrOCR.Helper
{

	public static class StaticValue
	{
        public static string v_Split;

        public static string v_Restore;

        public static string v_Merge;

        public static string v_googleTranslate_txt;

        public static string v_googleTranslate_back;

        public static int image_h;

        public static int image_w;

        public static string v_single;

        public static Image image_OCR;

        public static string CurrentVersion;

        public static string copy_f;

        public static string content;

        public static bool ZH2EN;

        public static bool ZH2JP;

        public static bool ZH2KO;

        public static bool set_默认;

        public static bool set_拆分;

        public static bool set_合并;

        public static bool set_翻译;

        public static bool set_记录;

        public static bool set_截图;

        public static float DpiFactor;

        public static IntPtr mainHandle;

        public static string note;

        public static string[] v_note;

        public static int NoteCount;

        public static string BD_API_ID = "";

        public static string BD_API_KEY = "";

        public static string TX_API_ID = "";

        public static string TX_API_KEY = "";

        public static string BD_T_API_ID = "";

        public static string BD_T_API_KEY = "";

        public static string TX_T_API_ID = "";

        public static string TX_T_API_KEY = "";

        public static bool IsCapture;

        public static bool v_topmost;

        static StaticValue()
		{
			note = "";
			NoteCount = 40;
			copy_f = "无格式";
			content = "天若OCR更新";
			ZH2EN = true;
			ZH2JP = false;
			ZH2KO = false;
			set_默认 = true;
			set_拆分 = false;
			set_合并 = false;
			set_翻译 = false;
			set_记录 = false;
			set_截图 = false;
			DpiFactor = 1f;
			// 动态获取程序集版本，确保一致性
			CurrentVersion = System.Windows.Forms.Application.ProductVersion;
		}

		
	}
}
