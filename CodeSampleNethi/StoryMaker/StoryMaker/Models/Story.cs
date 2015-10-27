//////////////////////////////////////////////////////////////////////////
// Story.cs -    Contains classes Story, Collage and StoryBlock which   //
//               has respective properties.  A story block is an HTML5  //
//               structure that contains an image or drawing, a caption,//
//               and text description, normally placed vertically in    //
//               that order of the block.collage will probably provide  //
//               additional text that spans the entire view below the   //
//               contained story blocks. story may have subsequences    //
//               that are collages of several blocks in a single view.  //
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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StoryMaker.Models
{

    // Story class 
    public class Story
    {
        [Key]
        public int storyId { get; set; }

        [Required(ErrorMessage="Story Name is Required")]
        [StringLength(30,MinimumLength = 5)]
        [Display(Name="Story Name")]
        public string storyName { get; set; }

        [Required(ErrorMessage="Story Description is Required")]
        [StringLength(300)]
        [DisplayName("Story Description")]
        public string storyDescription { get; set; }

        [StringLength(250)]
        [DisplayName("Location")]
        public string storyLocation { get; set; }

        [DisplayName("Archival Status")]
        public bool archivalStatus { get; set; }

        [DisplayName("Upload Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime uploadDate { get; set; }

        public virtual ICollection<Collage> Collages { get; set; }
        public string temp_string1 { get; set; }

        public string temp_string2 { get; set; }

        public int temp_int1 { get; set; }

        public int temp_int2 { get; set; }


    }

    public class Collage
    {
        [Key]

        public int collageId { get; set; }

        [StringLength(30,MinimumLength=5)]
        [Required(ErrorMessage="Collage Name is Required")]
        [DisplayName("Collage Name")]
        public string collageName { get; set; }

        [Required(ErrorMessage= "Collage Description is Required")]
        [StringLength(300)]
        [DisplayName("Collage Description")]
        public string collageDescription { get; set; }

        [DisplayName("Sequece Id")]
        [Required(ErrorMessage="Order is required")]
        public int orderNumber { get; set; }

        [DisplayName("Collage Location")]
        [StringLength(250)]
        public string collageLocation { get; set; }

        [DisplayName("Archival Status")]
        public bool archivalStatus { get; set; }

        [DisplayName("Upload Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime uploadDate { get; set; }

        // one-many relationship; a story can contain multiple collages
        public virtual Story story { get; set; }

        [Required]
        [ForeignKey("story")]
        [DisplayName("Story Name")]
        public virtual int storyId { get; set; }

        public virtual ICollection<StoryBlock> StoryBlocks { get; set; }

        public string temp_string1 { get; set; }

        public string temp_string2 { get; set; }

        public int temp_int1 { get; set; }

        public int temp_int2 { get; set; }

    }
    public class StoryBlock
    {
        [Key]

        public int storyBlockId { get; set; }

        [Required(ErrorMessage= "StoryBlock Name is Required")]
        [StringLength(30,MinimumLength=5)]
        [DisplayName("StoryBlock Name")]
        public string storyBlockName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        [DisplayName("Title")]
        public string storyBlockCaption { get; set; }

        [StringLength(300)]
        [DisplayName("StoryBlock Description")]
        public string storyBlockDescription { get; set; }

        [DisplayName("StoryBlock Location")]
        [StringLength(250)]
        public string storyBlockLocation { get; set; }

        [DisplayName("Sequece Id")]
        [Required(ErrorMessage = "Order is required")]
        public int storyBlockOrderNumber { get; set; }

        [DisplayName("Archival Status")]
        public bool archivalStatus { get; set; }

        [DisplayName("Upload Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime uploadDate { get; set; }
        // one-many relationship; a collage can contain multiple story-blocks
        public virtual Collage collage { get; set; }
        [Required]
        [DisplayName("Collage Name")]
        [ForeignKey("collage")]
        public virtual int collageId { get; set; }

        public string temp_string1 { get; set; }

        public string temp_string2 { get; set; }

        public int temp_int1 { get; set; }

        public int temp_int2 { get; set; }

    }

}