##	ORACLE

### 任务调度

* 创建

```plsql
BEGIN
  DBMS_SCHEDULER.CREATE_JOB(
      JOB_NAME          => 'CHANGE_STATE',          --任务名称
      JOB_TYPE          => 'STORED_PROCEDURE',      --任务类型
      JOB_ACTION        => 'TRU_ACCEPT_ABNORMAL_STATE',--任务执行的程序名称
      START_DATE        => '',                      --开始执行时间
      REPEAT_INTERVAL   => 'FREQ=MINUTELY;INTERVAL=10',--执行频率：每分钟，间隔10秒
      END_DATE          => NULL,                    --结束时间
      ENABLED           => TRUE,                    --任务创建完毕后是否自动激活
      AUTO_DROP         => FALSE,                   --自动删除
      COMMENTS          => '');               		--备注，任务说明
END;
```

* 查询

```plsql
select * from user_scheduler_jobs

select * from user_scheduler_job_log

select * from user_scheduler_job_run_details

select * from user_scheduler_running_jobs
```

* 修改

```plsql
call dbms_scheduler.set_attribute('MY_JOB', 'REPEAT_INTERVAL', 'FREQ=MINUTELY;INTERVAL=2');--修改属性

call dbms_scheduler.enable('my_job1');--激活

call dbms_scheduler.disable('my_job1');--禁用

call dbms_scheduler.drop_job('my_job1');--删除

call dbms_scheduler.run_job('my_job1');--运行

call dbms_scheduler.stop_job('my_job1');--停止
```



