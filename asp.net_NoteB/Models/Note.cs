using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asp.net_NoteB.Models
{
    public class Note
    {
        /// <summary>
        /// 게시판 번호
        /// </summary>
        [ Key]  //PK 설정
        public int NoteNo { get; set; }

        /// <summary>
        /// 게시판 제목
        /// </summary>
        [Required(ErrorMessage ="제목을 입력하세요")]  //NOT NULL 설정
        public string NoteTitle { get; set; }

        /// <summary>
        /// 게시판 내용
        /// </summary>
        [Required(ErrorMessage = "내용을 입력하세요")]  //NOT NULL 설정
        public string NoteContents { get; set; }

        /// <summary>
        /// 작성자 번호
        /// </summary>
        [Required]  //NOT NULL 설정
        public int UserNo { get; set; }

        [ForeignKey("UserNo")]  //외래키 설정
        public virtual User User { get; set; }  //테이블 조인
    }
}
