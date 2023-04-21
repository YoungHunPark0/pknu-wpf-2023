using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wp08_personalInfoApp_mvvm.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        // View에서 사용할 멤버변수 선언
        // 입력쪽 변수
        private string inFirstName;
        private string inLastName;
        private string inEmail;
        private DateTime inDate;

        // 결과 출력쪽 변수
        private string outFirstName;
        private string outLastName;
        private string outEmail;
        private string outDate; // 생일날짜 출력할때는 문자열로 대체
        private string outAdult;
        private string outBirthDay;
        private string outZodiac;

        // 일이 많아짐. 실제로 상용할 속성
        // 입력을 위한 속성들
        public string InFirstName 
        { get => inFirstName; 
          set
          {
            inFirstName = value;
                RaisePropertyChanged(nameof(InFirstName)); // "InfirstName" 전체시스템에 알려줌
          }
        }

        public string InLastName 
        { 
            get => inLastName; 
            set
            {
                inLastName = value;
                RaisePropertyChanged(nameof(InLastName)); // "InLastName" 전체시스템에 알려줌
            }
        }

        public string InEmail 
        { 
            get => inEmail; 
            set
            {
                inEmail = value;
                RaisePropertyChanged(nameof(InEmail)); // "InEmail" 전체시스템에 알려줌
            }
        }

        public DateTime InDate 
        { 
            get => inDate; 
            set
            {
                inDate = value;
                RaisePropertyChanged(nameof(InDate)); // "InDate" 전체시스템에 알려줌
            }
        }

        // 출력을 위한 속성들
        public string OutFirstName 
        { 
            get => outFirstName; 
            set
            {
                outFirstName = value;
                RaisePropertyChanged(nameof(OutFirstName)); // "OutFirstName" 전체시스템에 알려줌
            }
        }
        public string OutLastName 
        { get => outLastName; 
          set
          {
                outLastName = value;
                RaisePropertyChanged(nameof(OutLastName)); // "OutLastName" 전체시스템에 알려줌
          }
        }
        public string OutEmail 
        {
            get => outEmail; 
            set
            {
                outEmail = value;
                RaisePropertyChanged(nameof(OutEmail)); // "OutEmail" 전체시스템에 알려줌
            }
        }
        public string OutDate 
        { 
            get => outDate; 
            set
            {
                outDate = value;
                RaisePropertyChanged(nameof(OutDate)); // "OutDate" 전체시스템에 알려줌
            }
        }
        public string OutAdult 
        {
            get => outAdult; 
            set
            {
                outAdult = value;
                RaisePropertyChanged(nameof(OutAdult)); // "OutAdult" 전체시스템에 알려줌
            }
        }
        public string OutBirthDay 
        {
            get => outBirthDay; 
            set
            {
                outBirthDay = value;
                RaisePropertyChanged(nameof(OutBirthDay)); // "OutBirthDay" 전체시스템에 알려줌
            }
        }
        public string OutZodiac 
        {
            get => outZodiac; 
            set
            {
                outZodiac = value;
                RaisePropertyChanged(nameof(OutZodiac)); // "OutZodiac" 전체시스템에 알려줌
            }
        }
    }
}
