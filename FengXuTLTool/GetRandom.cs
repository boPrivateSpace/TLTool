using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FengXuTLTool
{
    public  class GetRandom
    {

		//数字+字母
		private const string CHAR = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";


		/// <summary>
		/// 随机排序
		/// </summary>
		/// <param name="charList"></param>
		/// <returns></returns>
		private static List<string> SortByRandom(List<string> charList)
		{
			Random rand = new Random();
			for (int i = 0; i < charList.Count; i++)
			{
				int index = rand.Next(0, charList.Count);
				string temp = charList[i];
				charList[i] = charList[index];
				charList[index] = temp;
			}

			return charList;
		}

		/// <summary>
		/// 获取随机字符串
		/// </summary>
		/// <param name="len"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public static  List<string> GetRandString(int len, int count, string startStr)
		{
			double max_value = Math.Pow(36, len);
			if (max_value > long.MaxValue)
			{
				return null;
			}

			long all_count = (long)max_value;
			long stepLong = all_count / count/1000;
			int step = (int)stepLong;

			long begin = 0;
			List<string> list = new List<string>();
			Random rand = new Random();
			while (true)
			{
				long value = rand.Next(1, step) + begin;
				begin += step;
				list.Add(GetChart(len, value, startStr));
				if (list.Count == count)
				{
					break;
				}
			}

			list = SortByRandom(list);

			return list;
		}

		/// <summary>
		/// 将数字转化成字符串
		/// </summary>
		/// <param name="len"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		private static string GetChart(int len, long value,string startStr)
		{
			StringBuilder str = new StringBuilder();
			str.Append(startStr);
			while (true)
			{
				str.Append(CHAR[(int)(value % 36)]);
				value = value / 36;
				if (str.Length == len)
				{
					break;
				}
			}

			return str.ToString();
		}
	}
}
