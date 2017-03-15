using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cabshare
{
    public class DBquery
    {
        public static cleandata Clean(LUIS data)
        {
            cleandata clean = new cleandata();
            clean.origin = data.entities.Where(b => b.type == "location::fromlocation").FirstOrDefault().entity;
            clean.dest = data.entities.Where(b => b.type == "location::tolocation").FirstOrDefault().entity;
            clean.date = data.entities.Where(b => (b.type == "builtin.datetime.date" && b.resolution.time == null)).FirstOrDefault().resolution.date;
            clean.time = data.entities.Where(b => (b.type == "builtin.datetime.date" && b.resolution.date == null)).FirstOrDefault().resolution.time;
            return clean;
        }
        public static List<Request>  dataquery(cleandata data)
        {
            /*List<queryout> matchlist = new List<queryout>();
            return matchlist;*/
            using (var DB = new travelrecordEntities())
            {
                var match = (from b in DB.Requests where (b.origin == data.origin || data.origin == null) && (b.destination == data.dest || data.dest == null) && (b.date1 == data.date || data.date == null) && (b.time1 == data.time.Value.TimeOfDay || data.time == null) select b).ToList();
                return match;
                
            }
        }
    }
}