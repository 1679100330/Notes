# systemctl服务管理

* 服务单位（unit）
* .service：一般服务类型（service unit）



* 示例

```
vim /ect/systemd/system/my_service.service

[Unit]
Description=My Java Service
[Service]
User=ubuntu
WorkingDirectory=/home/ubuntu/server
ExecStart=/usr/bin/java -jar /home/ubuntu/server/main.jar
SuccessExitStatus=143
TimeoutStopSec=10
Restart=on-failure
RestartSec=5
[Install]
WantedBy=multi-user.target

更新、运行、启动服务配置
systemctl daemon-reload
systemctl enable my_service.service
systemctl start my_service.service
```

* 说明

```
Description=当前服务的简单介绍
Documentation: 使用文档的位置 
After: 应该在那些服务之后启动
Before: 应该在那些服务之前启动

ExecStart字段：定义启动进程时执行的命令
ExecReload字段：重启服务时执行的命令
ExecStop字段：停止服务时执行的命令
ExecStartPre字段：启动服务之前执行的命令
ExecStartPost字段：启动服务之后执行的命令
ExecStopPost字段：停止服务之后执行的命令

一般来说，常用的 Target 有两个：
multi-user.target：表示多用户命令行状态；
graphical.target：表示图形用户状态，它依赖于multi-user.target
```



