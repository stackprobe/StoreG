#include "C:\Factory\Common\all.h"

#define R_ROOT_DIR "C:\\Factory"
#define W_ROOT_DIR_FORMAT "C:\\home\\GitHub\\Store%c\\Factory"

static void ClearRepoDir(char *dir)
{
	recurClearDir(dir);
}
static int IsCancelCopyToRepoDir(char *rFile, char *wFile, char *mode)
{
	char *ext = getExt(rFile);

	if (
		!_stricmp(ext, "exe") ||
		!_stricmp(ext, "obj")
		)
		return 1;

	return 0;
}
static void CopyToRepoDir(char *rDir, char *wDir)
{
	autoList_t *rPaths = ls(rDir);
	char *rPath;
	uint index;

	foreach (rPaths, rPath, index)
	{
		char *wPath = changeRoot(strx(rPath), rDir, wDir);

		cout("< %s\n", rPath);
		cout("> %s\n", wPath);

		userIsCancel_CopyFile_DM = IsCancelCopyToRepoDir;
		copyPath(rPath, wPath);
		userIsCancel_CopyFile_DM = NULL; // restore

		memFree(wPath);
	}
	releaseDim(rPaths, 1);
}
static void RemoveNotNeedFiles(char *dir)
{
	addCwd(dir);
	{
		recurClearDir("tmp");
		recurClearDir("tmp_data");
	}
	unaddCwd();
}
static char *GetDestRootDir(void)
{
	int alpha;

	for (alpha = 'A'; alpha <= 'Z'; alpha++)
	{
		char *dir = xcout(W_ROOT_DIR_FORMAT, alpha);

		if (existDir(dir))
			return dir;

		memFree(dir);
	}
	error_m("no W_ROOT_DIR");
	return NULL; // dummy
}
int main(int argc, char **argv)
{
	char *destRootDir = GetDestRootDir();

	cout("< %s\n", R_ROOT_DIR);
	cout("> %s\n", destRootDir);

	errorCase(!existDir(R_ROOT_DIR));
	errorCase(!existDir(destRootDir)); // 2bs

	LOGPOS();
	ClearRepoDir(destRootDir);
	LOGPOS();
	CopyToRepoDir(R_ROOT_DIR, destRootDir);
	LOGPOS();
	RemoveNotNeedFiles(destRootDir);
	LOGPOS();

	memFree(destRootDir);
}
