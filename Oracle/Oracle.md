#oracle

##命令框设置
*	spool录屏
	*	spool d:/sql.txt 开始录屏
	*	spool off 结束录屏
*	行宽
	*	show linesize 显示行宽
	*	set linesize 300 设置行宽
*	列宽 a8表示8位字符串，9999表示4位数字
	*	col 列名 for a8
	*	col 列名 for 9999
*	页数
	*	set pagesize 20 设置一页显示的大小

##SQL> 命令
*	/ 执行上一条语句
*	conn 用户名/密码 连接
*	disconn 断开连接
*	host cls或clear screen 清屏
*	set timing on和set timing off SQL执行时间的开关
*	set serveroutput on 开启控制台输出

##监听
*	lsnrctl start 启动监听
*	lsnrctl stop 停止监听
*	lsnrctl status 查看监听状态

##登录
*	本地登录
	*	sqlplus / as sysdba 管理员登录
	*	sqlplus scott/scott 普通用户登录
	*	sqlplus /nolog 未连接登录
*	远程登录
	*	sqlplus scott/scott@ip:port/orcl

##oracle结构
*	逻辑结构
	*	表空间
		*	区管理
			*	本地管理
			*	字典管理
		*	类型
			*	永久
			*	临时
			*	还原
		*	状态
			*	读写 read write
			*	只读 read only
			*	脱机 offline
		*	文件
			*	小文件
			*	大文件
		*	创建表空间
			*	create smallfile tablespace demo datafile 'D:\APP\USERNAME\ORADATA\ORCL\DEMO.DBF' size 100M autoextend on next 10M maxsize 200M logging extent management local segment space management auto;
		*	重命名表空间
			*	alter tablespace aaa rename to bbb;
		*	修改表空间状态
			*	alter tablespace aaa read only;
			*	alter tablespace aaa read write;
			*	alter tablespace aaa offline;
			*	alter tablespace aaa online;
		*	删除表空间
			*	drop tablespace demo including contents and datafiles cascade constraints;
		*	查看表空间的对象
			*	select owner, segment_name,segment_type from dba_segments where tablespace_name=upper('demo');
		*	修改表空间压缩
			*	alter tablespace demo row store compress advanced;
			*	alter tablespace demo default compress basic;
			*	alter tablespace demo default nocompress;
		*	添加临时文件
			*	alter tablespace temp add tempfile '/u01/dbfile/o18c/temp02.dbf' size 500m;
	*	段
	*	区
	*	块
*	物理结构
	*	数据文件
		*	修改数据文件大小
			*	alter database datafile 'D:\APP\USERNAME\ORADATA\ORCL\DEMO.DBF' resize 150M;
	*	临时文件
		*	修改临时文件大小
			*	alter database tempfile '/u01/dbfile/o18c/temp01.dbf' resize 500m;
	*	控制文件
		*	select distinct type from v$controlfile_record_section;
			*	显示控制文件信息类型
		*	show parameter control_files
			*	显示控制文件位置
		*	select name from v$controlfile;
			*	查询控制文件位置
		*	增删控制文件
			*	1.修改control_files参数
				*	alter system set control_files='D:\APP\USERNAME\ORADATA\ORCL\CONTROL01.CTL','D:\APP\USERNAME\ORADATA\ORCL\CONTROL02.CTL' scope=spfile;					
			*	2.复制或删除磁盘上的控制文件
			*	3.重启数据库实例，使参数生效
		*	备份控制文件到跟踪文件
			*	alter database backup controlfile to trace as 'D:\app\USERNAME\tmp\bak.sql' noresetlogs;
	*	联机重做日志文件
		*	SELECT a.group#,a.thread#,a.status grp_status,b.member member,b.status mem_status,a.bytes/1024/1024 mbytes FROM v$log a,v$logfile b WHERE a.group# = b.group# ORDER BY a.group#, b.member;
			*	查看日志组和成员的信息
		*	select count(*),to_char(first_time,'YYYY:MM:DD:HH24')from v$log_history group by to_char(first_time,'YYYY:MM:DD:HH24') order by 2;
			*	查看日志的切换数量
		*	ALTER DATABASE ADD LOGFILE GROUP 4 ( 'D:\APP\USERNAME\ORADATA\ORCL\redo4a.log', 'D:\APP\USERNAME\ORADATA\ORCL\redo4b.log') SIZE 51200K;
			*	添加日志组
		*	REUSE参数
			*	如果文件存在，加上reuse可以使用新的大小重新使用它。
		*	select group#, status, archived, thread#, sequence# from v$log;
			*	查看日志组信息
		*	alter database drop logfile group 1;
			*	删除日志组
		*	alter system switch logfile;
			*	切换日志组，使日志组处于非当前状态
		*	alter system checkpoint;
			*	发出更改系统检查点命令使日志组处于非活动状态
		*	select member from v$logfile;
			*	查看日志成员
		*	alter database add logfile member 'D:\APP\USERNAME\ORADATA\ORCL\redo2a.log' to group 2;
			*	添加日志组成员
		*	alter database clear logfile group 2;
			*	清除日志组2，如果日志成员状态是invalid，可变成null。
		*	alter database drop logfile member 'D:\APP\USERNAME\ORADATA\ORCL\REDO02.LOG';
			*	删除日志组的成员
		*	日志组状态
			*	current 日志写入器当前正在写入的日志组
			*	active 日志组中提交的事务引起的数据改变还没有完全从DB buffer cache写入到数据文件，实际是由于当前数据文件头部的scn值还位于状态为active日志组的low scn 和next scn 内
			*	inactive 崩溃恢复不需要日志组，可能已经存档，也可能还没有存档
			*	unused 日志组从未被写入;它是最近创建的
			*	clearing 日志组被ALTER DATABASE CLEAR清除日志文件命令。
			*	clearing_current 正在清除当前日志组中的一个关闭线程
		*	日志成员状态
			*	invalid 无效的
			*	deleted 未使用
			*	stale 日志文件成员的内容不完整
			*	null 正在使用
	*	归档日志文件
	*	参数文件
		*	show parameter spfile 显示spfile的值
		*	show parameter control_files 显示control_files的值
	*	密码文件
	*	监听文件
		*	Net Manager 监听器添加地址
		*	D:\app\username\product\11.2.0\dbhome_1\NETWORK\ADMIN
![architecture](imgs/architecture.jpg)
![log](imgs/tablespace.png)
![log](imgs/log.png)
![redo](imgs/redo.png)


##oracle实例启动
*	startup [option] [exclusive|shared] 独占或共享启动
	*	force 通过shutdown abort关闭实例在启动它，等于shutdown abort;startup open;
	*	restrict 仅允许拥有"RESTRICTED SESSION"权限的用户才能连接到数据库
	*	pfile 指定pfile参数文件启动实例
		*	startup pfile='D:\app\username\admin\orcl\pfile\init.ora.917201916299';
	*	quiet 在启动实例时禁止显示SGA信息
	*	nomount 启动后台进程并分配内存;不读取控制文件
	*	mount 启动后台进程，分配内存，读取控制文件
	*	open 启动后台进程，分配内存，读取控制文件，并打开在线重做日志和数据文件
	*	open recover 在打开数据库之前尝试介质恢复，等于startup mount;recover database;alter database open;
	*	open read only 以只读模式打开数据库
	*	upgrade 在升级数据库时使用
	*	downgrade 降级数据库时使用
![startup](imgs/startup.jpg)

##oracle实例停止
*	shutdown
	*	normal 等待退出用户活动会话后再关闭。
	*	transactional 等待事务完成，然后终止会话。
	*	transactional local 仅对本地实例执行事务关闭。
	*	immediate 立即终止活动会话。打开的事务被回滚。
	*	abort 立即终止实例。事务被终止，并且没有回滚。

##用户和安全
*	查询用户
	*	select username from dba_users	
*	解锁账户
	*	alter user <username> account unlock;
*	创建用户
	*	CREATE USER LISI PROFILE DEFAULT IDENTIFIED BY lisi PASSWORD EXPIRE QUOTA 50M ON USERS DEFAULT TABLESPACE USERS TEMPORARY TABLESPACE TEMP ACCOUNT UNLOCK
*	授权
	*	grant create session to scott;
*	回收
	*	revoke create session from scott;
*	修改用户的默认表空间和临时表空间
	*	alter user scott default tablespace users temporary tablespace temp;
*	修改密码
	*	alter user <username> identified by <new password>;
	*	passw scott
	*	password
*	锁定用户
	*	alter user scott account lock;
*	限额
	*	alter user scott quota 500M on users;
*	删除用户
	*	drop user scott cascade;
*	概要文件
	*	resource_limit 为ture资源限制才能生效
*	create role myrole;
	*	创建角色
*	授予角色
	*	grant myrole to scott;
*	回收角色
	*	revoke myrole from scott;


##Data Pump（数据泵）
*	create directory dp_dir as 'D:\app\USERNAME\oradump';
	*	创建一个数据库目录对象，该对象对应于磁盘上的物理位置。此位置将用于保存导出和日志文件
*	select owner,directory_name,directory_path from dba_directories;
	*	查看数据库目录对象
*	grant read, write on directory dp_dir to <username>;
	*	给需要的用户授予权限
*	expdp 导出
*	impdp 导入





##Oracle Managed File (OMF)
*	参数
	*	DB_CREATE_FILE_DEST
	*	DB_CREATE_ONLINE_LOG_DEST_N
	*	DB_RECOVERY_FILE_DEST
*	自动化表空间
	*	开启OMF特性
		*	alter system set db_create_file_dest='D:\app\username\omfdata';
	*	创建表空间
		*	create tablespace inv1;默认文件大小100m
		*	create tablespace inv2 datafile size 20m;指定文件大小20m

##ASM

##修改参数文件（逻辑上）
*	alter system

##修改控制文件（物理上）
*	alter database
	*	alter database mount;挂载数据库实例
	*	alter database open;打开数据库实例

##修改会话设置（会话期间生效）
*	alter session
	*	alter session set current_schema = hr; 


##静态视图
*	DBA
*	ALL
*	USER
*	CDB

##动态视图
*	select name from v$controlfile;
*	select name, open_mode, created, current_scn from v$database;
	*	读取控制文件的数据库信息
*	select name from v$datafile;
	*	查看数据文件
*	select name, bytes from v$tempfile;
	*	查看临时文件

##获取参数
*	show parameter control_files 显示控制文件路径
*	show parameter spfile 显示spfile文件路径


##脚本
	示例：
	SQL> @create 5G 500M
	脚本文件create.sql的内容

	define tbsp_large=&1
	define tbsp_med=&2
	--
	create tablespace reg_data
	datafile '/u01/dbfile/o12c/reg_data01.dbf'
	size &&tbsp_large
	extent management local
	uniform size 128k
	segment space management auto;
	--
	create tablespace reg_index
	datafile '/u01/dbfile/o12c/reg_index01.dbf'
	size &&tbsp_med
	extent management local
	uniform size 128k
	segment space management auto;


##oradim

##oradebug
