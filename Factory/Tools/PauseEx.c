#include "C:\Factory\Common\all.h"

int main(int argc, char **argv)
{
	if (!strcmp("1", getEnvLine("@NoPause")))
	{
		LOGPOS();
		return;
	}
	cout("続行するには何かキーを押して下さい . . .\n");
	clearGetKey();
}
