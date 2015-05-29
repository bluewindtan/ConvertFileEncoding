using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ConvertFileEncoding
{
	class CConvertEncoding
	{
		const string S_KEY_NewLine = "\r\n";
		string S_READER_ENCODING = "";
		string S_WRITER_ENCODING = "";
		System.Text.Encoding S_ENCODING_R = null;
		System.Text.Encoding S_ENCODING_W = null;

		List<string> m_listRead = new List<string>();
		List<string> m_listWrite = new List<string>();

		public void SetSourceEncoding(string sEncoding)
		{
			S_READER_ENCODING = sEncoding;
		}

		public void SetConvertEncoding(string sEncoding)
		{
			S_WRITER_ENCODING = sEncoding;
		}

		public void Clear()
		{
			m_listRead.Clear();
			m_listWrite.Clear();
		}

		public void DoAll(string strFilePath)
		{
			if (0 == S_READER_ENCODING.CompareTo(S_WRITER_ENCODING))
			{
				return;
			}
			if (!File.Exists(strFilePath))
			{
				return;
			}
			// Get the encodings for read and write 
			if (null == S_ENCODING_R || 0 != S_READER_ENCODING.CompareTo(S_ENCODING_R.EncodingName))
			{
				S_ENCODING_R = GetEncodingWithName(S_READER_ENCODING);
			}
			if (null == S_ENCODING_W || 0 != S_WRITER_ENCODING.CompareTo(S_ENCODING_W.EncodingName))
			{
				S_ENCODING_W = GetEncodingWithName(S_WRITER_ENCODING);
			}

			// first, read strings from file 
			ReadFile(S_ENCODING_R, strFilePath);

			// then, convert encoding 
			ConverEncoding(S_ENCODING_R, S_ENCODING_W);

			// finally, write strings into file 
			FileInfo fiRead = new FileInfo(strFilePath);
			string strConvertDir = fiRead.DirectoryName + "\\convert";
			if (!Directory.Exists(strConvertDir))
			{
				Directory.CreateDirectory(strConvertDir);
			}
			WriteFile(S_ENCODING_W, strConvertDir + "\\" + fiRead.Name);
		}

		public bool ReadFile(Encoding encodeReader, string strFilePath)
		{
			if (!File.Exists(strFilePath))
			{
				return false;
			}

			using (StreamReader sr = new StreamReader(strFilePath, encodeReader))
			{
				Clear();

				string strLine = null;
				while ((strLine = sr.ReadLine()) != null)
				{
					m_listRead.Add(strLine);
				}
				sr.Close();
			}

			return true;
		}

		public bool WriteFile(Encoding encodeWriter, string strFilePath)
		{
			using (StreamWriter sw = new StreamWriter(strFilePath, false, encodeWriter))
			{
				foreach (string strText in m_listWrite)
				{
					sw.WriteLine(strText);
				}
				sw.Flush();
				sw.Close();
			}

			return true;
		}

		public void ConverEncoding(Encoding encodingRead, Encoding encodingWrite)
		{
			foreach (string strText in m_listRead)
			{
				byte[] bytesRead = encodingRead.GetBytes(strText);
				byte[] bytesWrite = Encoding.Convert(encodingRead, encodingWrite, bytesRead);
				string strWrite = encodingWrite.GetString(bytesWrite);
				m_listWrite.Add(strWrite);
			}
		}

		public static System.Text.Encoding GetEncodingWithName(String strName)
		{
			Encoding encoding = null;
			try
			{
				encoding = Encoding.GetEncoding(strName);
			}
			catch
			{
				if (null == encoding)
				{
					encoding = Encoding.GetEncoding(Convert.ToInt32(strName));
				}
			}

			return encoding;
		}
	}
}
