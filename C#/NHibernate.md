#NHibernate的笔记一

----------

##引用的包
	NHibernate.5.1.3
	MySql.Data.8.0.12
##例子
##配置文件
#####MySql.cfg.xml
	<?xml version="1.0" encoding="utf-8"?>
	
	<hibernate-configuration  xmlns="urn:nhibernate-configuration-2.2" >
		<session-factory name="NHibernate.Test">
			<property name="connection.driver_class">NHibernate.Driver.MySqlDataDriver</property>
			<property name="connection.connection_string">Database=demo;Data Source=127.0.0.1;User Id=root;Password=root;pooling=false;CharSet=utf8;port=3306;SslMode = none;</property>
			<property name="dialect">NHibernate.Dialect.MySQL5Dialect</property>
			
			<property name="show_sql">true</property>

			<mapping file="Config/Product.hbm.xml" />
			<mapping file="Config/Customer.hbm.xml" />
			<mapping file="Config/MyOrder.hbm.xml" />
			<mapping file="Config/Student.hbm.xml" />
			<mapping file="Config/Course.hbm.xml" />
		</session-factory>
	</hibernate-configuration>
#####Oracle.cfg.xml
	<?xml version="1.0" encoding="utf-8"?>
	
	<hibernate-configuration  xmlns="urn:nhibernate-configuration-2.2" >
		<session-factory name="NHibernate.Test">
			<property name="connection.driver_class">NHibernate.Driver.OracleClientDriver</property>
			<property name="connection.connection_string">
				Data Source=127.0.0.1:1521/TEST;User ID=TEST;Password=TEST
			</property>
			<property name="show_sql">true</property>
			<property name="dialect">NHibernate.Dialect.Oracle10gDialect</property>
			<property name="oracle.use_n_prefixed_types_for_unicode">false</property>
			
			<mapping file="Config/Product.hbm.xml" />
		</session-factory>
	</hibernate-configuration>
#####Product.hbm.xml
	<?xml version="1.0" encoding="utf-8" ?>
	<hibernate-mapping xmlns="urn:nhibernate-mapping-2.2"
	                   assembly="NHibernateDemo"
	                   namespace="NHibernateDemo.Model">
	
	  <class name="Product" lazy="false">
	    <id name="Id">
	      <generator class="guid" />
	    </id>
	    <property name="Name" />
	  </class>
	
	</hibernate-mapping>
##表
	CREATE TABLE `product` (
		`id` VARCHAR(50) NOT NULL,
		`name` VARCHAR(20) NULL DEFAULT NULL,
		PRIMARY KEY (`id`)
	)
	COLLATE='utf8_general_ci'
	ENGINE=InnoDB
	;
##实体类
	using System;
	
	namespace NHibernateDemo.Model
	{
	    public class Product
	    {
	        public Guid Id { get; set; }
	        public string Name { get; set; }
	    }
	}
##Main
	using NHibernate;
	using NHibernate.Cfg;
	using NHibernateDemo.Model;
	using System;
	
	namespace NHibernateDemo
	{
	    class Program
	    {
	        static void Main(string[] args)
	        {            
	            var cfg = new Configuration();
	            cfg.Configure("Config/MySql.cfg.xml");
	            cfg.AddFile("Config/Product.hbm.xml");
	            ISessionFactory sessionFactory = cfg.BuildSessionFactory();
	            /*
	            //添加
	            using (ISession session = sessionFactory.OpenSession()) {
	                using (ITransaction transaction = session.BeginTransaction()) {
	                    Product product = new Product();
	                    product.Id = Guid.NewGuid();
	                    product.Name = "張三";
	                    session.Save(product);
	                    transaction.Commit();
	                }
	            }
	            //更新
	            using (ISession session = sessionFactory.OpenSession())
	            {
	                using (ITransaction transaction = session.BeginTransaction())
	                {
	                    Product product = new Product();
	                    product.Id = Guid.Parse("16587923-c95c-4bd5-bfb2-8169e506941f");
	                    product.Name = "李四";
	                    session.Update(product);
	                    transaction.Commit();
	                }
	            }           
	            //刪除 
	            using (ISession session = sessionFactory.OpenSession())
	            {
	                using (ITransaction transaction = session.BeginTransaction())
	                {
	                    Product product = new Product();
	                    product.Id = Guid.Parse("16587923-c95c-4bd5-bfb2-8169e506941f");
	                    session.Delete(product);
	                    transaction.Commit();
	                }
	            }
	            //查詢
	            using (ISession session = sessionFactory.OpenSession())
	            {
	                Guid Id = Guid.Parse("6d35f648-5abd-40fc-a4e8-15cf9d24f4c5");
	                Product product = session.Get<Product>(Id);
	            }				
	            */
	            sessionFactory.Dispose();
	                                    
	            Console.WriteLine("結束");
	            Console.ReadKey();
	        }
	    }
	}

##加載hbm.xml文件的方式
	1.在MySql.cfg.xml配置文件的節點</session-factory>中加載
		<mapping file="Config/Product.hbm.xml" />
	2.var cfg = new Configuration();
      cfg.Configure("Config/MySql.cfg.xml");
	  //通過AddFile加載
      cfg.AddFile("Config/Product.hbm.xml");
	  //通過AddXmlFile加載
      cfg.AddXmlFile("Config/Product.hbm.xml");	
##嵌入的資源
	XML文件的默認生成操作爲"內容(Content)",可以右鍵屬性Build Action下拉選擇"嵌入的資源(Embedded Resource)"
##查詢數據
	using (ISession session = sessionFactory.OpenSession())
	{
	    Product product = session
	        .CreateCriteria(typeof(Product))
	        .Add(Restrictions.Eq("Name", "李四"))
	        .UniqueResult<Product>();
	}  
          
	using (ISession session = sessionFactory.OpenSession())
	{
	    IList<Product> products = session
	        .CreateCriteria(typeof(Product))
	        .Add(Restrictions.Eq("Name", "張三"))
	        .List<Product>();
	}

	using (ISession session = sessionFactory.OpenSession())
    {
        var products = session
            .Query<Product>()
            .Where(c => c.Name == "張三")
            .ToList();
    }
##查詢語言(HQL)
	注意:HQL語句的表名和字段必須和hbm.xml配置的一致
	//from 子句
	using (ISession session = sessionFactory.OpenSession())
	{
	    IList<Product> products = session.CreateQuery("from Product")
	        .List<Product>();
	}

	//select 子句
	using (ISession session = sessionFactory.OpenSession())
	{
	    IList<Guid> ids = session.CreateQuery("select id from Product")
	        .List<Guid>();
	}
	using (ISession session = sessionFactory.OpenSession())
	{
	    IList<object[]> objs = session.CreateQuery("select Name,Id from Product")
	        .List<object[]>();
	}
	//where 子句
    using (ISession session = sessionFactory.OpenSession())
    {
        IList<Product> products = session.CreateQuery("from Product where Name='張三'")
            .List<Product>();
    }
	//order by 子句
    using (ISession session = sessionFactory.OpenSession())
    {
        IList<Product> products = session.CreateQuery("from Product order by Id")
            .List<Product>();
    }
	//group by 子句
    using (ISession session = sessionFactory.OpenSession())
    {
        IList<object[]> objs = session.CreateQuery("select Name,Count(Id) from Product group by Name")
            .List<object[]>();
    }
	//普通方式
    using (ISession session = sessionFactory.OpenSession())
    {
        string name = "王二";
        IList<Product> products = session.CreateQuery("from Product where Name='"+name+"'")
            .List<Product>();
    }
	//位置型參數
    using (ISession session = sessionFactory.OpenSession())
    {
        IList<Product> products = session.CreateQuery("from Product where Name=?")
            .SetString(0,"王二")
            .List<Product>();
    }
	//命名型參數
    using (ISession session = sessionFactory.OpenSession())
    {
        IList<Product> products = session.CreateQuery("from Product where Name=:name")
            .SetString("name","王二")
            .List<Product>();
    }
##條件查詢(Criteria Query)
	//查詢指定最大條數
	using (ISession session = sessionFactory.OpenSession())
	{
	    IList<Product> products = session
	        .CreateCriteria(typeof(Product))
	        .SetMaxResults(5).List<Product>();
	}
	//結果集限制
    using (ISession session = sessionFactory.OpenSession())
    {
        IList<Product> products = session
            .CreateCriteria(typeof(Product))
            .Add(Restrictions.Like("Name","張%"))
            .Add(Restrictions.In("Id",new object[] { "5f7fafa6-6e6b-4862-8e7c-9330ca152927", "5fb4120d-fc80-44cc-b56b-cd1ebb9b32ba" }))
            .List<Product>();
    }
	//結果集排序
    using (ISession session = sessionFactory.OpenSession())
    {
        IList<Product> products = session
            .CreateCriteria(typeof(Product))
            .AddOrder(new Order("Id",false))
            .List<Product>();
    }
	//根據示例查詢
    using (ISession session = sessionFactory.OpenSession())
    {
        Product product = new Product() { Name="王二" };                
        IList<Product> products = session
            .CreateCriteria(typeof(Product))
            .Add(Example.Create(product))
            .List<Product>();
    }
	using (ISession session = sessionFactory.OpenSession())
    {
        Product product = new Product() { Name="王二" };
        Example example = Example.Create(product)
            .IgnoreCase()
            .EnableLike()
            .SetEscapeCharacter('&');
        IList<Product> products = session
            .CreateCriteria(typeof(Product))
            .Add(example)
            .List<Product>();
    }
##對象操作
	//在Save,Delete,Update後需要Flush才會把數據保存到數據庫
	using (ISession session = sessionFactory.OpenSession())
    {
        Product product = new Product();
        product.Name = "小明";
        Guid newId = (Guid)session.Save(product);
        session.Flush();
        Product newProduct = session.Get<Product>(newId);
    }
	//對象如果存在就更新,不存在插入
	using (ISession session = sessionFactory.OpenSession())
    {
        Product product = new Product();
        //product.Id = Guid.Parse("40f98711-12e6-401c-b8c7-b7cef0bea5b9");
        product.Name = "小明";
        session.SaveOrUpdate(product);
        session.Flush();                
    }
##事務(Transactions)
	using (ITransaction transaction = session.BeginTransaction())
	{
	    try
	    {
	        transaction.Commit();
	    }
	    catch (Exception ex)
	    {
	        transaction.Rollback();
	        throw ex;
	    }
	}
##並發控制--乐观并发控制	
	public class Product
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }

        /** 版本控制 */
        public virtual int Version { get; set; }
    }
	配置version
	<class name="Product" lazy="false">
		<id name="Id">
		  <generator class="guid" />
		</id>
		<!-- XML格式的version属性映射必须立即放在标示符属性映射之后 -->
		<version name="Version" column="Version" type="integer" unsaved-value="0" />
		<property name="Name" />	
	</class>
	添加字段
	`version` INT(11) NULL DEFAULT NULL,
##組件
	新建類
	namespace NHibernateDemo.Model
	{
	    public class FullName
	    {
	        public virtual string FirstName { get; set; }
	        public virtual string LastName { get; set; }
	        public virtual string Name {
	            get {
	                return FirstName + " " + LastName;
	            }
	        }
	    }
	}

	添加屬性public virtual FullName FNmae { get; set; }
	using System;
	
	namespace NHibernateDemo.Model
	{
	    public class Product
	    {
	        public virtual Guid Id { get; set; }
	        public virtual string Name { get; set; }
	        public virtual FullName FNmae { get; set; }
	
	        /** 版本控制 */
	        public virtual int Version { get; set; }
	    }
	}

	在Product.hbm.xml添加組件配置
	<?xml version="1.0" encoding="utf-8" ?>
	<hibernate-mapping xmlns="urn:nhibernate-mapping-2.2"
	                   assembly="NHibernateDemo"
	                   namespace="NHibernateDemo.Model">
	
	  <class name="Product" lazy="false">
	    <id name="Id">
	      <generator class="guid" />
	    </id>
		<!-- XML格式的version属性映射必须立即放在标示符属性映射之后 -->
		<version name="Version" column="Version" type="integer" unsaved-value="0" />
		<component name="FNmae" class="FullName">
			<property name="FirstName" />	
			<property name="LastName" />	
		</component>
	    <property name="Name" />	
	  </class>  
	
	</hibernate-mapping>
	
	保存對象
	using (ISession session = sessionFactory.OpenSession())
    {
        using (ITransaction transaction = session.BeginTransaction())
        {
            Product product = new Product();
            product.Name = "王二";
            product.FNmae = new FullName() { FirstName = "小", LastName = "紅" };
            session.Save(product);
            transaction.Commit();
        }
    }	
##一對多
######創建Customer
	namespace NHibernateDemo.Model
	{
	    public class Customer
	    {
	        public virtual Guid Id { get; set; }
	        public virtual string Name { get; set; }
	        public virtual ISet<MyOrder> Orders { get; set; }
	    }
	}
######創建MyOrder
	namespace NHibernateDemo.Model
	{
	    public class MyOrder
	    {
	        public virtual Guid Id { get; set; }
	        public virtual string Describle { get; set; }
	        public virtual Customer Customer { get; set; }
	    }
	}
######創建Customer.hbm.xml
	<?xml version="1.0" encoding="utf-8" ?>
	<hibernate-mapping xmlns="urn:nhibernate-mapping-2.2"
	                   assembly="NHibernateDemo"
	                   namespace="NHibernateDemo.Model">
	
	  <class name="Customer" lazy="false">
	    <id name="Id">
	      <generator class="guid" />
	    </id>	
	    <property name="Name" />	
		<!--一對多關係-->
		<set name="Orders" inverse="true">
			<key column="Customer" foreign-key="Customer"/>
			<one-to-many class="MyOrder"/>
		</set>
	  </class>  
	
	</hibernate-mapping>
######創建MyOrder.hbm.xml
	<?xml version="1.0" encoding="utf-8" ?>
	<hibernate-mapping xmlns="urn:nhibernate-mapping-2.2"
	                   assembly="NHibernateDemo"
	                   namespace="NHibernateDemo.Model">
	
	  <class name="MyOrder" lazy="false">
	    <id name="Id">
	      <generator class="guid" />
	    </id>	
	    <property name="Describle" />
		<many-to-one name="Customer" class="Customer" foreign-key="Customer"/>	
	  </class>  
	
	</hibernate-mapping>
######添加到MySql.cfg.xml
	<mapping file="Config/Customer.hbm.xml" />
	<mapping file="Config/MyOrder.hbm.xml" />
######創建表
	CREATE TABLE `customer` (
		`id` VARCHAR(50) NOT NULL,
		`name` VARCHAR(20) NULL DEFAULT NULL,
		PRIMARY KEY (`id`)
	)
	COLLATE='utf8_general_ci'
	ENGINE=InnoDB
	;

	CREATE TABLE `myorder` (
		`id` VARCHAR(50) NOT NULL,
		`describle` VARCHAR(50) NULL DEFAULT NULL,
		`customer` VARCHAR(50) NULL DEFAULT NULL,
		PRIMARY KEY (`id`)
	)
	COLLATE='utf8_general_ci'
	ENGINE=InnoDB
	;
######代碼
	//添加Customer
	using (ISession session = sessionFactory.OpenSession())
    {
        using (ITransaction transaction = session.BeginTransaction())
        {
            Customer customer = new Customer() { Name = "李白" };
            session.Save(customer);
            transaction.Commit();
        }
    }
	//添加MyOrder
	using (ISession session = sessionFactory.OpenSession())
    {
        using (ITransaction transaction = session.BeginTransaction())
        {                    
            Customer customer = session.Get<Customer>(Guid.Parse("d57708c9-2620-4514-95b5-7f1ae6fb59ec"));
            MyOrder order = new MyOrder();
            order.Describle = "訂單";
            order.Customer = customer;
            session.Save(order);
            transaction.Commit();
        }
    }
	//刪除Customer
	using (ISession session = sessionFactory.OpenSession())
    {
        using (ITransaction transaction = session.BeginTransaction())
        {
            Customer customer = session.Get<Customer>(Guid.Parse("02247735-5b45-4f86-aa9e-1a67f10fb6a1"));
            session.Delete(customer);
            transaction.Commit();
        }
    }
	//根據條件查詢
	using (ISession session = sessionFactory.OpenSession())
    {
        IList<Customer> customers = session.CreateCriteria(typeof(Customer))
            .CreateCriteria("Orders")
            .Add(Restrictions.Eq("Describle", "訂單"))
            .List<Customer>();
    }
	//去重
	using (ISession session = sessionFactory.OpenSession())
    {
        IList<Customer> customers = session.CreateCriteria(typeof(Customer))
            .CreateCriteria("Orders")
            .Add(Restrictions.Eq("Describle", "訂單"))
            .SetResultTransformer(new NHibernate.Transform.DistinctRootEntityResultTransformer())
            .List<Customer>();
    }
	//投影--去重
	using (ISession session = sessionFactory.OpenSession())
    {
        IList<Guid> ids = session.CreateCriteria(typeof(Customer))                    
            .SetProjection(Projections.Distinct(Projections.ProjectionList()
            .Add(Projections.Property("Id"))))
            .CreateCriteria("Orders")
            .Add(Restrictions.Eq("Describle", "訂單"))
            .List<Guid>();
        IList<Customer> customers = session.CreateCriteria(typeof(Customer))
            .Add(Restrictions.In("Id", ids.ToArray<Guid>()))
            .List<Customer>();
    }
##多對多
######創建Student
	namespace NHibernateDemo.Model
	{
	    public class Student
	    {
	        public virtual Guid Id { get; set; }
	        public virtual string Name { get; set; }
	        public virtual int Age { get; set; }
	        public virtual IList<Course> Courses { get; set; }
	    }
	}
######創建Course
namespace NHibernateDemo.Model
{
    public class Course
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual IList<Student> Students { get; set; }
    }
}
######創建Student.hbm.xml
	<?xml version="1.0" encoding="utf-8" ?>
	<hibernate-mapping xmlns="urn:nhibernate-mapping-2.2"
	                   assembly="NHibernateDemo"
	                   namespace="NHibernateDemo.Model">
	
	  <class name="Student" lazy="false">
	    <id name="Id">
	      <generator class="guid" />
	    </id>	
	    <property name="Name" />	
		<property name="Age" />
		<!--多對多關係-->
		<bag name="Courses" generic="true" table="r_student_course">
			<key column="student_id" foreign-key="student_id"/>
			<many-to-many column="course_id" class="Course" foreign-key="course_id"/>
		</bag>
	  </class>  
	
	</hibernate-mapping>
######創建Course.hbm.xml
	<?xml version="1.0" encoding="utf-8" ?>
	<hibernate-mapping xmlns="urn:nhibernate-mapping-2.2"
	                   assembly="NHibernateDemo"
	                   namespace="NHibernateDemo.Model">
	
	  <class name="Course" lazy="false">
	    <id name="Id">
	      <generator class="guid" />
	    </id>	
	    <property name="Name" />	
		<property name="CreateDate" />
		<!--多對多關係-->
		<bag name="Students" generic="true" table="r_student_course">
			<key column="course_id" foreign-key="course_id"/>
			<many-to-many column="student_id" class="Student" foreign-key="student_id"/>
		</bag>
	  </class>  
	
	</hibernate-mapping>
######在MySql.cfg.xml添加
	<mapping file="Config/Student.hbm.xml" />
	<mapping file="Config/Course.hbm.xml" />
######創建表
	CREATE TABLE `student` (
		`id` VARCHAR(50) NOT NULL,
		`name` VARCHAR(20) NULL DEFAULT NULL,
		`age` INT(11) NULL DEFAULT NULL,
		PRIMARY KEY (`id`)
	)
	COLLATE='utf8_general_ci'
	ENGINE=InnoDB
	;

	CREATE TABLE `course` (
		`id` VARCHAR(50) NOT NULL,
		`name` VARCHAR(20) NULL DEFAULT NULL,
		`createDate` DATETIME NULL DEFAULT NULL,
		PRIMARY KEY (`id`)
	)
	COLLATE='utf8_general_ci'
	ENGINE=InnoDB
	;

	CREATE TABLE `r_student_course` (
		`student_id` VARCHAR(50) NULL DEFAULT NULL,
		`course_id` VARCHAR(50) NULL DEFAULT NULL
	)
	COLLATE='utf8_general_ci'
	ENGINE=InnoDB
	;
######代碼
	//學生選擇課程
	using (ISession session = sessionFactory.OpenSession())
    {
        using (ITransaction transaction = session.BeginTransaction())
        {
            Student stu = session.Get<Student>(Guid.Parse("d7559b0c-ee75-4679-9305-f8c7b592c0eb"));
            Course course = session.Get<Course>(Guid.Parse("1b66049e-46ee-4d49-bd0a-ff390ec2cc3f"));
            stu.Courses.Add(course);
            session.Save(stu);
            transaction.Commit();
        }
    }
	//查詢選了語文的學生
	using (ISession session = sessionFactory.OpenSession())
    {
        IList<Student> students = session.CreateCriteria(typeof(Student))
            .CreateCriteria("Courses")
            .Add(Restrictions.Eq("Name", "語文"))
            .List<Student>();

    }
	//根據ID刪除學生
	using (ISession session = sessionFactory.OpenSession())
    {
        using (ITransaction transaction = session.BeginTransaction())
        {
            Student stu = session.Get<Student>(Guid.Parse("82906299-b90f-4d27-be7f-1feb72342869"));
            session.Delete(stu);
            transaction.Commit();
        }
    }
	//查詢選擇了英語的學生
	using (ISession session = sessionFactory.OpenSession())
    {
        IList<Student> students = session.CreateQuery("from Student a"+
            " left outer join fetch a.Courses b" +
            " where b.Name = :Name")
            .SetString("Name", "英語")
            .List<Student>();
        
    }
##懶加載
	Student student = null;
    using (ISession session = sessionFactory.OpenSession())
    {
        student = session.Get<Student>(Guid.Parse("d7559b0c-ee75-4679-9305-f8c7b592c0eb"));
        //加載懶加載的數據
        NHibernateUtil.Initialize(student.Courses);
    }
##對象狀態
*	瞬時態
*	持久態
*	託管態
#
	//查看對象是否被Session管理
	bool flag = session.Contains(course);
	session.Evict(course);
##一級緩存
	實例緩存在Session中,每個Session是獨立的
	using (ISession session = sessionFactory.OpenSession())
    {
        Student stu1 = session.Get<Student>(Guid.Parse("07c077e5-6e70-49e9-bcff-9468a5150dba"));
        Student stu2 = session.Get<Student>(Guid.Parse("07c077e5-6e70-49e9-bcff-9468a5150dba"));
        Console.WriteLine(stu1.GetHashCode());
        Console.WriteLine(stu2.GetHashCode());
        Console.WriteLine(stu1 == stu2);                
    }
##ISession.Get和ISession.Load
	ISession.Get()方法立即把對象實例保存到緩存中,使用ISession.Load()方法當你需要使用的時候再訪問數據庫把這個實例保存到緩存中
	using (ISession session = sessionFactory.OpenSession())
	{
	    Student student = session.Load<Student>(Guid.Parse("07c077e5-6e70-49e9-bcff-9468a5150dba"));
	    Console.WriteLine(student.Id);
	    Console.WriteLine(student.Name);
	}
##二級緩存
	NHibernate二級緩存由ISessionFactory創建,可以被所有的ISession共享,當開啓二級緩存後,使用ISession查詢數據時,NHibernate首先從內置緩存(一級緩存)中查找是否存在,不存在才從二級緩存查找

#NHibernate的笔记二

----------
	
##配置
	<?xml version="1.0" encoding="utf-8"?>
	<configuration>
	  <configSections>
	    <section name="hibernate-configuration" type="NHibernate.Cfg.ConfigurationSectionHandler, NHibernate" />
	  </configSections>
	
	  <hibernate-configuration xmlns="urn:nhibernate-configuration-2.2">
	    <session-factory>
	      <property name="dialect">NHibernate.Dialect.MySQL5Dialect</property>
	      <property name="connection.connection_string">Database=demo;Data Source=127.0.0.1;User Id=root;Password=root;pooling=false;CharSet=utf8;port=3306;SslMode = none;</property>
	      <property name="show_sql">true</property>
	      <mapping assembly="NHibernateDemo" />
	    </session-factory>
	  </hibernate-configuration>
	
	  <startup>
	    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.6.1"/>
	  </startup>
	</configuration>
##配置Cat.hbm.xml,並且設置爲 嵌入的資源(Embedded Resource)
	<?xml version="1.0" encoding="utf-8" ?>
	<hibernate-mapping xmlns="urn:nhibernate-mapping-2.2"
	namespace="NHibernateDemo.Model" assembly="NHibernateDemo">
	  <class name="Cat" table="Cat">
	    <!-- A 32 hex character is our surrogate key. It's automatically
	generated by NHibernate with the UUID pattern. -->
	    <id name="Id">
	      <column name="CatId" sql-type="char(32)" not-null="true"/>
	      <generator class="uuid.hex" />
	    </id>
	    <!-- A cat has to have a name, but it shouldn't be too long. -->
	    <property name="Name">
	      <column name="Name" length="16" not-null="true" />
	    </property>
	    <property name="Sex" />
	    <property name="Weight" />
	  </class>
	</hibernate-mapping>
##實體類Cat
	namespace NHibernateDemo.Model
	{
	    public class Cat
	    {
	        public virtual string Id { get; set; }
	        public virtual string Name { get; set; }
	        public virtual char Sex { get; set; }
	        public virtual float Weight { get; set; }
	    }
	}
##創建表
	CREATE TABLE `cat` (
		`CatId` CHAR(32) NOT NULL,
		`Name` VARCHAR(16) NOT NULL,
		`Sex` CHAR(1) NULL DEFAULT NULL,
		`Weight` DOUBLE NULL DEFAULT NULL,
		PRIMARY KEY (`CatId`)
	)
	COLLATE='utf8_general_ci'
	ENGINE=InnoDB
	;
##代碼
	ISessionFactory sessionFactory =
        new Configuration().Configure().BuildSessionFactory();
    using (ISession session = sessionFactory.OpenSession())
    {
        using (ITransaction tx = session.BeginTransaction())
        {
            var cat = new Cat
            {
                Name = "白貓",
                Sex = 'F',
                Weight = 6.5f
            };
            session.Save(cat);
            tx.Commit();
        }
    }
##Linq查詢
	var cats = session
        .Query<Cat>()
        .Where(c => c.Sex == 'F')
        .ToList();