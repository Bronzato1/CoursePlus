using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace CoursePlus.Shared.Models
{
    public class QuizTopic : IAuditable
    {
        [Required]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "{0} is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public string Description { get; set; }
        
        public int PlayCount { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public int? ImageId { get; set; }
        public virtual Image Image { get; set; }

        public int? ThumbnailId { get; set; }
        public virtual Thumbnail Thumbnail { get; set; }

        public string Provider { get; set; }
        public string Editor { get; set; }
        public string Theme { get; set; }

        [Required(ErrorMessage = "Owner is required.")]
        public int? OwnerId { get; set; }
        public Profile Owner { get; set; }

        public bool Featured { get; set; }
        public bool Popular { get; set; }

        public List<Enrollment> Enrollments { get; set; }

        public List<QuizItem> Items { get; set; }
        public List<Chapter> Chapters { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string CreatedUser { get; set; }
        public string UpdatedUser { get; set; }

        public static implicit operator QuizTopic(QuizModelA quizA)
        {
            QuizTopic quiz = new QuizTopic();

            quiz.Title = quizA.Titre;
            quiz.Editor = quizA.Rédacteur;
            quiz.Provider = quizA.Fournisseur;

            var fr = GetQuizContents(new List<QuizContentAB>() { quizA.Quizz }, EnumLanguages.French);

            quiz.Items = new List<QuizItem>();
            quiz.Items.AddRange(fr);

            return quiz;
        }
        public static implicit operator QuizTopic(QuizModelB quizB)
        {
            QuizTopic quiz = new QuizTopic();

            quiz.Title = quizB.Titre;
            quiz.Editor = quizB.Rédacteur;
            quiz.Provider = quizB.Fournisseur;

            var en = GetQuizContents(quizB.Quizz.En, EnumLanguages.English);
            var fr = GetQuizContents(quizB.Quizz.Fr, EnumLanguages.French);
            var de = GetQuizContents(quizB.Quizz.De, EnumLanguages.Deutch);
            var sp = GetQuizContents(quizB.Quizz.Es, EnumLanguages.Spanish);
            var it = GetQuizContents(quizB.Quizz.It, EnumLanguages.Italian);
            var nl = GetQuizContents(quizB.Quizz.Nl, EnumLanguages.Netherland);

            quiz.Items = new List<QuizItem>();

            quiz.Items.AddRange(en);
            quiz.Items.AddRange(fr);
            quiz.Items.AddRange(de);
            quiz.Items.AddRange(sp);
            quiz.Items.AddRange(it);
            quiz.Items.AddRange(nl);

            return quiz;
        }

        private static List<QuizItem> GetQuizItems<T>(List<T> quizItems, EnumLanguages language, EnumDifficulty difficulty) where T : QuizItemAB
        {
            List<QuizItem> items = new List<QuizItem>();

            quizItems.ForEach(y =>
            {
                QuizItem item = new QuizItem()
                {
                    Question = y.Question,
                    Answer = y.Réponse,
                    Anecdote = y.Anecdote,
                    Difficulty = difficulty,
                    Language = language
                };

                List<QuizProposal> proposals = new List<QuizProposal>();

                y.Propositions.ForEach(z => { proposals.Add(new QuizProposal { Proposition = z }); });

                item.Proposals = proposals;

                items.Add(item);
            });

            return items;
        }
        private static List<QuizItem> GetQuizContents<T>(List<T> quizItems, EnumLanguages language) where T : QuizContentAB
        {
            List<QuizItem> items = new List<QuizItem>();

            quizItems.ForEach(x =>
             {
                 var a = GetQuizItems(x.Débutant, language, EnumDifficulty.Beginner);
                 var b = GetQuizItems(x.Confirmé, language, EnumDifficulty.Confirmed);
                 var c = GetQuizItems(x.Expert, language, EnumDifficulty.Expert);
                 items.AddRange(a);
                 items.AddRange(b);
                 items.AddRange(c);
             });
            return items;
        }
    }

    public class QuizItem
    {
        public int Id { get; set; }
        public EnumLanguages Language { get; set; }
        public EnumDifficulty Difficulty { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Anecdote { get; set; }
        public List<QuizProposal> Proposals { get; set; }

        //[ForeignKey("QuizTopicId")]

        public int QuizTopicId { get; set; }
        public QuizTopic QuizTopic { get; set; }
    }

    public class QuizProposal
    {
        public int Id { get; set; }
        public string Proposition { get; set; }

        //[ForeignKey("QuizItemId")]

        public int QuizItemId { get; set; }
        public QuizItem QuizItem { get; set; }
    }
}
