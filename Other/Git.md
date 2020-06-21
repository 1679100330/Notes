#Git

----------

##Git的基本命令
###Git配置
*	git config --global user.name "John Doe"
*	git config --global user.email "johndoe@example.com"
*	git config --global alias.co checkout
	*	别名
*	git config --global https.proxy http://username:password@ip:port
*	git config --list
	*	列出所有 Git 的配置
*	git config user.name
	*	查看用户名	
###获取帮助
*	git help config
###初始化仓库
*	git init
*	git init --bare
	*	初始化空的仓库
###克隆仓库
*	$ git clone https://github.com/libgit2/libgit2
	*	克隆Github上的仓库
###检查当前文件状态
*	git status
*	git status -s
#
	新添加的未跟踪文件前面有 ?? 标记，新添加到暂存区中的文件前面有 A 标记，修改过的文件前面有 M 标记。 你可能注意到了 M 有两个可以出现的位置，出现在右边的 M 表示该文件被修改了但是还没放入暂存区，出现在靠左边的 M 表示该文件被修改了并放入了暂存区。
###添加到暂存区
*	git add README
*	git add .
	*	添加不在忽略范围的所有未添加和修改的文件
###忽略文件
	在仓库的根目录下创建一个名为 .gitignore 的文件，列出要忽略的文件模式。
###查看已暂存和未暂存的修改
*	git diff
*	git diff --staged
*	git diff --cached
###提交更新
*	git commit
*	git commit -m "描述"
*	git commit -a -m '描述'
	*	跳过暂存区域提交
*	git commit --amend
	*	重新提交
###移除文件
*	git rm PROJECTS.md
	*	从git仓库移除文件并移除工作区的
*	git rm -f PROJECTS.md
	*	如果删除之前修改过并且已经放到暂存区域的话，则必须要用强制删除选项 -f（译注：即 force 的首字母）。
*	git rm --cached README
	*	把文件从 Git 仓库中删除（亦即从暂存区域移除），并保留在当前工作目录中
###移动文件
*	git mv README.md README
#
	其实，运行 git mv 就相当于运行了下面三条命令：
	$ mv README.md README
	$ git rm README.md
	$ git add README
###查看提交历史
*	git log
*	git reflog	
	*	简单查看
*	git log -p -2
	*	常用的选项是 -p，用来显示每次提交的内容差异。 你也可以加上 -2 来仅显示最近两次提交	
*	git log --stat
	*	简略的统计信息，你可以使用 --stat 选项
*	git log --pretty=oneline
	*	按行显示
*	git log --oneline --decorate
	*	查看各个分支当前所指的对象
*	git log --oneline --decorate --graph --all
	*	输出你的提交历史、各个分支的指向以及项目的分支分叉情况
###取消添加到暂存区的文件
*	git reset HEAD CONTRIBUTING.md
###撤消对文件的修改
*	git checkout -- CONTRIBUTING.md
###远程仓库
*	git remote
	*	查看你已经配置的远程仓库服务器
*	git remote -v
	*	显示需要读写远程仓库使用的 Git 保存的简写与其对应的 URL
*	git remote add pb https://github.com/paulboone/ticgit
	*	添加远程仓库
*	git remote rename pb paul
	*	重命名
*	git remote rm paul
	*	删除
###从远程仓库中抓取与拉取
*	git fetch [remote-name]
	*	git fetch origin 会抓取克隆（或上一次抓取）后新推送的所有工作。
###推送到远程仓库
*	git push [remote-name] [branch-name]
###查看远程仓库
*	git remote show origin
###打标签
*	git tag
	*	列出标签
*	git tag -l 'v1.8.5*'
	*	查找特定的标签
*	git tag -a v1.4 -m 'my version 1.4'
	*	附注标签
*	git tag v1.4-lw
	*	轻量标签
*	git show v1.4
	*	查看标签信息与对应的提交信息	
*	git tag -a v1.2 9fceb02
	*	在9fceb02提交上打标签
###推送标签到远程仓库
*	git push origin v1.5
*	git push origin --tags	
	*	把所有不在远程仓库服务器上的标签全部传送到那里
###
*	git checkout -b [branchname] [tagname]
	*	git checkout -b version2 v2.0.0
###分支
*	git branch testing
	*	分支创建
*	git checkout testing
	*	分支切换
*	git checkout -b iss53
	*	新建分支并切换
*	git merge iss53
	*	分支的合并
*	git branch
	*	得到当前所有分支的一个列表
*	git branch -v
	*	查看每一个分支的最后一次提交
*	git branch --merged
	*	--merged 与 --no-merged 这两个有用的选项可以过滤这个列表中已经合并或尚未合并到当前分支的分支
*	git branch -d testing
	*	删除分支