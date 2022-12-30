# SC KRM
Simsimhan Chobo Kernel Manager   
~~School Live! Kurumi (Gakkou Gurashi)~~

UI는 [osu!lazer](https://github.com/ppy/osu)에서 영감을 받았습니다

## 라이선스
[MIT License](https://opensource.org/licenses/MIT)

## 플랫폼
- Windows
- Standalone (테스트 되지 않았음, 이론상 대부분은 작동)
- Linux (오디오를 불러오지 못함)
- Android
- iOS (``)

## 사용된 패키지와 DLL, 오픈 소스
- **Unity UI**
- **Input System**
- **TextMeshPro**
- **Post Processing**
- **Input System**
- [**Newtonsoft.Json for Unity**](https://github.com/jilleJr/Newtonsoft.Json-for-Unity/wiki/Install-official-via-UPM)
- [**Unity Asynchronous Image Loader**](https://github.com/SimsimhanChobo/UnityAsyncImageLoader) ([Original](https://github.com/Looooong/UnityAsyncImageLoader))  
(``https://github.com/SimsimhanChobo/UnityAsyncImageLoader.git`` 링크를 사용해서 설치하세요)
- [UniTask](https://github.com/Cysharp/UniTask)
- [HSV-Color-Picker-Unity](https://github.com/judah4/HSV-Color-Picker-Unity)
- [K4UnityThreadDispatcher](https://gist.github.com/heshuimu/f63cd9126117afc4004be37b19fa1c09#file-k4unitythreaddispatcher-cs)
- [UnityWindowsFileDrag&Drop](https://github.com/Bunny83/UnityWindowsFileDrag-Drop)
- [FastString](https://github.com/snozbot/FastString)
- [BigDecimal](https://github.com/AdamWhiteHat/BigDecimal)
- [KnownFolders](https://gitlab.com/Syroot/KnownFolders/-/blob/master/src/Syroot.KnownFolders/KnownFolderType.cs) (SCKRM.KnowFolder.KnownFolderType, SCKRM.KnowFolder.KnownFolderTypeExtensions, SCKRM.KnowFolder.KnownFolderGuidAttribute 클래스에서 코드 일부분이 사용됨)
- [Recyclable Scroll Rect](https://github.com/MdIqubal/Recyclable-Scroll-Rect)
- [SharpZipLib](https://github.com/icsharpcode/SharpZipLib)
- [Brigadier.NET](https://github.com/SimsimhanChobo/Brigadier.NET) ([Original](https://github.com/AtomicBlom/Brigadier.NET))
- [Simple Zoom](https://github.com/SimsimhanChobo/simple-zoom) ([Original](https://github.com/daniellochner/simple-zoom))

볼드 처리된 패키지는 이 프로젝트를 사용하기 전에 무조건 패키지를 직접 설치해주셔야 합니다 (링크가 없는것은 유니티 레지스트리에 있습니다)   
안그러면, 컴파일 에러가 나고 관련 참조가 끊어질수 있습니다

## 사용한 아이콘
- 제가 직접 만들었거나, [여기](https://www.iconfinder.com/search?q=&price=free&family=bootstrap)에서 가져왔습니다

## 버전 표기 규칙
- 기본적으로 [Semantic Versioning](https://semver.org/) 표기법을 따릅니다 (x.y.z)
- x : 대형 업데이트시에 1 올립니다 (SDJK 1.0 -> SDJK 2.0, Old SC KRM -> SC KRM 이와 같이 새롭게 만들어지거나 매우 많이 변했을경우)
- y : API, 클래스 추가, 변경과 같은 작업을 해서 변경된 사항이 클때 1 올립니다
- z : API, 클래스 추가, 변경과 같은 작업을 했지만 변경된 사항이 작을때 1 올립니다
- x가 0이라면 이는 초기 개발 버전을 의미합니다 (버전 표기법을 완벽하게 따르지 않아도 됨)

## SC KRM 적용 전 주의
- 이 프로젝트는 처음 프로젝트를 만들때 사용해야 나중에 안 귀찮아집니다
- **사용 중인 입력 처리를 '모두'로 바꿔주세요**
- UniTask가 내장 되어있습니다 
  - 만약 오류가 발생한다면, 기존에 있는 UniTask를 지워주시거나 SC KRM/UniTask 폴더를 지워주세요
- 기본적으로 스크립트는 SCKRM 네임스페이스를 가지고 있습니다
  만약, 스크립트가 안보인다면 SCKRM 뒤에 네임스페이스가 더 있을수 있으니 확인해주세요
  (예: SCKRM.Resource.ResourceManager, SCKRM.Kernel)
- SC KRM의 모든 스크립트와 리소스들을 수정하지 않는것을 추천드립니다  
  이유는 단순하게 SC KRM을 새 버전으로 업데이트하기 편합니다  
  
  호환성과 최적화를 최 우선으로 보고 있기 때문에 업데이트가 잘 되갰지만  
  혹시 모르니 업데이트 전엔 SC KRM과 스트리밍 에셋 전체를 백업을 하시는걸 추천드립니다  
  ([SC KRM Installer](https://github.com/SimsimhanChobo/SC-KRM-Installer) 프로그램을 사용하여 설치한다면, 자동으로 백업되긴 합니다)  
  
  프리팹을 수정해야 한다면 프리팹 배리언트를 사용하면 됩니다

## SC KRM 적용 방법
- [SC KRM Installer](https://github.com/SimsimhanChobo/SC-KRM-Installer) 프로그램을 사용하여, 프로젝트에 설치할 수 있습니다 (아직 최신버전 밖에 설치하지 못합니다)
- 프로그램을 사용하지 않고, 수동으로 프로젝트를 다운받아 설치할 수도 있습니다 (단, 프로젝트 설정과 같은 부분은 수동으로 합치는 작업을 해주셔야합니다)
  
## GIF 포맷은 이제 버릴때가 됐습니다!!!!!!
SC KRM은 곧 APNG 포맷을 지원할 것 입니다

## 전처리기
- `ENABLE_ANDROID_SUPPORT` (안드로이드 스트리밍 에셋 호환 코드 강제 활성화)