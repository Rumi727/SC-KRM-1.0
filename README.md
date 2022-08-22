# SC KRM
Simsimhan Chobo Kernel Manager   
~~School Live! Kurumi (Gakkou Gurashi)~~

UI는 [osu!lazer](https://github.com/ppy/osu)에서 영감을 받았습니다

## 라이선스
TEAM Bucket 블랙리스트에 등록된 사람은 등록된 기간의 모든 커밋 또는 릴리즈 등 제 모든 창작물을 이용, 변경 등을 할 수 없습니다 (2022-07-28 0:53 시간 이후의 만들어진 창작물만 해당)  
(예: a라는 사람이 2022-8-01부터 2022-8-23까지 블랙리스트에 등록되어있으면 그 기간 동안 생성된 커밋과 릴리즈를 등 올라온 모든 창작물을 사용할 수 없다는 뜻)

위에 해당하지 않는다면, 파일에 작성되어있는 라이선스를 따르게 됩니다

[TEAM Bucket 블랙리스트에 등록된 사용자 목록](https://docs.google.com/document/d/1diUFkd4drD_hroCqmRTYNVYzU_jpxQXsb45F-VvWekE/edit?usp=sharing)

이러한 라이선스가 [오픈 소스 정의](https://opensource.org/osd)와 일치하지 않다는것을 알고있습니다  
하지만 상관 없습니다  
블랙리스트에 등록될려면 적어도 테러 정돈 해야 등록될 정도로 매우 빡빡하며 등록된 사람들 전부 대부분의 사람들이 납득 가능한 이유가 있습니다

## 사용된 패키지와 DLL, 오픈 소스
- **Unity UI**
- **Input System**
- **TextMeshPro**
- **Post Processing**
- **Input System**
- [**Newtonsoft.Json for Unity**](https://github.com/jilleJr/Newtonsoft.Json-for-Unity/wiki/Install-official-via-UPM)
- [**Unity Asynchronous Image Loader**](https://github.com/Looooong/UnityAsyncImageLoader)
- [UniTask](https://github.com/Cysharp/UniTask)
- [HSV-Color-Picker-Unity](https://github.com/judah4/HSV-Color-Picker-Unity)
- [K4UnityThreadDispatcher](https://gist.github.com/heshuimu/f63cd9126117afc4004be37b19fa1c09#file-k4unitythreaddispatcher-cs)
- [UnityWindowsFileDrag&Drop](https://github.com/Bunny83/UnityWindowsFileDrag-Drop)
- [FastString](https://github.com/snozbot/FastString)
- [BigDecimal](https://github.com/AdamWhiteHat/BigDecimal)
- [KnownFolders](https://gitlab.com/Syroot/KnownFolders/-/blob/master/src/Syroot.KnownFolders/KnownFolderType.cs) (SCKRM.KnowFolder.KnownFolderType, SCKRM.KnowFolder.KnownFolderTypeExtensions, SCKRM.KnowFolder.KnownFolderGuidAttribute 클래스에서 코드 일부분이 사용됨)
- [Recyclable Scroll Rect](https://github.com/MdIqubal/Recyclable-Scroll-Rect)
- [SharpZipLib](https://github.com/icsharpcode/SharpZipLib)
- [Brigadier.NET](https://github.com/AtomicBlom/Brigadier.NET)

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

## SC KRM 적용 방법
- [SC KRM Installer](https://github.com/SimsimhanChobo/SC-KRM-Installer) 프로그램을 사용하여, 프로젝트에 설치할 수 있습니다 (아직 최신버전 밖에 설치하지 못합니다)
- 프로그램을 사용하지 않고, 수동으로 프로젝트를 다운받아 설치할 수도 있습니다 (단, 프로젝트 설정과 같은 부분은 수동으로 합치는 작업을 해주셔야합니다)

## GIF 포맷은 이제 버릴때가 됐습니다!!!!!!
SC KRM은 곧 APNG 포맷을 지원할 것 입니다
