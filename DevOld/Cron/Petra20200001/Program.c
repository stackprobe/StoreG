#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <windows.h>

#define Error() \
	Error_LN(__LINE__)

static void Error_LN(int lineno)
{
	char command[1024]; // rough size

	printf("FATAL %d\n", lineno);
	sprintf(command, "START "" *[ERROR-%d]", lineno);
	system(command);
	exit(1);
}

// ----

static int GetMax(int a, int b)
{
	return a < b ? b : a;
}
static int GetMin(int a, int b)
{
	return a < b ? a : b;
}

#define STOP_EV_NAME "{baf19612-401e-4417-9b1f-48b0b8b72501}_STOP_CRON"
#define PERIOD_SEC 60

static time_t CurrTime;

static int IsFairBatch(char *batch)
{
	FILE *fp = fopen(batch, "rb");
	int ret = 0;

	if (!fp) // ? バッチファイルは存在しない。
		goto endFunc_NC;

	if (fgetc(fp) == EOF) // ? バッチファイルは空のファイル
		goto endFunc;

	ret = 1;

endFunc:
	if (fclose(fp) != 0)
		Error();

endFunc_NC:
	printf("IsFairBatch_ret: %d\n", ret);
	return ret;
}
static void RunIfTimeout(char *batch, int periodSec)
{
	int remSec = (int)(CurrTime % periodSec);

	printf("Batch: %s (%d, %d, %d)\n", batch, periodSec, remSec, periodSec - remSec);

	if (remSec == 0 && IsFairBatch(batch))
		system(batch);

	printf("Batch_End\n");
}
static void RunIfTimeoutAll(void)
{
	printf("----\n");
	printf("%s", ctime(&CurrTime));

	// 周期は PERIOD_SEC の倍数であること。

	RunIfTimeout(".\\Cron1m.bat"  , 60 * 1);
	RunIfTimeout(".\\Cron3m.bat"  , 60 * 3);
	RunIfTimeout(".\\Cron10m.bat" , 60 * 10);
	RunIfTimeout(".\\Cron30m.bat" , 60 * 30);
	RunIfTimeout(".\\Cron1h.bat"  , 3600 * 1);
	RunIfTimeout(".\\Cron3h.bat"  , 3600 * 3);
	RunIfTimeout(".\\Cron10h.bat" , 3600 * 10);
	RunIfTimeout(".\\Cron30h.bat" , 3600 * 30);
	RunIfTimeout(".\\Cron1d.bat"  , 86400 * 1);
	RunIfTimeout(".\\Cron3d.bat"  , 86400 * 3);
	RunIfTimeout(".\\Cron10d.bat" , 86400 * 10);
	RunIfTimeout(".\\Cron30d.bat" , 86400 * 30);
}
void main(int argc, char **argv)
{
	HANDLE evStop = CreateEventA(NULL, 0, 0, STOP_EV_NAME);
	int waitSec;

	if (evStop == NULL)
		Error();

	if (argc == 2 && !_stricmp(argv[1], "/S"))
	{
		SetEvent(evStop);
		printf("SetEvent_Stop\n");
		goto endProc;
	}
	if (argc != 1) // ? 不明なコマンド引数が指定されている。
		Error();

	printf("ST_Cron\n");

	CurrTime = 0;
	RunIfTimeoutAll();
	CurrTime = (time(NULL) / PERIOD_SEC) * PERIOD_SEC;

	do
	{
		RunIfTimeoutAll();
		CurrTime += PERIOD_SEC;

		waitSec = CurrTime - time(NULL);
		waitSec = GetMax(waitSec, PERIOD_SEC - PERIOD_SEC / 2);
		waitSec = GetMin(waitSec, PERIOD_SEC + PERIOD_SEC / 2);

		printf("waitSec: %d\n", waitSec);
	}
	while (WaitForSingleObject(evStop, waitSec * 1000) == WAIT_TIMEOUT);

	printf("ED_Cron\n");

endProc:
	CloseHandle(evStop);
}
