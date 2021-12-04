# SC KRM
Simsimhan Chobo Kernel Manager

## 사용된 패키지와 DLL
- **Unity UI**
- **Input System**
- **TextMeshPro**
- Newtonsoft.Json (https://github.com/JamesNK/Newtonsoft.Json)
- UniTask (https://github.com/Cysharp/UniTask)
- Better Streaming Assets (https://github.com/gwiazdorrr/BetterStreamingAssets)

볼드 처리된 패키지는 이 프로젝트를 사용하기 전에 무조건 패키지를 직접 설치해주셔야 합니다 (유니티 레지스트리에 있습니다)

안그러면, 컴파일 에러가 나고 관련 참조가 끊어질수 있습니다

## 주의
- 이 프로젝트는 처음 프로젝트를 만들때 사용해야 나중에 안 귀찮아집니다

- Newtonsoft.Json이 내장 되어있습니다

  - 만약 중복되었다는 오류가 발생한다면 기존에 있는 dll을 지워주시거나 SC KRM/Json/Newtonsoft.Json.dll를 지워주세요
  
- UniTask가 내장 되어있습니다 

  - 만약 오류가 발생한다면, 기존에 있는 UniTask를 지워주시거나 SC KRM/UniTask 폴더를 지워주세요

- Better Streaming Assets가 내장 되어있습니다
  - 만약 오류가 발생한다면, 기존에 있는 UniTask를 지워주시거나 SC KRM/BetterStreamingAssets 폴더를 지워주세요

- 기본적으로 스크립트는 SCKRM 네임스페이스를 가지고 있습니다

  만약, 스크립트가 안보인다면 SCKRM 뒤에 네임스페이스가 더 있을수 있으니 확인해주세요

  (예: SCKRM.Resource.ResourceManager, SCKRM.Kernel)

- SC KRM의 모든 스크립트와 리소스들을 수정하지 않는것을 추천드립니다

  이유는 단순하게 SC KRM을 새 버전으로 업데이트하기 편합니다

  호환성과 최적화를 최 우선으로 보고 있기 때문에 업데이트가 잘 되갰지만, 혹시 모르니 업데이트 전엔 SC KRM과 스트리밍 에셋 전체를 백업을 하시는걸 추천드립니다

  프리팹을 수정해야 한다면 프리팹 배리언트를 사용하면 됩니다
  
- ThreadManager과 ThreadMetaData은 **멀티 스레드에 안전하지 않습니다!**
  
  단순히 메인 스레드가 멈추지 않게 하기 위하거나, 로딩바를 스레드랑 같이 표시하기위해서 있는 스크립트이며, **절때로 스레드가 동시에 같은 리소스를 읽거나 변경하도록 하지 마세요**
  
  같은 리소스를 꼭 사용해야하는 경우, ConcurrentQueue로 한 스레드로 묶어서 순서대로 실행하거나, 직접 락을 거셔야합니다
  
  **스레드 메타 데이터에 있는 Remove 함수도 안전하지 않습니다!** 다만 스레드 매니저가 알아서 스레드를 개별적으로 지워주니, **직접 커스텀으로 삭제할 필요가 없습니다**
  
  이는 나중에 패치되서 스레드 스크립트 자채는 안전하게 바뀌거나(예: Remove 함수) 자동으로 관리해주게 바뀔수가 있습니다
  
  - 만약 스레드를 지원하는 내장 스크립트나 (RendererManager.AllRerender() 같은) 내부에서 스레드가 사용되는 스크립트들에서 리소스 동시 접근이 발생한경우 알려주세요

## 기능
- Minecraft에 있는 리소스팩 기능이 포함되어있습니다 ~~(리소스팩을 바꿀수 있는 GUI 포함됨)~~ (미완성, 직접 리소스팩 경로를 넣어줘야 작동)
  즉 유니티에 있는 기본 리소스 관리 시스템이 사용되지 않습니다
  사용 할 수 있기는 하지만, 웬만하면 추천하지 않습니다

- 조작키를 쉽게 변경 할 수 있고, GUI가 마련되어있습니다

- 터치, 컨트롤러 같은 키보드를 사용하지 않고서 GetKeyDown 같은 스크립트를 실행할려면 ``InputManager.KeyDownEnable InputManager.KeyToggle`` 류의 함수를 사용하면 됩니다

- 조작을 잠글 수 있고, 그걸 무시 할 수도 있습니다

- 언어 기능이 있습니다 (언어를 바꿀수 있는 GUI 포함)

- Kernel 스크립트에 확장 함수들과 최적화를 위한 여러 변수들이 있습니다.

  예를들어 Time.deltaTime 같은경우 값을 얻을때마다 함수가 호출되서 여러번 사용하면 상당한 개적화가 되지만

  Kernel.deltaTime을 사용 할 경우, 일반 변수처럼 비슷하게 작동하게 됩니다.

  직접 for로 여러번 실행시켜보시면 알게될겁니다.

- 디스코드 리치 프리센스(?)를 스크립트에서 수정 할 수 있습니다

  기본 값을 변경할려면 (앱 id 같은) 커널을 프리팹 배리언트로 새로 만든다음 그 프리팹 배리언트에서 기본 값을 수정해주시면 됩니다

- 커스텀 렌더러들이 있습니다

  기본적으로 유니티 내장 렌더러를 사용하게 되지만, 리소스팩과 호환되게 만들어주는 렌더러입니다.

  정확하겐 커스텀 렌더러 스크립트는 기존에 있던 렌더러의 스프라이트 같은거를 바꿔주는거입니다.

  단, 스트리밍 에셋에 파일이 있어야 하며

  이미지 같은 UI 컴포넌트들은 `assets/%NameSpace%/textures/gui`에 위치해있어야합니다.

  스프라이트 렌더러 같은것들은 `assets/%NameSpace%/textures`에 위치해있어야합니다.
  
- 세이브 로드 기능이 있습니다, 어트리뷰트로 자동화가 되어있습니다 ``[SaveLoad(파일 이름)] [SaveLoad]``

- 오브젝트 풀링 시스템이 만들어져 있습니다. 사용 방법은 밑에 적혀있습니다

- 마인크래프트의 PlaySound랑 거의 비슷한 기능이 만들어져있습니다 관련 함수는 `SoundManager.PlaySound() SoundManager.StopSound() SoundManager.StopSoundAll()` 입니다

  파일은 `assets/%NameSpace%/sounds`에 위치해 있어야하며
  
  마인크래프트 처럼 sounds.json이 필요합니다
  
  - sounds.json 세팅은 ``커널 창/프로젝트 설정/오디오 설정``에서 할 수 있습니다
  
- 윈도우에서는 무려 창 위치와 크기를 마음대로 지지고 볶을 수 있습니다! 리듬게임에 들어간다면 금상첨화죠.

- 무려 NBS를 로드하고 재생 할 수 있습니다! 관련 함수는 ``SoundManager.PlayNBS() SoundManager.StopNBS() SoundManager.StopNBSAll()`` 입니다

- 등등...

## 사용 방법
- 스크립트에서 수동으로 리소스팩에 있는 에셋을 가져올려면 `ResourcesManager.Search`류의 함수를 사용하면 됩니다.

- 조작키 설정은 ``커널 창/프로젝트 설정/조작 키 설정``으로 가서 수정하거나, 스트리밍 에셋에 있는 projectSettings 폴더에가서 `SCKRM.Input.InputManager+Data.ControlSetting`를 수정하면 됩니다.

- 오브젝트 풀링에서 가져올 오브젝트를 설정하는건 `커널 창/프로젝트 설정/오브젝트 풀링 설정` 가서 수정하거나, 스트리밍 에셋에 있는 projectSettings 폴더에 가서 `SCKRM.Object.ObjectPoolingSystem+Data.PrefabList`를 수정하면 됩니다.

- 오브젝트 풀링 관련 함수는 `ObjectPollingSystem.ObjectCreate() ObjectPollingSystem.ObjectRemove()` 입니다.

- 오브젝트 풀링으로 관리하는 오브젝트들은 메인 스크립트가 ``ObjectPooling``를 상속하는것을 추천 드립니다 (만약 상속하는 스크립트가 단 한개도 없다면, 자동으로 스크립트가 추가됩니다)

- 오디오 로드 설정은 ``커널 창/프로젝트 설정/오디오 설정``에서 할 수 있습니다

- 스크립트에서 언어 파일을 수동으로 불러올려면 `LanguageManager.TextLoad()` 함수를 사용하면 됩니다.

- 윈도우에서 창 위치와 크기를 마음대로 지지고 볶을려면 `WindowManager.SetWindowRect(Rect rect, Vector2 windowDatumPoint, Vector2 screenDatumPoint, bool clientDatum = true, bool Lerp = false, float time = 0.05f)` 함수를 사용하시면 됩니다.
  
  - 들어가기에 앞서, 윈도우는 유니티랑 y좌표가 반대입니다. 즉 맨 위가 y의 0이 됩니다.
  
  - rect의 x와 y는 창의 위치이고 width랑 height는 창의 크기를 담당합니다.
  
  - windowDatumPoint는 창의 중심점을 설정합니다, 즉 x1 y1으로 설정하면 창의 오른쪽 아래가 중심점이 됩니다.
  
  - screenDatumPoint는 화면의 중심점을 설정합니다, 즉 x1 y1으로 설정하면 화면의 오른쪽 아래가 중심점이 됩니다.
  
  - clientDatum은 켜져있으면 크기를 설정할때 윈도우의 보더를 포함하지 않고 크기를 설정하지만 (`Screen.SetResolution`랑 동일) 꺼져있으면 윈도우의 보더를 포함해서 크기를 설정합니다.

- 대부분의 프로젝트 설정은 상단의 `커널/커널 설정`에서 변경 할 수 있고, 디버깅도 가능합니다
