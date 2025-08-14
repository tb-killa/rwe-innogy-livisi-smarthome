using System;
using System.Collections.Generic;
using SHCWrapper.Drivers;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public static class Lcd
{
	private const int MAX_LCD_UPDATE_ATTEMPTS = 5;

	private const int COLUMNS_PER_CELL = 3;

	private const int ROWS_PER_CELL = 5;

	private const int CELLS_PER_LINE = 8;

	private const int ROWS = 5;

	private const int COLUMNS = 24;

	private const int BYTES_PER_DISPLAY_BUFFER = 16;

	private const bool O = false;

	private const bool X = true;

	private static readonly Dictionary<char, bool[]> font;

	private static readonly object mutex;

	private static LCD wrappedLcdDriver;

	private static readonly byte[] displayBuffer;

	private static bool x1;

	private static bool x2;

	private static bool x3;

	private static bool s1;

	private static bool s2;

	private static bool s3;

	private static bool s4;

	private static bool backlight;

	private static string text;

	public static string Text
	{
		get
		{
			return text;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("newText");
			}
			lock (mutex)
			{
				text = value;
			}
		}
	}

	public static bool X1
	{
		get
		{
			return x1;
		}
		set
		{
			lock (mutex)
			{
				x1 = value;
			}
		}
	}

	public static bool X2
	{
		get
		{
			return x2;
		}
		set
		{
			lock (mutex)
			{
				x2 = value;
			}
		}
	}

	public static bool X3
	{
		get
		{
			return x3;
		}
		set
		{
			lock (mutex)
			{
				x3 = value;
			}
		}
	}

	public static bool NoInternet
	{
		get
		{
			return s1;
		}
		set
		{
			lock (mutex)
			{
				s1 = value;
			}
		}
	}

	public static bool Antenna
	{
		get
		{
			return s2;
		}
		set
		{
			lock (mutex)
			{
				s2 = value;
			}
		}
	}

	public static bool Alerts
	{
		get
		{
			return s3;
		}
		set
		{
			lock (mutex)
			{
				s3 = value;
			}
		}
	}

	public static bool Messages
	{
		get
		{
			return s4;
		}
		set
		{
			lock (mutex)
			{
				s4 = value;
			}
		}
	}

	public static bool Backlight
	{
		get
		{
			return backlight;
		}
		set
		{
			lock (mutex)
			{
				backlight = value;
				CreateWrappedLcdDriver();
				wrappedLcdDriver.SetBackLightState(backlight);
			}
		}
	}

	public static int MAX_TEXT_LENGTH => 8;

	private static void ClearDisplayBuffer()
	{
		Array.Clear(displayBuffer, 0, displayBuffer.Length);
	}

	private static void RenderTextIntoDisplayBuffer()
	{
		if (text == null)
		{
			return;
		}
		for (int i = 0; i < 8 && i < text.Length; i++)
		{
			if (!font.ContainsKey(text[i]))
			{
				continue;
			}
			bool[] array = font[text[i]];
			for (int j = 0; j < 5; j++)
			{
				for (int k = 0; k < 3; k++)
				{
					if (array[k + j * 3])
					{
						SetCellPixel(i, k, j);
					}
				}
			}
		}
	}

	private static void SetCellPixel(int cell, int columnOfCell, int row)
	{
		SetPixel(columnOfCell + cell * 3, row);
	}

	private static void SetPixel(int column, int row)
	{
		int byteIdx;
		int nibbleIdx;
		int bitIdx;
		if (row < 4)
		{
			byteIdx = column / 2 + 1;
			nibbleIdx = column % 2;
			bitIdx = row;
		}
		else
		{
			byteIdx = (16 - column / 6) % 16;
			nibbleIdx = 1 - column % 6 / 3;
			bitIdx = 3 - column % 3;
		}
		SetBit(byteIdx, nibbleIdx, bitIdx);
	}

	private static void SetBit(int byteIdx, int nibbleIdx, int bitIdx)
	{
		displayBuffer[byteIdx] |= (byte)(1 << nibbleIdx * 4 + bitIdx);
	}

	private static void RenderIconsIntoDisplayBuffer()
	{
		if (x1)
		{
			SetBit(0, 0, 0);
		}
		if (x2)
		{
			SetBit(15, 0, 0);
		}
		if (x3)
		{
			SetBit(13, 0, 0);
		}
		if (s1)
		{
			SetBit(0, 1, 0);
		}
		if (s2)
		{
			SetBit(15, 1, 0);
		}
		if (s3)
		{
			SetBit(14, 1, 0);
		}
		if (s4)
		{
			SetBit(14, 0, 0);
		}
	}

	private static void SendDisplayBufferToLcd()
	{
		CreateWrappedLcdDriver();
		int num = 0;
		bool flag;
		do
		{
			num++;
			flag = wrappedLcdDriver.SendCommand(48, displayBuffer, displayBuffer.Length);
		}
		while (!flag && num < 5);
		if (!flag)
		{
			Console.WriteLine("ERROR: LCD Update failed");
		}
	}

	private static void CreateWrappedLcdDriver()
	{
		if (wrappedLcdDriver == null)
		{
			wrappedLcdDriver = new LCD();
			wrappedLcdDriver.Initialize();
		}
	}

	public static void ClearSymbols()
	{
		lock (mutex)
		{
			s1 = false;
			s2 = false;
			s3 = false;
			s4 = false;
			x1 = false;
			x2 = false;
			x3 = false;
		}
	}

	public static void Update()
	{
		lock (mutex)
		{
			ClearDisplayBuffer();
			RenderTextIntoDisplayBuffer();
			RenderIconsIntoDisplayBuffer();
			SendDisplayBufferToLcd();
		}
	}

	static Lcd()
	{
		Dictionary<char, bool[]> dictionary = new Dictionary<char, bool[]>();
		bool[] value = new bool[15];
		dictionary.Add(' ', value);
		dictionary.Add('1', new bool[15]
		{
			true, true, false, false, true, false, false, true, false, false,
			true, false, true, true, true
		});
		dictionary.Add('2', new bool[15]
		{
			true, true, true, false, false, true, true, true, true, true,
			false, false, true, true, true
		});
		dictionary.Add('3', new bool[15]
		{
			true, true, true, false, false, true, false, true, true, false,
			false, true, true, true, true
		});
		dictionary.Add('4', new bool[15]
		{
			true, false, true, true, false, true, true, true, true, false,
			false, true, false, false, true
		});
		dictionary.Add('5', new bool[15]
		{
			true, true, true, true, false, false, true, true, true, false,
			false, true, true, true, true
		});
		dictionary.Add('6', new bool[15]
		{
			true, true, true, true, false, false, true, true, true, true,
			false, true, true, true, true
		});
		dictionary.Add('7', new bool[15]
		{
			true, true, true, false, false, true, false, false, true, false,
			false, true, false, false, true
		});
		dictionary.Add('8', new bool[15]
		{
			true, true, true, true, false, true, true, true, true, true,
			false, true, true, true, true
		});
		dictionary.Add('9', new bool[15]
		{
			true, true, true, true, false, true, true, true, true, false,
			false, true, true, true, true
		});
		dictionary.Add('0', new bool[15]
		{
			true, true, true, true, false, true, true, false, true, true,
			false, true, true, true, true
		});
		dictionary.Add('A', new bool[15]
		{
			false, true, false, true, false, true, true, false, true, true,
			true, true, true, false, true
		});
		dictionary.Add('B', new bool[15]
		{
			true, true, false, true, false, true, true, true, false, true,
			false, true, true, true, false
		});
		dictionary.Add('C', new bool[15]
		{
			false, true, false, true, false, true, true, false, false, true,
			false, true, false, true, false
		});
		dictionary.Add('D', new bool[15]
		{
			true, true, false, true, false, true, true, false, true, true,
			false, true, true, true, false
		});
		dictionary.Add('E', new bool[15]
		{
			true, true, true, true, false, false, true, true, false, true,
			false, false, true, true, true
		});
		dictionary.Add('F', new bool[15]
		{
			true, true, true, true, false, false, true, true, false, true,
			false, false, true, false, false
		});
		dictionary.Add('G', new bool[15]
		{
			false, true, true, true, false, false, true, false, true, true,
			false, true, true, true, false
		});
		dictionary.Add('H', new bool[15]
		{
			true, false, true, true, false, true, true, true, true, true,
			false, true, true, false, true
		});
		dictionary.Add('I', new bool[15]
		{
			false, true, false, false, true, false, false, true, false, false,
			true, false, false, true, false
		});
		dictionary.Add('J', new bool[15]
		{
			false, false, true, false, false, true, false, false, true, true,
			false, true, false, true, false
		});
		dictionary.Add('K', new bool[15]
		{
			true, false, true, true, false, true, true, true, false, true,
			false, true, true, false, true
		});
		dictionary.Add('L', new bool[15]
		{
			true, false, false, true, false, false, true, false, false, true,
			false, false, true, true, true
		});
		dictionary.Add('M', new bool[15]
		{
			true, false, true, true, true, true, true, false, true, true,
			false, true, true, false, true
		});
		dictionary.Add('N', new bool[15]
		{
			true, true, false, true, false, true, true, false, true, true,
			false, true, true, false, true
		});
		dictionary.Add('O', new bool[15]
		{
			false, true, false, true, false, true, true, false, true, true,
			false, true, false, true, false
		});
		dictionary.Add('P', new bool[15]
		{
			true, true, false, true, false, true, true, false, true, true,
			true, false, true, false, false
		});
		dictionary.Add('Q', new bool[15]
		{
			false, true, false, true, false, true, true, false, true, true,
			true, false, false, true, true
		});
		dictionary.Add('R', new bool[15]
		{
			true, true, false, true, false, true, true, false, true, true,
			true, false, true, false, true
		});
		dictionary.Add('S', new bool[15]
		{
			true, true, true, true, false, false, true, true, true, false,
			false, true, true, true, true
		});
		dictionary.Add('T', new bool[15]
		{
			true, true, true, false, true, false, false, true, false, false,
			true, false, false, true, false
		});
		dictionary.Add('U', new bool[15]
		{
			true, false, true, true, false, true, true, false, true, true,
			false, true, true, true, true
		});
		dictionary.Add('V', new bool[15]
		{
			true, false, true, true, false, true, true, false, true, true,
			false, true, false, true, false
		});
		dictionary.Add('W', new bool[15]
		{
			true, false, true, true, false, true, true, false, true, true,
			true, true, true, false, true
		});
		dictionary.Add('X', new bool[15]
		{
			true, false, true, true, false, true, false, true, false, true,
			false, true, true, false, true
		});
		dictionary.Add('Y', new bool[15]
		{
			true, false, true, true, false, true, true, false, true, false,
			true, false, false, true, false
		});
		dictionary.Add('Z', new bool[15]
		{
			true, true, true, false, false, true, false, true, false, true,
			false, false, true, true, true
		});
		dictionary.Add(':', new bool[15]
		{
			false, false, false, false, true, false, false, false, false, false,
			true, false, false, false, false
		});
		dictionary.Add('.', new bool[15]
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, true, false
		});
		dictionary.Add('-', new bool[15]
		{
			false, false, false, false, false, false, true, true, true, false,
			false, false, false, false, false
		});
		font = dictionary;
		mutex = new object();
		wrappedLcdDriver = null;
		displayBuffer = new byte[16];
		text = null;
	}
}
