#	Tomcat

* 下载后解压

> http://tomcat.apache.org/ 
>
> 建议下载包含service.bat的绿色包[64-bit Windows zip](https://mirrors.tuna.tsinghua.edu.cn/apache/tomcat/tomcat-9/v9.0.37/bin/apache-tomcat-9.0.37-windows-x64.zip) ([pgp](https://downloads.apache.org/tomcat/tomcat-9/v9.0.37/bin/apache-tomcat-9.0.37-windows-x64.zip.asc), [sha512](https://downloads.apache.org/tomcat/tomcat-9/v9.0.37/bin/apache-tomcat-9.0.37-windows-x64.zip.sha512)) 

* 配置环境变量

>新增系统变量CATALINA_HOME，值C:\Program Files\apache-tomcat-9.0.37
>
>Path后追加;%CATALINA_HOME%\bin

* 运行startup.bat

> 通过管理员方式打开命令行运行bin目录下的startup.bat，如果没有报错并且浏览器访问http://localhost:8080访问正常，则说明环境OK

* 安装Window服务

> 1、通过管理员方式打开命令行，并且进入tomcat的bin目录下，运行service.bat install安装服务（service.bai uninstall卸载服务）
>
> 2、运行tomcat的bin目录下的tomcat9w.exe，第四项勾选Use default
>
> 3、启动服务net start tomcat9，停止服务net stop tomcat9，注意：必须是通过管理员方式打开的命令行

* 注意

> 检测C:\Program Files\apache-tomcat-9.0.37目录的权限
>
> 这些组或用户名 ALL APPLICATION PACKAGES、SYSTEM、Administrators、Users都必须拥有完全控制的权限，不然会发生一些疑难问题
