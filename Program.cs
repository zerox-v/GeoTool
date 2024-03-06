// See https://aka.ms/new-console-template for more information
using GeoTool.Entity;
string runDirectory = AppDomain.CurrentDomain.BaseDirectory;
string filePath = Path.Combine(runDirectory, "ok_geo.csv");
Console.WriteLine("请输入数据库链接字符串：");
var com=Console.ReadLine();
var connectionString = "";
if (com != null) {
    connectionString = com;
}
if (!File.Exists(filePath)) {
    Console.WriteLine("未找到文件:"+ filePath);
    Console.ReadKey();
    return;
}
Console.WriteLine("开始处理数据...");

IFreeSql fsql = new FreeSql.FreeSqlBuilder()
    .UseConnectionString(FreeSql.DataType.SqlServer, connectionString)
    .UseAutoSyncStructure(true)
    .UseMonitorCommand(cmd => { })
    .Build();
using (StreamReader reader = new StreamReader(filePath))
{
    var i = 0;
    while (!reader.EndOfStream)
    {
        string line = reader.ReadLine();
        if (i!= 0) {
            var entity = new GeoTool.Entity.GeoInfo(line);
            Console.WriteLine(entity.Name+" "+entity.ExtPath);
            fsql.InsertOrUpdate<GeoInfo>()
  .SetSource(entity) 
  .ExecuteAffrows();
        }
       
        i++;
    }
    Console.WriteLine("共处理数据："+(i-1)+"条");
    Console.ReadKey();
}

