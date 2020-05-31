using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text;

namespace CoursePlus.Shared.Models
{
    public class QuizModelBase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public int? ImageId { get; set; }
        public virtual Image Image { get; set; }

        public int? ThumbnailId { get; set; }
        public virtual Thumbnail Thumbnail { get; set; }

        public string Fournisseur { get; set; }
        public string Rédacteur { get; set; }
        public string Thème { get; set; }
        public int Difficulté { get; set; }
    }

    public class QuizModelA : QuizModelBase
    {
        public virtual QuizContentAB Quizz { get; set; }
    }

    public class QuizModelB : QuizModelBase
    {
        public QuizLanguagesB Quizz { get; set; }
    }

    public class QuizLanguagesB
    {
        public List<QuizContentAB> Fr { get; set; }
        public List<QuizContentAB> En { get; set; }
        public List<QuizContentAB> De { get; set; }
        public List<QuizContentAB> Es { get; set; }
        public List<QuizContentAB> It { get; set; }
        public List<QuizContentAB> Nl { get; set; }
    }

    public class QuizContentAB
    {
        public List<QuizItemAB> Débutant { get; set; }
        public List<QuizItemAB> Confirmé { get; set; }
        public List<QuizItemAB> Expert { get; set; }
    }

    public class QuizItemAB
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public List<string> Propositions { get; set; }
        public string Réponse { get; set; }
        public string Anecdote { get; set; }
    }
}
