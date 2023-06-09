/*
	/RD = rd
	/MD = md
	/MC = md + compact
	/RM = rd + md
	/RC = rd + md + compact
*/

#include "C:\Factory\Common\all.h"

static void CompactDir(char *dir, int compactFlag)
{
	if (compactFlag) // ? ���k����B
	{
		coExecute_x(xcout("Compact.exe /C /S:\"%s\"", dir));
	}
	else // ? ���k��������B
	{
		coExecute_x(xcout("Compact.exe /U /S:\"%s\"", dir));
	}
}

static int CompactMode;

static void DoRD(char *dir)
{
	for (; ; )
	{
		coExecute_x(xcout("RD /S /Q \"%s\"", dir));

		if (!existDir(dir))
			break;

		coSleep(1000);
	}
}
static void DoMD(char *dir)
{
	for (; ; )
	{
		coExecute_x(xcout("MD \"%s\"", dir));

		if (existDir(dir))
			break;

		coSleep(1000);
	}
	if (CompactMode)
		CompactDir(dir, 1);
}
static void DoRDMD(char *dir)
{
	if (isEmptyDir(dir))
	{
		CompactDir(dir, CompactMode);
	}
	else
	{
		DoRD(dir);
		DoMD(dir);
	}
}

int main(int argc, char **argv)
{
	while (hasArgs(1))
	{
		if (argIs("/RD"))
		{
			DoRD(nextArg());
			continue;
		}
		if (argIs("/MD"))
		{
			DoMD(nextArg());
			continue;
		}
		if (argIs("/MC") || argIs("/CD"))
		{
			CompactMode = 1;
			DoMD(nextArg());
			CompactMode = 0;
			continue;
		}
		if (argIs("/RM"))
		{
			DoRDMD(nextArg());
			continue;
		}
		if (argIs("/RC"))
		{
			CompactMode = 1;
			DoRDMD(nextArg());
			CompactMode = 0;
			continue;
		}
		error();
	}
}
