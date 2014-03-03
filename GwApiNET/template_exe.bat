for /r %1 %%f in (*.tt) do (
 for /f "tokens=3,4 delims==, " %%a in ("%%f") do (
  if %%~a==extension "%CommonProgramFiles%\Microsoft Shared\TextTemplating\1.2\texttransform.exe" -out %%~pnf.%%~b -P %%~pf -P "%ProgramFiles%\Reference Assemblies\Microsoft\Framework\v3.5" %%f
 )
)
echo Exit Code = %ERRORLEVEL%