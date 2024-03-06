using FreeSql.DataAnnotations;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;

namespace GeoTool.Entity
{
    [Table(Name = "geo_info")]
    public class GeoInfo
    {

        public int Id { get; set; }

        public int Pid { get; set; }

        public int Dep { get; set; }

        public string Name { get; set; }

        public string ExtPath { get; set; }

        [Column(
             DbType = "geometry",
             RewriteSql = "geometry::STGeomFromText({0},0).MakeValid()",
             RereadSql = "{0}.STAsText()"
             )]
        public string Geo { get; set; }

        [Column(
            DbType = "geometry",
            RewriteSql = "geometry::STGeomFromText({0},0).MakeValid()",
            RereadSql = "{0}.STAsText()"
            )]
        public string Polygon { get; set; }


        public GeoInfo(string data)
        {
            if (!string.IsNullOrEmpty(data)) {
                var dataArr = data.Split(",");
                Id = Convert.ToInt32(dataArr[0]);
                Pid = Convert.ToInt32(dataArr[1]);
                Dep = Convert.ToInt32(dataArr[2]);
                Name = dataArr[3].Trim().TrimStart('"').TrimEnd('"');
                ExtPath = dataArr[4].Trim().TrimStart('"').TrimEnd('"');
                var geo = dataArr[5].Trim().TrimStart('"').TrimEnd('"');

                if (geo != "EMPTY")
                {
                    Geo = $"POINT ({geo})";
                }

                var polygon = data.Replace(String.Join(',', dataArr.Take<string>(6)) + ",", "").Trim().TrimStart('"').TrimEnd('"');
                if (polygon != "EMPTY")
                {
                    var x = polygon.Split(";");
                    if (x.Count() > 1)
                    {
                        var list = new List<string>();
                        foreach (var item in x)
                        {
                            var item2 = item.Split(',').ToList();
                            item2.Add(item2[0]);
                            list.Add($"(({string.Join(',', item2)}))");
                        }
                      
                        Polygon = $"MULTIPOLYGON ({string.Join(',', list)})";
                    }
                    else
                    {
                        var list = polygon.Split(',').ToList();
                        list.Add(list[0]);
                        Polygon = $"POLYGON (({string.Join(',', list)}))";
                    }

                }
            }

          

        }

    }
}
