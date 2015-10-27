//////////////////////////////////////////////////////////////////////////
// StoryMakerContext.cs -    Creates DBContext for classes Story,       //
//                           Collage and StoryMaker Classes.            //
//                                                                      //
// Language:     C#                                                     //
// Platform:     Lenovo G 50-80, Windows 8.1,                           //
//               .Net Version 4.5, MVC 5                                //
// Application:  CSE686 - Internet Programming, Spring 2015             //
// Author:       Rambabu Nethi, Syracuse University                     //
//               (315) 728-8883, rnethi@syr.edu                         //
// Release Date: Version 1.0 May 1st - 2015                             //
//////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace StoryMaker.Models
{
    public class StoryMakerContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public StoryMakerContext() : base("name=StoryMakerContext")
        {
        }

        public System.Data.Entity.DbSet<StoryMaker.Models.Story> Stories { get; set; }

        public System.Data.Entity.DbSet<StoryMaker.Models.Collage> Collages { get; set; }

        public System.Data.Entity.DbSet<StoryMaker.Models.StoryBlock> StoryBlocks { get; set; }
    
    }
}
