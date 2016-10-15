echo off

echo 127.0.0.1 fluenthateoas.web >> %WINDIR%\System32\Drivers\Etc\Hosts

%windir%\system32\inetsrv\appcmd add site /name:"fluenthateoas.web" /physicalPath:"%~dp0sample\SampleApi" /bindings:"http://fluenthateoas.web:80"

ipconfig /flushdns

pause
