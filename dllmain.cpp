// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"
#include <Windows.h>
#include <string>
#include <ShlObj.h>
#include <iostream>
#include <istream>
#include <fstream>
using namespace std;

LPCTSTR SzToLPCTSTR(const char* szString)
{
	LPTSTR  lpszRet;
	size_t  size = strlen(szString) + 1;

	lpszRet = (LPTSTR)malloc(MAX_PATH);
	mbstowcs_s(NULL, lpszRet, size, szString, _TRUNCATE);

	return lpszRet;
}
void me()
{
	try
	{
		while (true)
		{
			char* buff = new char[255];
			SHGetSpecialFolderPathA(HWND_DESKTOP, buff, CSIDL_DESKTOPDIRECTORY, FALSE);
			string desktop = buff;
			string path;
			path.append(desktop);
			path.append("\\Message.txt");
			cout << path << endl;
			ifstream reader(path);
			if (reader.is_open())
			{
				string  line;
				string whole;
				while (!reader.eof())
				{
					reader >> line;
					whole = whole + " " + line;
				}
				reader.close();
				const	char* msgs;
				msgs = whole.c_str();
				LPCTSTR MSG = NULL;
				MSG = SzToLPCTSTR(msgs);
				MessageBox(NULL, MSG, L"Hacked Process", MB_OK);
				const char* pathf = path.c_str();
				remove(pathf);

			}
		}
	}
	catch (exception ex)
	{

	}
}
BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		me();
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}

