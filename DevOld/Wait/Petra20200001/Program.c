#pragma comment(lib, "user32.lib") // for MessageBox

#include <stdio.h>
#include <conio.h>
#include <windows.h>
#include <winuser.h> // for MessageBox

static void ShowErrorDialog(void)
{
	MessageBox(NULL, "ERROR !", __argv[0], MB_OK | MB_ICONSTOP | MB_TOPMOST);
}

#define error() \
	{ printf("ERROR %d 0x%x\n", __LINE__, GetLastError()); ShowErrorDialog(); exit(1); }

#define errorCase(status) \
	{ if ((status)) error(); }

#define GetMin(a, b) ((a) < (b) ? (a) : (b))
#define GetMax(a, b) ((a) < (b) ? (b) : (a))

#define WAIT_SEC_MIN 3
#define WAIT_SEC_MAX 999999

#define SLEEP_MILLIS 250

main()
{
	int waitSec;
	int millis;
	int lastPrintRemSec = -1;

	errorCase(__argc != 2);
	waitSec = atoi(__argv[1]);
	errorCase(waitSec < 0 || WAIT_SEC_MAX < waitSec);
	millis = waitSec * 1000;

	for (; ; )
	{
		if (lastPrintRemSec != millis / 1000)
		{
			lastPrintRemSec = millis / 1000;
			printf("\r%d 秒待っています。中止するには ESC 続行するには ENTER を押して下さい ... ", lastPrintRemSec);
		}

		while (_kbhit())
		{
			int key = _getch();

			if (key == 0x1b) // ? ESC
			{
				printf("\n中止します。\n");
				exit(1);
			}
			if (key == 0x0d) // ? ENTER
			{
				printf("\n続行します。\n");
				exit(0);
			}

			if (key == '+') // プラス 1 分
			{
				millis += 60000;
			}
			else if (key == '-') // マイナス 1 分
			{
				millis -= 60000;
			}
			else if (key == '*') // プラス 1 時間
			{
				millis += 3600000;
			}
			else if (key == '/') // 残り時間クリア
			{
				millis = 0;
			}

			millis = GetMin(millis, WAIT_SEC_MAX * 1000);
			millis = GetMax(millis, WAIT_SEC_MIN * 1000);
		}

		if (millis <= 0)
		{
			printf("\nタイムアウトにより続行します。\n");
			exit(0);
		}

		Sleep(SLEEP_MILLIS);
		millis -= SLEEP_MILLIS;
	}
}
