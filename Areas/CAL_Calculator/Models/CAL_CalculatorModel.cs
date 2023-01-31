﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Areas.CAL_Calculator.Models
{
    public class CAL_CalculatorModel
    {
        // ModelName: CAL_CalculatorModel

        /*******************************************************************
         *	FILTERS
         *******************************************************************/
        public string? C_CalculatorId { get; set; }

        public string? C_CalculatorName { get; set; }



        /*******************************************************************
         *	ADDEDIT FORM
         *******************************************************************/
        [Required, Display(Name = "CalculatorID")]
        public int CalculatorID { get; set; }

        [Required, Display(Name = "CategoryID")]
        public int CategoryID { get; set; }

        [Required, Display(Name = "CalculatorName")]
        public string? CalculatorName { get; set; }

        [Required, Display(Name = "CalculatorIcon")]
        public string? CalculatorIcon { get; set; }

        [Required, Display(Name = "URLName")]
        public string? URLName { get; set; }

        [Required, Display(Name = "HeaderName")]
        public string? HeaderName { get; set; }

        [Required, Display(Name = "SubHeaderName")]
        public string? SubHeaderName { get; set; }

        [Required, Display(Name = "CalculatorDescription")]
        public string? CalculatorDescription { get; set; }

        [Required, Display(Name = "PageSection1")]
        public string? PageSection1 { get; set; }

        [Required, Display(Name = "PageSection2")]
        public string? PageSection2 { get; set; }

        [Required, Display(Name = "PageSecton3")]
        public string? PageSecton3 { get; set; }

        [Required, Display(Name = "MetaTitle")]
        public string? MetaTitle { get; set; }

        [Required, Display(Name = "MetaKeyword")]
        public string? MetaKeyword { get; set; }

        [Required, Display(Name = "MetaDescription")]
        public string? MetaDescription { get; set; }

        [Required, Display(Name = "MetaAuthor")]
        public string? MetaAuthor { get; set; }

        [Required, Display(Name = "MetaOgTitle")]
        public string? MetaOgTitle { get; set; }

        [Required, Display(Name = "MetaOgImage")]
        public string? MetaOgImage { get; set; }

        [Required, Display(Name = "MetaOgDescription")]
        public string? MetaOgDescription { get; set; }

        [Required, Display(Name = "MetaOgUrl")]
        public string? MetaOgUrl { get; set; }

        [Required, Display(Name = "MetaOgType")]
        public string? MetaOgType { get; set; }

        [Required, Display(Name = "Sequence")]
        public int Sequence { get; set; }

        [Required, Display(Name = "Description")]
        public string? Description { get; set; }

        [Required, Display(Name = "User")]
        public int UserID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
