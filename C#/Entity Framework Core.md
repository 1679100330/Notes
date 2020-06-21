#Entity Framework Core

##搭建环境
*	使用Visual Studio Code开发工具
*	在Extensions安装C#
*	在Explorer打开磁盘上的空目录（项目名称）
*	然后打开的目录下Open in Terminal
*	在打开的TERMINAL命令输入栏执行 dotnet new mvc --auth Individual 创建.net core mvc项目
*	在EFCoreDemo.csproj中添加<PackageReference Include="Oracle.EntityFrameworkCore" Version="2.19.50" />
*	执行dotnet restore 还原 .NET 项目中指定的依赖项。
*	在appsettings.json中配置数据库连接字符串
	"ConnectionStrings": {
	  "DefaultConnection": "Data Source=localhost:1521/orcl;User ID=zhangsan;Password=zhangsan"
	},
*	在appsettings.json中配置EFCore日志等级
	"Logging": {
	   "LogLevel": {
	     "Default": "Warning",
	     "Microsoft.EntityFrameworkCore": "Information"
	   }
	 },

##以学生选课作为示例
	学生 n-----n 课程
	班级 1-----n 学生

	//班级
    public class Grade{
        public string Id {set;get;}
        public string Name {set;get;}
        public DateTime CreateDate {set;get;}
        public string CreateBy {set;get;}
        public DateTime ModifyDate {set;get;}
        public string ModifyBy {set;get;}
        public ICollection<Student> Students { get; set; }        
    }

	//学生
    public class Student{
        public string Id {set;get;}
        public string Name {set;get;}
        public DateTime CreateDate {set;get;}
        public string CreateBy {set;get;}
        public DateTime ModifyDate {set;get;}
        public string ModifyBy {set;get;}
        public Grade Grade {set;get;}
        public string GradeId {set;get;}
        public ICollection<R_Student_Course> R_Student_Courses { get; set; }
    }

	//课程
    public class Course{
        public string Id {set;get;}
        public string Name {set;get;}
        public DateTime CreateDate {set;get;}
        public string CreateBy {set;get;}
        public DateTime ModifyDate {set;get;}
        public string ModifyBy {set;get;}
        public ICollection<R_Student_Course> R_Student_Courses { get; set; }
    }

	//学生课程关系
    [Table("R_SC")]
    public class R_Student_Course{
        public string Id {set;get;}
        public Student Student {set;get;}
        public string StudentId {set;get;}
        public Course Course {set;get;}
        public string CourseId {set;get;}
    }

	public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Grade> Grade { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Course> Course { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            //一对多关系模型
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Grade)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.GradeId);

			//多对多使用中间关系（两个一对多的组合）			
            modelBuilder.Entity<R_Student_Course>()
                .HasOne(r => r.Student)
                .WithMany(s => s.R_Student_Courses)
                .HasForeignKey(r => r.StudentId);            
            modelBuilder.Entity<R_Student_Course>()
                .HasOne(r => r.Course)
                .WithMany(s => s.R_Student_Courses)
                .HasForeignKey(r => r.CourseId);

        }
    }

	public interface IRepository
    {
        IEnumerable<Grade> Grades { get; }
        int AddGrade(Grade grade);
        Grade GetGrade(string id);
        int UpdateGrade(Grade grade);
        void Test();
    }

	public class ApplicationRepository : IRepository
    {
        private ApplicationDbContext context;

        public ApplicationRepository(ApplicationDbContext ctx) => context = ctx;

        public IEnumerable<Grade> Grades => context.Grade.ToArray();

        public int AddGrade(Grade grade)
        {
            context.Grade.Add(grade);
            int result = context.SaveChanges();
            return result;
        }

        public Grade GetGrade(string id) => context.Grade.Find(id);        

        public int UpdateGrade(Grade grade)
        {
            context.Grade.Update(grade);
            return context.SaveChanges();
        }

        public void Test(){
            Dictionary<string, Grade> data = new Grade[]{
                new Grade(){Id="9d7184ef104145058cc3629e8506a507"}
                , new Grade(){Id="339385aea6ba4c109cc4db638f9aa0e5"}}
                .ToDictionary(g => g.Id);
            IEnumerable<Grade> grades =
                context.Grade.Where(p => data.Keys.Contains(p.Id));
        }
    }

	//Startup.cs
	//获取配置文件的数据库连接字符串
    string connStr = Configuration["ConnectionStrings:DefaultConnection"];
    //string connStr = Configuration.GetConnectionString("DefaultConnection");

    //配置DbContext，UseOracleSQLCompatibility：11表示使用11g版本的Oracle数据库
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseOracle(connStr, o=>o.UseOracleSQLCompatibility("11")));

	//配置仓储
	//services.AddSingleton<IRepository, ApplicationRepository>();
	services.AddTransient<IRepository, ApplicationRepository>();

	public class HomeController : Controller
    {    
        private IRepository repository;

        public HomeController(IRepository repo) => repository = repo;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Test()
        {
            Grade grade = new Grade();
            grade.Id = Guid.NewGuid().ToString("N");
            grade.Name = "班级一";
            grade.CreateBy = "Test";
            grade.CreateDate = DateTime.Now;
            // context.Grade.Add(grade);
            // int result = context.SaveChanges();

            // var grades = context.Grade.OrderByDescending(g => g.Id);

            // grades = context.Grade.Where(g => g.Name == "班级一")
            //     .OrderBy(g => g.Id);

            repository.AddGrade(grade);

            var grades = repository.Grades;

            grade = repository.GetGrade("9d7184ef104145058cc3629e8506a507");
            grade.Name = "班级二";
            repository.UpdateGrade(grade);

            repository.Test();

            return Content("ok：");
        }
                
    }

##查询
> IN

	Dictionary<string, Grade> data = new Grade[]{
        new Grade(){Id="9d7184ef104145058cc3629e8506a507"}
        , new Grade(){Id="339385aea6ba4c109cc4db638f9aa0e5"}}
        .ToDictionary(g => g.Id);
    IEnumerable<Grade> grades =
        context.Grade.Where(p => data.Keys.Contains(p.Id));
> 分页

	var grades = context.Grade.Skip(0).Take(10);

> 指定要查询的属性

	var students = context.Student.Select(s => new{
        Id = s.Id, 
        Name = s.Name,
        Grade = new {Id = s.Grade.Id, Name = s.Grade.Name}
    });


##实体
	Grade grade = context.Grade.Find("9d7184ef104145058cc3629e8506a507");
    grade.Name = "班级三";
    EntityEntry entry = context.Entry(grade);
    //实体的状态
    var state = entry.State;
    //原始值
    var originalName = entry.OriginalValues["Name"];
    //当前值
    var currentName = entry.CurrentValues["Name"];

##其它
	//设置超时时间
	context.Database.SetCommandTimeout(System.TimeSpan.FromMinutes(10));
	//开启事务
	context.Database.BeginTransaction();
	//执行SQL命令
	context.Database.ExecuteSqlCommand("DELETE FROM Orders");
	//提交事务	
	context.Database.CommitTransaction();

##Fluent API	
	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		//添加索引
		modelBuilder.Entity<Grade>().HasIndex(g => g.Name);	

		//一对多关系模型
        modelBuilder.Entity<Student>()
            .HasOne(s => s.Grade)
            .WithMany(g => g.Students)
            .HasForeignKey(s => s.GradeId);

		//多对多使用中间关系（两个一对多的组合）			
        modelBuilder.Entity<R_Student_Course>()
            .HasOne(r => r.Student)
            .WithMany(s => s.R_Student_Courses)
            .HasForeignKey(r => r.StudentId);            
        modelBuilder.Entity<R_Student_Course>()
            .HasOne(r => r.Course)
            .WithMany(s => s.R_Student_Courses)
            .HasForeignKey(r => r.CourseId);
	}

##数据迁移
> <timestamp>_Initial.cs

	这是初始类的一部分，它将第一次迁移应用到数据库。它包含创建数据库模式的指令

> <timestamp>_Initial.Designer.cs

	这是初始类的一部分，它将第一次迁移应用到数据库。它包含为模型对象创建的指令

> EFDatabaseContextModelSnapshot.cs

	该类包含迁移中使用的实体类的描述，用于检测创建进一步迁移的更改

> Up 和 Down 方法

	Up()	此方法包含将数据库升级为存储实体数据的语句。
	Down()	这个方法包含将数据库降级到原始状态的语句

> 命令
	dotnet ef migrations add Initial 创建新的迁移 

	dotnet ef database update 将迁移应用到数据库 
	dotnet ef database update [迁移名称] 回滚到指定迁移 
	dotnet ef database update 0 重置数据库 

	dotnet ef migrations remove 删除未应用到数据库的迁移 
	dotnet ef migrations remove --force 删除所有迁移，包括应用到数据库的迁移 

	dotnet ef database drop 删除数据库（删除用户下所有的对象）
	dotnet ef database drop --force 不提示  

	dotnet ef migrations list 列出所有迁移 

	dotnet ef migrations script [迁移名称] 查看迁移的脚本 
	dotnet ef migrations script Initial AddColorProperty 显示两个迁移更新数据库所需的语句 

	dotnet ef dbcontext list 获取上下文列表
	dotnet ef ... --context ApplicationDbContext 多个上下文时，需要指定
