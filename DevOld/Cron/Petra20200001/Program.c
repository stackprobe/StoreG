#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <windows.h>

// ---- error

#define Error() \
	Error_LN(__LINE__)

static void Error_LN(int lineno)
{
	char command[1024]; // rough size

	printf("FATAL %d\n", lineno);
	sprintf(command, "START *[ERROR-%d]", lineno);
	system(command);
	exit(1);
}

// ---- utils

static int IsExistFile(char *file)
{
	FILE *fp = fopen(file, "rb");

	if (fp && fclose(fp) != 0)
		Error();

	return !!fp;
}
static char *GetStrDateTime(time_t t)
{
	static char str[26]; // size: ctime ret

	strcpy(str, ctime(&t));

	if (str[8] == ' ')
		str[8] = '0';

	str[24] = '\0';
	return str;
}

// ---- main

#define STOP_EV_NAME "Cron_STOP_{baf19612-401e-4417-9b1f-48b0b8b72501}"
#define BATCH_NAME_MAX 12
#define BATCH_MAX (4 * 99)

typedef struct Batch_st
{
	char Name[BATCH_NAME_MAX];
	int PeriodSec;
	int RemainingSec;
}
Batch_t;

static Batch_t Batches[BATCH_MAX];
static int BatchCount;

static void CollectBatch_SS(int suffix, int scale)
{
	char name[BATCH_NAME_MAX];
	int count;

	for (count = 1; count <= 99; count++)
	{
		sprintf(name, "Cron%d%c.bat", count, suffix);

		if (IsExistFile(name))
		{
			Batch_t *i = Batches + BatchCount++;

			strcpy(i->Name, name);
			i->PeriodSec = count * scale;
			i->RemainingSec = 0;
		}
	}
}
static void CollectBatch(void)
{
	CollectBatch_SS('s', 1);
	CollectBatch_SS('m', 60);
	CollectBatch_SS('h', 3600);
	CollectBatch_SS('d', 86400);
}
static int ExecuteAllBatchIfTimeout(int elapsedSec)
{
	int nextWaitSec = 60; // rough limit
	int index;

	for (index = 0; index < BatchCount; index++)
	{
		Batch_t *i = Batches + index;

		i->RemainingSec -= elapsedSec;

		if (i->RemainingSec <= 0)
		{
			printf("# %s %s\n", GetStrDateTime(time(NULL)), i->Name);

			system(i->Name);

			printf("\n");

			i->RemainingSec = i->PeriodSec;
		}
	}
	for (index = 0; index < BatchCount; index++)
	{
		Batch_t *i = Batches + index;

		if (nextWaitSec > i->RemainingSec)
			nextWaitSec = i->RemainingSec;
	}
	return nextWaitSec;
}
void main(int argc, char **argv)
{
	HANDLE evStop = CreateEventA(NULL, 0, 0, STOP_EV_NAME);

	if (evStop == NULL)
		Error();

	if (argc == 2 && !_stricmp(argv[1], "/S"))
	{
		printf("Cron-STOP\n");

		SetEvent(evStop);
	}
	else
	{
		int waitSec = 0;

		if (argc != 1) // ? 不明なコマンド引数が指定されている。
			Error();

		CollectBatch();

		printf("Cron-START\n");

		while (WaitForSingleObject(evStop, waitSec * 1000) == WAIT_TIMEOUT)
			waitSec = ExecuteAllBatchIfTimeout(waitSec);

		printf("Cron-END\n");
	}
	CloseHandle(evStop);
}
