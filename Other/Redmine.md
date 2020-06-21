#Redmine的搭建
##下载相关软件
###1、下载Redmine
*	地址：http://www.redmine.org/releases/
*	版本：redmine-3.2.9.zip
###2、下载Ruby
*	地址：https://rubyinstaller.org/downloads/
*	版本：rubyinstaller-2.1.9-x64.exe
###3.下载DevKit
*	地址：https://rubyinstaller.org/downloads/
*	版本：DevKit-mingw64-64-4.7.2-20130224-1432-sfx.exe
###4.下载MySQL
*	地址：https://dev.mysql.com/downloads/mysql/5.5.html#downloads
*	版本：mysql-5.5.62-winx64.msi
###5.下载ImageMagick
*	地址：
	*	http://www.imagemagick.org/script/binary-releases.php#windows
	*	https://sourceforge.net/projects/imagemagick/files/im6-exes/
*	版本：ImageMagick-6.9.9-37-Q16-HDRI-x64-dll.exe	

##安装
	1、Redmine解压到指定目录下，例如D:\ProjectMangement
	2、安装Ruby，点击rubyinstaller-2.1.9-x64.exe，指定目录到D:\ProjectMangement（方便管理）
	3、安装DevKit：
		一、运行，解压到一个目录，例如：D:\ProjectMangement\DevKit
		二、打开命令行，切换到这个目录
		三、执行命令：ruby dk.rb init
		四、修改config.yml文件
			- D:/ProjectMangement/ruby-2.1.9
			# - C:/ruby192dev
		五、执行命令：ruby dk.rb install
	4、安装MySQL，默认下一步，编码选择utf-8，配置root的密码，完成后新建数据库
	5、安装ImageMagick
		Run the setup package. In the Select Additional Tasks page of the wizard, make sure that both Add application directory to your system path and Install development headers and libraries for C and C++ options are checked
		
	6、安装Redmine		
		切到Redmine根目录
		进入命令行
		set CPATH=C:\Program Files (x86)\ImageMagick-6.7.9-Q16\include
		set LIBRARY_PATH=C:\Program Files (x86)\ImageMagick-6.7.9-Q16\lib
		bundle install --without development test
		gem install mysql2 -v '0.4.6' -- '--with-mysql-lib="C:\ProjectMangement\MySQL Server 5.5\lib" --with-mysql-include="C:\ProjectMangement\MySQL Server 5.5\include"'
		bundle exec rake generate_secret_token
		bundle exec rake db:migrate RAILS_ENV=production
		bundle exec rake redmine:load_default_data RAILS_ENV=production REDMINE_LANG=zh
		bundle exec rails server webrick -e production -b 0.0.0.0 -p 3000

##安装插件
	bundle install --no-deployment
	
	1、安装所有插件，bundle exec rake redmine:plugins RAILS_ENV=production

	2、安装指定插件，bundle exec rake redmine:plugins NAME=redmine_knowledgebase RAILS_ENV=production

##配置
	1、安装可视化的MySQL工具，创建数据库redmine、redmine_development、redmine_test，编码utf-8
	2、配置redmine-3.2.9\config\database.yml文件，参照database.yml.example

##设置代理
	在命令行下输入：set http_proxy=https://用户名:密码@IP:PORT

##gem的使用
	gem help 
	gem help commands 
	gem sources -h 
	gem sources -l 查看源
	gem sources -r https://rubygem.org/ 移除源
	gem sources -a http://ruby.taobao.org 添加源
	
	gem install bundler -v '1.17.3' --http-proxy https://用户名:密码@IP:PORT
	gem list 查看已安装

##bundle的使用
	bundle install --without development test	

##安装出现的问题
	解决不能使用https协议，缺少SSL证书。下载https://curl.haxx.se/ca/cacert.pem文件，
	在环境变量添加SSL_CERT_FILE，变量值写下载文件的路径：D:\ProjectMangement\cacert.pem
	
##bitnami-Redmine一键安装3.2.0版本
	https://downloads.bitnami.com/files/stacks/redmine/3.2.0-0/bitnami-redmine-3.2.0-0-windows-installer.exe	
	
