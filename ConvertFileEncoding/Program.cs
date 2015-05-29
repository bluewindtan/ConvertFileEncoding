using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertFileEncoding
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length != 3)
			{
				Console.WriteLine("args error: incorrect parameters.\n\tConvertFileEncoding.exe FILE SOURCE_ENCODING CONVERT_ENCODING");
				return;
			}

			CConvertEncoding convertEncoding = new CConvertEncoding();
			if (convertEncoding != null)
			{	
				if (args.Length == 3)
				{
					convertEncoding.SetSourceEncoding(args[1]);
					convertEncoding.SetConvertEncoding(args[2]);
				}
				convertEncoding.DoAll(System.Environment.CurrentDirectory + "\\" + args[0]);
			}
		}
	}
}
