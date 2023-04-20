using System.Windows.Media;

namespace wp05_bikeshop.Logics
{
    internal class Car : Notifier // 값이 바뀌는걸 인지해서 처리하겠다.
    {
        private string names;
        public string Names 
        {
            get => names;
            set
            {
                names = value;
                OnPropertyChanged("Names"); // Names 프로퍼티가 바꼇어요! 바꼇으니 처리해주세요 
            }
        } 
        private double speed;
        public double Speed 
        {
            get => speed; 
            set
            {
                speed = value;
                OnPropertyChanged(nameof(Speed)); // nameof(Speed)하면 알아서 "Speed" 들어감 == "Speed"
            }
        }
        public Color Colorz { get; set; }
        public Human Driver { get; set; } // get; set; 쓰는건 Auto Property 이기때문
    }
}
