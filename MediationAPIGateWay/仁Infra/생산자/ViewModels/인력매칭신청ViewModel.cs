using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using 仁Infra.생산자.DTO;

namespace 仁Infra.생산자.ViewModels
{
    public class 인력매칭신청ViewModel : ObservableObject
    {
        private 인력매칭신청DTO _인력매칭신청Data;

        public 인력매칭신청DTO 인력매칭신청Data
        {
            get => _인력매칭신청Data;
            set => SetProperty(ref _인력매칭신청Data, value);
        }

        public RelayCommand SubmitCommand { get; }

        public 인력매칭신청ViewModel()
        {
            인력매칭신청Data = new 인력매칭신청DTO(); // 초기 DTO 설정

            // Submit 커맨드 초기화
            SubmitCommand = new RelayCommand(Submit매칭신청);
        }

        private void Submit매칭신청()
        {
            // 실제 제출 로직 구현
            // 이곳에서 API 호출 등의 로직을 구현할 수 있습니다.
            // 예: API.Submit매칭신청(인력매칭신청Data);
        }
    }
}
