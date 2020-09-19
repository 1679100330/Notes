# RabbitMQ集群搭建

> 准备两台服务器，各自的计算机命名rabbitmq-one和rabbitmq-two
>
> 并且两台服务器可以互相ping通

###	安装ERLANG语言包

> 在下载地址https://www.erlang.org/downloads下载[OTP 23.0 Windows 64-bit Binary File ](http://erlang.org/download/otp_win64_23.0.exe)
>
> 右键管理员安装，如果安装目录C:\Program Files\erl-23.0下没有bin文件夹，
>
> 卸载后下载msvcr120.dll拷贝到C:\Windows\system下在重新安装，
>
> 添加环境变量ERLANG_HOME值C:\Program Files\erl-23.0，添加%ERLANG_HOME%\bin到Path，
>
> 命令行输入erl -version查看是否成功安装

###	安装RaibbitMQ

> 下载地址https://www.rabbitmq.com/download.html下载https://github.com/rabbitmq/rabbitmq-server/releases/download/v3.8.8/rabbitmq-server-3.8.8.exe
>
> 点击安装后添加环境变量RABBITMQ_SERVER值C:\Program Files\RabbitMQ Server\rabbitmq_server-3.8.8，并添加%RABBITMQ_SERVER%\sbin到Path
>
> 命令行开启监控功能rabbitmq-plugins enable rabbitmq_management
>
> 生效需要重启服务，输入**rabbitmq-service**可以查看命令列表
>
> rabbitmq-service stop 停止
>
> rabbitmq-service start 开启
>
> 浏览器访问http://localhost:15672可以打开，说明安装成功
>
> 解决执行命令rabbitmqctl status报错Error的问题：
>
> 拷贝C:\Windows\System32\config\systemprofile\\.erlang.cookie文件覆盖C:\Users\Administrator\\.erlang.cookie文件，
>
> 如果还报错，就修改RabbitMQ服务的登录为当前系统的账户

###	集群配置

> 修改hosts文件，路径C:\Windows\System32\drivers\etc
>
> 都添加如下内容：
>
> 10.250.xxx.xxx rabbitmq-one
> 10.250.xxx.xxx rabbitmq-two
>
> 在浏览器访问上面配置的两个别名测试
>
> 
>
> 分别在两台机器创建集群配置文件 rabbitmq.config
>
> 路径：C:\Users\Administrator\AppData\Roaming\RabbitMQ 内容为（包括最后的 .）：
>
> [{rabbit,[{cluster_nodes, ['rabbit@JTV-ELNDEVDB01', 'rabbit@JTV-ELNPRESS']}]}]. 
>
> 
>
> 配置环境变量，在C:\Users\Administrator\AppData\Roaming\RabbitMQ 路径中，创建rabbitmq-env.conf
>
> NODENAME=rabbit@rabbitmq-one
> NODE_IP_ADDRESS=10.250.xxx.xxx
> NODE_PORT=5672
> RABBITMQ_MNESIA_BASE=C:\Users\Administrator\AppData\Roaming\RabbitMQ\db
> RABBITMQ_LOG_BASE=C:\Users\Administrator\AppData\Roaming\RabbitMQ\log
>
> 另一台
>
> NODENAME=rabbit@rabbitmq-two
> NODE_IP_ADDRESS=10.250.xxx.xxx
> NODE_PORT=5672
> RABBITMQ_MNESIA_BASE=C:\Users\Administrator\AppData\Roaming\RabbitMQ\db
> RABBITMQ_LOG_BASE=C:\Users\Administrator\AppData\Roaming\RabbitMQ\log
>
> 
>
> .erlang.cookie文件统一，拷贝第一台服务器的.erlang.cookie文件覆盖另一台，有两个目录C:\Users\Administrator和C:\Windows\System32\config\systemprofile
>
> 
>
> 重启服务
>
> 第一台
>
> rabbitmqctl stop_app
>
> rabbitmqctl reset
>
> rabbitmqctl start_app
>
> 第二台
>
> rabbitmqctl stop_app
>
> rabbitmqctl reset
>
> rabbitmqctl join_cluster rabbit@rabbitmq-one
> 注意：报错有可能是以下端口的防火墙没有打开
> 4369,5672,15672,25672
> rabbitmqctl start_app
>
> 然后浏览器http://localhost:15672/#/查看两台，在Nodes那里都相互有了
>
> 
>
> 配置为镜像高可用模式：
>
> 镜像模式要依赖policy模块
>
> rabbitmqctl set_policy ha-all "^" '{"ha-mode":"all"}'
>
> 参数意思为：
>
> ha-all：为策略名称，^：为匹配符，只有一个^代表匹配所有，^zlh为匹配名称为zlh的exchanges或者queue。ha-mode：为匹配类型，他分为3种模式：all-所有（所有的queue），exctly-部分。
>
> 也可以在页面Admin的Policies配置





